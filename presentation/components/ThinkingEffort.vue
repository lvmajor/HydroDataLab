<script setup lang="ts">
const groups = [
  { label: 'GPT‑6.1 Sol · Codex', rows: [['Low', '57', '0,50'], ['High', '60', '0,89'], ['Max', '60', '1,55']] },
  { label: 'Sonnet 5.5 · Claude Code', rows: [['Low', '42', '0,48'], ['High', '55', '1,24'], ['Max', '68', '14,19']] },
]
</script>

<template>
  <div class="thinking-effort">
    <p class="effort-intro"><b>Effort de raisonnement :</b> partir du réglage recommandé, puis mesurer le gain.</p>
    <div class="effort-columns">
      <figure class="benchmark-capture">
        <img src="/aa-cost-effort-20261019.png" alt="Capture réelle d’Artificial Analysis : six configurations de Sol et Sonnet, aux niveaux Low, High et Max. Indice vertical, coût API par tâche sur un axe horizontal logarithmique. Valeurs reprises dans le tableau voisin." />
        <figcaption>19 octobre 2026 · indice v1.5 · coût : axe <b>logarithmique</b>.</figcaption>
      </figure>
      <table class="effort-results" aria-label="Six résultats relevés dans les infobulles d’Artificial Analysis">
        <thead><tr><th scope="col">Effort</th><th scope="col">Indice</th><th scope="col">$ US / tâche</th></tr></thead>
        <tbody v-for="group in groups" :key="group.label">
          <tr class="group-title"><th colspan="3" scope="rowgroup">{{ group.label }}</th></tr>
          <tr v-for="row in group.rows" :key="row[0]"><th scope="row">{{ row[0] }}</th><td>{{ row[1] }}</td><td>{{ row[2] }}</td></tr>
        </tbody>
      </table>
    </div>
    <div class="effort-findings">
      <p><b>Sol High → Max :</b> coût +74 %; même indice affiché de 60.</p>
    </div>
    <p class="effort-limit">Configurations Codex / Claude Code mesurées; coût API ≠ facture Copilot.</p>
  </div>
</template>

<style scoped>
.thinking-effort { font-size:18px; line-height:1.35; }
.thinking-effort p { margin:0; opacity:1; }
.effort-intro { margin-bottom:8px!important; }
.effort-columns { display:grid; grid-template-columns:minmax(0,1fr) 285px; align-items:start; gap:20px; }
.benchmark-capture { margin:0; }
.benchmark-capture img { display:block; width:100%; height:auto; border:1px solid #cadce3; }
.benchmark-capture figcaption { margin-top:4px; font-size:16px; line-height:1.4; color:var(--muted); }
.thinking-effort .effort-results { margin:0; font-size:17px; line-height:1.25; font-variant-numeric:tabular-nums; }
.effort-results th,.effort-results td { padding:3px 7px; text-align:right; border-bottom:1px solid #cadce3; }
.effort-results thead th { font-size:16px; background:var(--ink); color:white; }
.effort-results th:first-child { text-align:left; }
.effort-results tbody th { background:transparent; color:var(--ink); }
.effort-results .group-title th { padding:6px 7px 4px; font-size:18px; text-align:left; background:#e1f1ee; color:#005d55; }
.effort-results tbody tr:nth-child(even) { background:transparent; }
.effort-findings { border-left:5px solid var(--gold); padding-left:14px; margin-top:8px; font-size:19px; line-height:1.4; }
.thinking-effort .effort-limit { font-size:16px; color:var(--muted); line-height:1.4; margin-top:5px; }
</style>
