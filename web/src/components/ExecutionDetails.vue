<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { api } from '../api'
import type { Run, RunDetails } from '../types'
import { number } from '../format'
const props = defineProps<{ runs: Run[]; selected: string }>()
const emit = defineEmits<{ select: [id: string] }>()
const details = ref<RunDetails | null>(null), error = ref(''), loading = ref(false)
const current = computed(() => props.runs.find(r => r.id === props.selected))
const labels: Record<string, string> = { queued: 'En attente', running: 'En cours', succeeded: 'Terminé', failed: 'Échec', interrupted: 'Interrompu' }
const duration = computed(() => { const run = details.value?.run; return run?.startedAtUtc && run.completedAtUtc ? (Date.parse(run.completedAtUtc) - Date.parse(run.startedAtUtc)) / 1000 : null })
const stamp = (value: string | null) => value ? new Intl.DateTimeFormat('fr-CA', { timeZone: 'America/Toronto', dateStyle: 'short', timeStyle: 'medium' }).format(new Date(value)) : '—'
watch(() => [props.selected, current.value?.status], async (_, __, cleanup) => {
  let stale = false; cleanup(() => { stale = true })
  details.value = null; error.value = ''; loading.value = !!props.selected
  if (!props.selected) return
  try { const data = await api<RunDetails>(`/runs/${encodeURIComponent(props.selected)}/details`); if (!stale) details.value = data }
  catch { if (!stale) error.value = 'Impossible de charger les détails de cette exécution.' }
  finally { if (!stale) loading.value = false }
}, { immediate: true })
</script>
<template>
  <section id="execution" class="etl-section execution-section" aria-labelledby="execution-title">
    <h2 id="execution-title">Détail d’une exécution</h2><p class="section-intro">Quartz planifie le travail. Le moteur ETL mesure ses étapes; SQLite conserve ce bilan après la fin du traitement.</p>
    <label class="field-label" for="selected-run">Exécution à consulter (100 dernières)</label><select id="selected-run" :value="selected" @change="emit('select', ($event.target as HTMLSelectElement).value)"><option value="">Choisir une exécution</option><option v-for="run in runs" :value="run.id" :key="run.id">{{ stamp(run.createdAtUtc) }} · {{ labels[run.status] }} · {{ run.id.slice(0, 8) }}</option></select>
    <p v-if="loading" role="status" class="quiet">Chargement des détails…</p><p v-if="error" role="alert" class="inline-error">{{ error }}</p>
    <template v-if="details">
      <dl class="execution-facts"><div><dt>Identifiant de corrélation</dt><dd>{{ details.run.id }}</dd></div><div><dt>État</dt><dd>{{ labels[details.run.status] }}</dd></div><div><dt>Source / scénario</dt><dd>{{ details.run.source === 'fixture' ? 'Jeu enregistré' : 'Sources publiques' }} / {{ details.run.qualityScenario === 'missing' ? 'Une mesure retirée' : 'Complet' }}</dd></div><div><dt>Échec simulé demandé</dt><dd>{{ details.simulateFailure == null ? 'Non enregistré (ancien traitement)' : details.simulateFailure ? 'Oui' : 'Non' }}</dd></div><div><dt>Création · Toronto</dt><dd>{{ stamp(details.run.createdAtUtc) }}</dd></div><div><dt>Début → fin · Toronto</dt><dd>{{ stamp(details.run.startedAtUtc) }} → {{ stamp(details.run.completedAtUtc) }}</dd></div><div><dt>Durée du travail API + ETL</dt><dd>{{ duration == null ? 'Non disponible' : `${number(duration, 3)} s` }}</dd></div></dl>
      <p v-if="details.retryOf">Nouvelle tentative de <button class="text-button break-id" @click="emit('select', details!.retryOf!)">{{ details.retryOf }}</button></p>
      <p v-if="details.run.error" class="notice error">{{ details.run.error }}</p>
      <p v-if="['queued', 'running'].includes(details.run.status)" class="quiet">Traitement en cours. Le bilan des étapes sera disponible à la fin; ce tableau n’est pas un suivi en temps réel des étapes ETL.</p>
      <div v-if="details.execution?.steps.length" class="table-scroll" role="region" aria-label="Étapes mesurées du traitement" tabindex="0"><table><thead><tr><th>Étape ETL</th><th>État</th><th>Durée mesurée</th></tr></thead><tbody><tr v-for="item in details.execution.steps" :key="item.id"><td>{{ item.label }}<p v-if="item.error" class="run-error">{{ item.error }}</p></td><td>{{ labels[item.status] ?? item.status }}</td><td>{{ number(item.durationMs, 3) }} ms</td></tr></tbody></table></div>
      <p v-else-if="!['queued', 'running'].includes(details.run.status)" class="quiet">Aucun bilan ETL reçu pour cette exécution : ancien traitement, interruption ou erreur de communication.</p>
      <p v-if="details.execution" class="quiet">Les étapes suivantes ne sont pas exécutées après un échec. Les durées ETL excluent le transport HTTP, la publication finale du rapport et son enregistrement SQLite.</p>
      <template v-if="details.report"><p class="section-intro">{{ details.report.summary.observationCount }} observations valides · {{ details.report.analysis?.completeHours ?? '—' }} heures complètes. Rapport propre à cette exécution.</p><ul v-if="details.report.warnings.length" class="warnings"><li v-for="warning in details.report.warnings" :key="warning">{{ warning }}</li></ul><div class="export-links"><a v-for="(file, kind) in details.report.artifacts" :key="kind" :href="`/api/artifacts/${encodeURIComponent(details.run.id)}/${encodeURIComponent(file!)}`" download>{{ kind === 'hourly' ? 'Synthèse horaire CSV' : kind === 'report' ? 'Rapport JSON' : kind.toUpperCase() }} ↓</a></div></template>
    </template>
  </section>
</template>
