<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue'
import SeriesChart from './components/SeriesChart.vue'
import { api, ApiError } from './api'
import { coverage, dateTime, freshness, number, pointsInWindow, safeSourceUrl } from './format'
import type { Report, Run, Schedule, Series, Source } from './types'
const report = ref<Report | null>(null), runs = ref<Run[]>([]), loading = ref(true), posting = ref(false), scheduleSaving = ref(false)
const connectionError = ref(''), actionError = ref(''), announcement = ref(''), scheduleError = ref('')
const source = ref<Source>('fixture'), missing = ref(false), windowHours = ref(0), showTable = ref(false)
const schedule = ref<Schedule>({ enabled: false, intervalMinutes: 5 }), scheduleLoaded = ref(false)
const now = ref(Date.now())
let timer: ReturnType<typeof setTimeout> | undefined, disposed = false
const active = computed(() => runs.value.find(r => r.status === 'queued' || r.status === 'running'))
const totalCoverage = computed(() => coverage(report.value?.series ?? []))
const dataEnd = computed(() => Math.max(0, ...(report.value?.series.flatMap(s => s.points.map(p => Date.parse(p.timestamp)).filter(Number.isFinite)) ?? [])))
const filteredSeries = computed(() => report.value?.series.map(s => ({ ...s, points: pointsInWindow(s.points, windowHours.value, dataEnd.value) })) ?? [])
const selectedSeries = (ids: string[]) => filteredSeries.value.filter(s => ids.includes(s.id))
const series = (id: string) => report.value?.series.find(s => s.id === id)
const latestLabel = (id: string, digits = 0) => number(series(id)?.latest, digits)
const allRows = computed(() => filteredSeries.value.flatMap(s => s.points.map(p => ({ ...p, label: s.label, station: s.station, unit: s.unit }))).sort((a, b) => Date.parse(b.timestamp) - Date.parse(a.timestamp)).slice(0, 300))
const labels: Record<string, string> = { queued: 'En attente', running: 'En cours', succeeded: 'Terminé', failed: 'Échec', interrupted: 'Interrompu' }
const stages: Record<string, string> = { queued: 'Dans la file', 'extract-transform-export': 'Collecte et transformation', persist: 'Enregistrement', complete: 'Rapport prêt' }
const sourceLabel = (value: Source) => value === 'fixture' ? 'Jeu de démonstration' : 'Sources ouvertes'
const exportUrl = (key: 'csv' | 'parquet') => report.value ? `/api/artifacts/${encodeURIComponent(report.value.runId)}/${encodeURIComponent(report.value.artifacts[key])}` : '#'
async function refresh() {
  try {
    const [nextRuns, nextReport] = await Promise.all([api<Run[]>('/runs'), api<Report>('/report').catch(error => { if (error instanceof ApiError && error.status === 404) return null; throw error })])
    if (disposed) return
    runs.value = nextRuns
    if (report.value?.runId !== nextReport?.runId) report.value = nextReport
    connectionError.value = ''
    if (!scheduleLoaded.value) {
      try { schedule.value = await api<Schedule>('/schedule'); scheduleLoaded.value = true; scheduleError.value = '' }
      catch { scheduleError.value = 'La planification est momentanément indisponible.' }
    }
  } catch { if (!disposed) connectionError.value = 'Le service de données est inaccessible. Vérifiez le démarrage de la démonstration. Une nouvelle tentative est automatique.' }
  finally { if (!disposed) { loading.value = false; now.value = Date.now(); timer = setTimeout(refresh, 3000) } }
}
async function launch(simulateFailure = false) {
  posting.value = true; actionError.value = ''; announcement.value = ''
  try { await api('/runs', { method: 'POST', body: JSON.stringify({ source: source.value, simulateFailure, qualityScenario: missing.value ? 'missing' : 'complete' }) }); announcement.value = simulateFailure ? 'Exécution lancée avec un échec contrôlé.' : 'Collecte lancée. Suivez son avancement ci-dessous.'; runs.value = await api<Run[]>('/runs') }
  catch (error) { actionError.value = error instanceof ApiError && error.status === 409 ? 'Une collecte est déjà en cours. Attendez sa fin pour en lancer une autre.' : error instanceof Error ? error.message : 'La collecte n’a pas démarré.' }
  finally { posting.value = false }
}
async function retry(id: string) {
  posting.value = true; actionError.value = ''
  try { await api(`/runs/${encodeURIComponent(id)}/retry`, { method: 'POST' }); announcement.value = 'Nouvelle tentative lancée sans échec simulé.'; runs.value = await api<Run[]>('/runs') }
  catch (error) { actionError.value = error instanceof Error ? error.message : 'La nouvelle tentative a échoué.' }
  finally { posting.value = false }
}
async function saveSchedule() {
  scheduleSaving.value = true; scheduleError.value = ''
  if (!Number.isInteger(schedule.value.intervalMinutes) || schedule.value.intervalMinutes < 1 || schedule.value.intervalMinutes > 60) { scheduleError.value = 'Choisissez un intervalle entier de 1 à 60 minutes.'; scheduleSaving.value = false; return }
  try { schedule.value = await api<Schedule>('/schedule', { method: 'PUT', body: JSON.stringify(schedule.value) }); announcement.value = schedule.value.enabled ? 'Planification activée.' : 'Planification désactivée.' }
  catch (error) { scheduleError.value = error instanceof Error ? error.message : 'La planification n’a pas été enregistrée.' }
  finally { scheduleSaving.value = false }
}
onMounted(() => { void refresh() })
onUnmounted(() => { disposed = true; clearTimeout(timer) })
</script>
<template>
  <div class="site-shell">
    <header class="topbar"><a class="brand" href="#" aria-label="HydroDataLab, accueil"><svg viewBox="0 0 36 36" aria-hidden="true"><path d="M5 25c7-15 12 12 26-13M5 17c8-15 14 9 26-12" fill="none" stroke="currentColor" stroke-width="3.5" stroke-linecap="round"/></svg><span>Hydro<span class="brand-light">DataLab</span></span></a><nav aria-label="Navigation principale"><a href="#observatoire">Observatoire</a><a href="#operations">Exploitation</a><a href="#sources">Sources</a></nav><span class="lab-badge">Laboratoire de données ouvertes</span></header>
    <main>
      <div v-if="connectionError" class="notice error" role="alert">{{ connectionError }} <span v-if="report">Les dernières données reçues restent affichées.</span></div>
      <section id="observatoire" class="intro"><div><p class="intro-location"><span class="location-dot"></span>Québec · Réservoir Gouin</p><h1>Les données prennent<br>une autre dimension.</h1><p class="intro-copy">Demande électrique, niveau d’eau et météo : trois regards sur le territoire, réunis dans une même lecture.</p></div><div class="intro-meta"><span :class="['source-pill', { live: report?.source === 'live' }]">{{ report ? sourceLabel(report.source) : 'En attente de données' }}</span><p v-if="report">Rapport produit {{ freshness(report.generatedAtUtc, now) }}<br><span>{{ dateTime(report.generatedAtUtc) }}</span></p><p v-else>Une chaîne de données vérifiable.<br><span>De la collecte jusqu’à la visualisation.</span></p></div></section>
      <div class="workspace">
        <div class="observation-area">
          <div class="period-bar"><div><strong>{{ report ? `${report.period.start} au ${report.period.end}` : 'Votre première exploration' }}</strong><span>{{ report ? `Fin exclusive · ${report.period.timezone}` : 'Lancez une collecte depuis le panneau d’exploitation.' }}</span></div><div v-if="report" class="segmented" role="group" aria-label="Période visible"><button v-for="choice in [{ label: 'Tout', value: 0 }, { label: '24 h', value: 24 }, { label: '6 h', value: 6 }]" :key="choice.value" :aria-pressed="windowHours === choice.value" :class="{ selected: windowHours === choice.value }" @click="windowHours = choice.value">{{ choice.label }}</button></div></div>
          <div v-if="loading" class="empty-state"><span class="spinner"></span><h2>Connexion à l’observatoire</h2><p>Chargement du dernier rapport et des traitements.</p></div>
          <div v-else-if="!report" class="empty-state"><svg viewBox="0 0 300 100" aria-hidden="true"><path d="M0 65H300M0 35H300" stroke="#d9e8f3"/><path d="M10 70L60 62L100 30L150 45L200 20L250 38L290 15" fill="none" stroke="#bfd8ea" stroke-width="3" stroke-dasharray="6 8"/></svg><h2>Le territoire attend ses premières mesures.</h2><p>Chargez le jeu de démonstration pour explorer les courbes et les contrôles de qualité. Vous pourrez ensuite interroger les sources ouvertes.</p><button class="primary-button" :disabled="posting || !!active || !!connectionError" @click="source = 'fixture'; launch()">Lancer le jeu de démonstration</button><small>Les mesures et leur provenance apparaîtront après la collecte.</small></div>
          <template v-else>
            <section class="demand-panel data-panel" aria-labelledby="demand-title"><div class="panel-heading"><div><span class="series-context">Réseau du Québec</span><h2 id="demand-title">Demande électrique</h2></div><div class="headline-value">{{ latestLabel('demand') }} <span>MW</span><small>Dernière mesure</small></div></div><SeriesChart :series="selectedSeries(['demand'])" unit="MW" :timezone="report.period.timezone"/><div class="range-strip"><span>Minimum <strong>{{ number(series('demand')?.minimum) }} MW</strong></span><span>Maximum <strong>{{ number(series('demand')?.maximum) }} MW</strong></span><span>Pas de mesure <strong>{{ series('demand')?.intervalMinutes ?? '—' }} min</strong></span></div></section>
            <div class="secondary-panels"><section class="data-panel water-panel" aria-labelledby="water-title"><div class="panel-heading"><div><span class="series-context">Réservoir Gouin</span><h2 id="water-title">Niveau d’eau</h2></div><div class="headline-value smaller">{{ latestLabel('water_level', 2) }} <span>m</span><small>Dernière mesure</small></div></div><SeriesChart :series="selectedSeries(['water_level'])" unit="m" :colors="['#139ca8']" compact :timezone="report.period.timezone"/><p class="metric-note">Une hauteur mesurée en mètres. Ce niveau ne représente pas un pourcentage de réserve.</p></section>
            <section class="data-panel weather-panel" aria-labelledby="weather-title"><div class="panel-heading"><div><span class="series-context">{{ series('temperature_min')?.station ?? 'Station météorologique' }}</span><h2 id="weather-title">Température horaire</h2></div><span class="unit-chip">°C</span></div><SeriesChart :series="selectedSeries(['temperature_min', 'temperature_max'])" unit="°C" :colors="['#3271ce', '#c17625']" compact :timezone="report.period.timezone"/><p class="metric-note">Les minima et maxima horaires restent deux séries distinctes. Une valeur absente crée une rupture.</p></section></div>
            <section class="quality-panel" aria-labelledby="quality-title"><div><span class="quality-symbol" aria-hidden="true">✓</span><div><h2 id="quality-title">Une lecture transparente</h2><p>{{ number(report.summary.observationCount) }} observations · {{ report.summary.seriesCount }} séries</p></div></div><dl><div><dt>Couverture</dt><dd :class="{ amber: totalCoverage != null && totalCoverage < 100 }">{{ totalCoverage == null ? 'Non calculée' : `${number(totalCoverage, 1)} %` }}</dd></div><div><dt>Valeurs invalides</dt><dd>{{ number(report.summary.invalidCount) }}</dd></div><div><dt>Doublons</dt><dd>{{ number(report.summary.duplicateCount) }}</dd></div></dl></section>
            <p v-if="totalCoverage == null" class="coverage-note">La couverture n’a pas encore été calculée. Une absence de résultat ne signifie pas 100 %.</p>
            <ul v-if="report.warnings.length" class="warnings"><li v-for="warning in report.warnings" :key="warning">{{ warning }}</li></ul>
            <details class="coverage-details"><summary>Qualité par série</summary><table><thead><tr><th>Série</th><th>Observées / attendues</th><th>Couverture</th></tr></thead><tbody><tr v-for="s in report.series" :key="s.id"><td>{{ s.label }}</td><td>{{ s.coverage ? `${number(s.coverage.observed)} / ${number(s.coverage.expected)}` : 'Non calculée' }}</td><td>{{ s.coverage ? `${number(s.coverage.percent, 1)} %` : '—' }}</td></tr></tbody></table></details>
            <section class="data-table-section"><div class="section-line"><h2>Les mesures, à la source</h2><button class="text-button" :aria-expanded="showTable" @click="showTable = !showTable">{{ showTable ? 'Masquer le tableau' : 'Afficher le tableau' }}</button></div><div v-if="showTable" class="table-scroll"><table><caption>300 mesures les plus récentes de la période visible, heure de Toronto.</caption><thead><tr><th>Date et heure</th><th>Série</th><th>Station</th><th>Valeur</th></tr></thead><tbody><tr v-for="(row, index) in allRows" :key="index"><td>{{ dateTime(row.timestamp, report.period.timezone) }}</td><td>{{ row.label }}</td><td>{{ row.station }}</td><td :class="{ amber: row.value == null }">{{ row.value == null ? 'Absente' : number(row.value, 2) }} <span v-if="row.value != null">{{ row.unit }}</span></td></tr></tbody></table></div></section>
          </template>
        </div>
        <aside id="operations" class="operations" aria-labelledby="operations-title"><div class="operations-heading"><span class="operations-icon" aria-hidden="true">↻</span><h2 id="operations-title">Le poste de contrôle</h2><p>Collecter. Transformer. Explorer.</p></div><form @submit.prevent="launch()"><label class="field-label" for="source">Source de la collecte</label><select id="source" v-model="source"><option value="fixture">Jeu de démonstration</option><option value="live">Sources ouvertes en direct</option></select><p class="input-hint">{{ source === 'fixture' ? 'Un jeu local reproductible pour la formation.' : 'Interroge les services publics. Un accès réseau est nécessaire.' }}</p><label class="checkbox-line"><input v-model="missing" type="checkbox"/><span>Retirer une mesure de niveau d’eau<small>Scénario de qualité, sans modifier la source.</small></span></label><button class="primary-button full-width" :disabled="posting || !!active || !!connectionError" type="submit"><span v-if="posting || active" class="spinner small"></span>{{ active ? 'Collecte en cours' : posting ? 'Démarrage…' : 'Lancer la collecte' }}</button><button type="button" class="secondary-button full-width" :disabled="posting || !!active || !!connectionError" @click="launch(true)">Démontrer un échec contrôlé</button></form><p v-if="actionError" class="inline-error" role="alert">{{ actionError }}</p><p v-if="announcement" class="announcement" role="status">{{ announcement }}</p>
          <div v-if="active" class="active-run" role="status"><div class="pulse-dot"></div><div><strong>{{ labels[active.status] }}</strong><span>{{ stages[active.stage] ?? active.stage }}</span><small>Depuis {{ dateTime(active.startedAtUtc || active.createdAtUtc) }}</small></div></div>
          <section class="run-history"><div class="section-line"><h3>Dernières exécutions</h3><span>{{ runs.length }}</span></div><p v-if="!runs.length" class="quiet">Les traitements apparaîtront ici.</p><ol v-else><li v-for="run in runs.slice(0, 5)" :key="run.id"><div class="run-line"><span :class="['status-mark', run.status]" aria-hidden="true">{{ run.status === 'succeeded' ? '✓' : run.status === 'failed' || run.status === 'interrupted' ? '!' : '·' }}</span><div><strong>{{ sourceLabel(run.source) }}</strong><small>{{ dateTime(run.createdAtUtc) }}</small></div><span :class="['status-text', run.status]">{{ labels[run.status] }}</span></div><p v-if="run.qualityScenario === 'missing'" class="run-detail">Une mesure retirée</p><p v-if="run.error" class="run-error">{{ run.error }}</p><button v-if="run.status === 'failed' || run.status === 'interrupted'" class="text-button retry" :disabled="posting || !!active" @click="retry(run.id)">Relancer ce traitement</button></li></ol></section>
          <details class="schedule-section"><summary>Collecte périodique</summary><form @submit.prevent="saveSchedule"><label class="checkbox-line"><input v-model="schedule.enabled" :disabled="!scheduleLoaded" type="checkbox"/><span>Activer la planification</span></label><label class="field-label" for="interval">Intervalle en minutes</label><div class="schedule-input"><input id="interval" v-model.number="schedule.intervalMinutes" :disabled="!scheduleLoaded" type="number" min="1" max="60" step="1"/><button class="secondary-button" :disabled="!scheduleLoaded || scheduleSaving" type="submit">Enregistrer</button></div><p class="input-hint">De 1 à 60 minutes. Le service conserve ce réglage.</p><p v-if="scheduleError" class="inline-error" role="alert">{{ scheduleError }}</p></form></details>
          <section class="exports"><h3>Emporter les données</h3><p>Exports du dernier traitement réussi.</p><div v-if="report" class="export-links"><a :href="exportUrl('csv')" download><span>CSV</span><span aria-hidden="true">↓</span></a><a :href="exportUrl('parquet')" download><span>Parquet</span><span aria-hidden="true">↓</span></a></div><p v-else class="quiet">Disponibles après la première collecte.</p></section>
        </aside>
      </div>
      <section id="sources" class="sources"><div><h2>Chaque donnée a une origine.</h2><p>Consultez la provenance et la licence avant de réutiliser les données.</p></div><div v-if="report" class="source-list"><article v-for="item in report.provenance" :key="item.label + item.url"><a v-if="safeSourceUrl(item.url)" :href="safeSourceUrl(item.url)" target="_blank" rel="noopener noreferrer">{{ item.label }} <span aria-hidden="true">↗</span></a><strong v-else>{{ item.label }}</strong><span>{{ item.license }}</span><small>Récupérées {{ dateTime(item.retrievedAtUtc) }}</small></article></div><p v-else class="quiet">La provenance accompagnera le premier rapport.</p></section>
    </main><footer><span>HydroDataLab</span><p>Projet pédagogique indépendant. Aucune affiliation officielle à Hydro-Québec.</p><span>Construire avec les agents IA</span></footer>
  </div>
</template>


