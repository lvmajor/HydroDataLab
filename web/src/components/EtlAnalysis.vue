<script setup lang="ts">
import { computed, ref } from 'vue'
import type { Report, Series } from '../types'
import { number, dateTime } from '../format'
import SeriesChart from './SeriesChart.vue'
const props = defineProps<{ report: Report }>()
const step = ref(0), showHours = ref(false)
const analysis = computed(() => props.report.analysis!)
const etl = computed(() => props.report.etl!)
const metrics = computed(() => analysis.value.indicators)
const hourlySeries = computed<Series[]>(() => [{ id: 'hourly', label: 'Demande moyenne par heure complète', station: 'Québec', unit: 'MW', kind: 'demand', intervalMinutes: 60, minimum: null, maximum: null, latest: null, coverage: null, points: analysis.value.hourly.map(h => ({ timestamp: h.startUtc, value: h.meanDemandMW })) }])
const artifact = (name: string) => `/api/artifacts/${encodeURIComponent(props.report.runId)}/${encodeURIComponent(name)}`
</script>
<template>
  <section id="etl" class="etl-section" aria-labelledby="etl-title">
    <div class="section-line"><div><p class="eyebrow">Le travail de Python · Extract → Transform → Load</p><h2 id="etl-title">Des mesures brutes aux réponses métier</h2></div><span>{{ report.period.start }} · {{ report.period.timezone }}</span></div>
    <p class="section-intro">Explorer les étapes de cette exécution. Les résultats ci-dessous sont calculés par Python, puis transmis à Vue.</p>
    <div class="etl-steps" role="group" aria-label="Étapes ETL à explorer">
      <button v-for="(label, i) in ['1 · Extraire', '2 · Transformer', '3 · Charger / exporter']" :key="i" :aria-pressed="step === i" :class="{ selected: step === i }" @click="step = i"><strong>{{ label }}</strong><span>{{ i === 0 ? `${etl.rawCount} lignes sources` : i === 1 ? `${etl.normalizedCount} lignes normalisées` : `${analysis.hourly.length} heures de demande` }}</span></button>
    </div>
    <div class="etl-content">
      <template v-if="step === 0">
        <h3>Avant · les champs du fournisseur</h3><p>Extrait des quatre premières lignes de demande reçues. Les fichiers sources sont conservés tels quels; leurs noms de champs et leur format de date sont ceux du fournisseur.</p>
        <div class="table-scroll" role="region" aria-label="Échantillon brut de demande" tabindex="0"><table><thead><tr><th>date</th><th>valeurs_demandetotal</th></tr></thead><tbody><tr v-for="(row, i) in etl.rawDemandSample" :key="i"><td>{{ row.date }}</td><td>{{ row.valeurs_demandetotal ?? 'Absente' }}</td></tr></tbody></table></div>
        <p class="quiet">{{ etl.rawCount }} lignes au total pour la demande, le niveau d’eau et les températures. La source publique est détaillée en bas de page.</p>
      </template>
      <template v-else-if="step === 1">
        <h3>Après · un schéma commun, prêt pour l’analyse</h3><p>Python convertit les dates en UTC, vérifie les unités et les nombres, puis conserve la dernière occurrence de chaque série et instant. Les quatre premières lignes normalisées de demande sont affichées ci-dessous.</p>
        <div class="table-scroll" role="region" aria-label="Échantillon normalisé de demande" tabindex="0"><table><thead><tr><th>timestamp_utc</th><th>value</th><th>unit</th></tr></thead><tbody><tr v-for="row in etl.normalizedDemandSample" :key="row.timestamp_utc"><td>{{ row.timestamp_utc }}</td><td>{{ number(row.value, 2) }}</td><td>{{ row.unit }}</td></tr></tbody></table></div>
        <p class="quiet">{{ etl.validCount }} valeurs numériques · {{ etl.missingCount }} valeurs laissées absentes · {{ report.summary.duplicateCount }} doublons retirés. Zéro reste une mesure valide.</p>
        <p v-if="etl.scenarioRemovedCount" class="notice error">Scénario de démonstration : {{ etl.scenarioRemovedCount }} mesure de niveau d’eau retirée de la copie en mémoire, sans modifier la source.</p>
      </template>
      <template v-else>
        <h3>Résultat · une table horaire réutilisable</h3><p>Quatre créneaux valides de 15 minutes donnent une moyenne horaire : somme des quatre valeurs ÷ 4. Une heure incomplète reste absente; aucune interpolation.</p>
        <p class="quiet">{{ analysis.completeHours }} heures complètes sur {{ analysis.finishedHours }} heures terminées. Les journées de changement d’heure comptent 23 ou 25 heures.</p>
        <SeriesChart :series="hourlySeries" unit="MW" :timezone="report.period.timezone" />
        <div class="section-line"><button class="text-button" :aria-expanded="showHours" @click="showHours = !showHours">{{ showHours ? 'Masquer la synthèse horaire' : 'Afficher la synthèse horaire' }}</button><a v-if="report.artifacts.hourly" :href="artifact(report.artifacts.hourly)" download>Télécharger la synthèse CSV ↓</a></div>
        <div v-if="showHours" class="table-scroll" role="region" aria-label="Synthèse horaire" tabindex="0"><table><caption>Début d’heure locale avec décalage UTC : les heures répétées restent distinctes.</caption><thead><tr><th>Heure locale</th><th>Mesures valides</th><th>Moyenne (MW)</th></tr></thead><tbody><tr v-for="hour in analysis.hourly" :key="hour.startUtc"><td>{{ hour.localHour }}</td><td>{{ hour.observed }} / {{ hour.expected }}</td><td>{{ hour.meanDemandMW == null ? 'Heure incomplète' : number(hour.meanDemandMW, 2) }}</td></tr></tbody></table></div>
      </template>
    </div>
    <h3 class="metrics-title">Ce que ces données nous permettent de dire</h3>
    <div class="business-metrics">
      <article><h4>Pointe horaire moyenne</h4><p class="metric-value">{{ number(metrics.peakHourlyMW, 2) }} <span>MW</span></p><p v-if="metrics.peakHourUtc">{{ dateTime(metrics.peakHourUtc, report.period.timezone) }}</p><p>Maximum des moyennes des heures complètes. Une heure manquante peut cacher une pointe plus élevée.</p></article>
      <article><h4>Énergie demandée estimée</h4><p class="metric-value">{{ number(metrics.estimatedEnergyMWh, 2) }} <span>MWh</span></p><p>Σ (demande en MW × 0,25 h). Chaque mesure est supposée constante pendant 15 minutes.</p><p>{{ metrics.estimatedEnergyMWh == null ? 'Non calculée : période non terminée ou au moins un créneau absent.' : 'Estimation pédagogique sur la période entière, pas une mesure de facturation.' }}</p></article>
      <article><h4>Variation du niveau observé</h4><p class="metric-value">{{ number(metrics.waterChangeCm, 2) }} <span>cm</span></p><p>(Dernier niveau − premier niveau) × 100.</p><p v-if="metrics.waterStartUtc && metrics.waterEndUtc">Du {{ dateTime(metrics.waterStartUtc, report.period.timezone) }} au {{ dateTime(metrics.waterEndUtc, report.period.timezone) }}.</p><p>Gouin Nord. Ni volume d’eau ni réserve d’énergie; des mesures manquantes peuvent raccourcir cet intervalle.</p></article>
    </div>
    <p class="quiet">Analyse descriptive de mesures passées. Aucune prévision de demande, de météo ou de réserve.</p>
  </section>
</template>
