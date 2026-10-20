<template>
  <div class="thinking-effort">
    <div class="effort-columns">
      <section aria-label="Choisir un effort de raisonnement">
        <h2>Thinking effort · effort de raisonnement</h2>
        <p class="effort-default">Commencer par le <b>réglage recommandé</b><br>et l’adaptation automatique, si disponible.</p>
        <dl class="effort-levels">
          <div><dt>Low · faible</dt><dd>Correction mécanique</dd></div>
          <div><dt>Medium · moyen</dt><dd>Modification délimitée</dd></div>
          <div><dt>High · élevé</dt><dd>Diagnostic complexe</dd></div>
        </dl>
        <p class="effort-caption">Exemples à tester; niveaux selon le modèle.</p>
        <p class="effort-path">Sélecteur → <b>Thinking Effort</b> · distinct d’Auto</p>
      </section>
      <section class="benchmark-reading" aria-label="Lire le benchmark Artificial Analysis">
        <h2>Artificial Analysis · code / coût</h2>
        <svg viewBox="0 0 390 155" role="img" aria-label="Repère de lecture sans données : le score de code augmente vers le haut; le coût API par tâche augmente vers la droite. Le coin supérieur gauche combine meilleur score et coût moindre.">
          <text x="8" y="19" class="axis-label">↑ Score sur les tâches de code</text>
          <path d="M 17 34 V 111 H 375" class="axis" />
          <path d="M 12 41 L 17 34 L 22 41 M 368 106 L 375 111 L 368 116" class="axis" />
          <path d="M 35 43 H 219 V 98 H 35 Z" class="preferred-zone" />
          <text x="49" y="65" class="zone-label">Meilleur score,</text>
          <text x="49" y="88" class="zone-label">coût plus faible ↖</text>
          <text x="132" y="144" class="axis-label">Coût API / tâche ($ US) →</text>
        </svg>
        <p class="effort-caption">Repère de lecture, sans points de benchmark.</p>
        <p class="benchmark-scope"><b>Agent + modèle + réglages</b> testés.<br>Coût API ≠ facture Copilot.</p>
      </section>
    </div>
    <div class="task-cost"><b>Coût du résultat validé</b> = tous les appels, reprises comprises.<br><span>Plus d’effort consomme plus de tokens; vérifier le gain sur vos tâches.</span></div>
  </div>
</template>

<style scoped>
.thinking-effort { font-size:18px; line-height:1.35; }
.effort-columns { display:grid; grid-template-columns:1.07fr 1fr; gap:30px; }
.thinking-effort h2 { font-family:'IBM Plex Sans',sans-serif; font-size:19px; line-height:1.3; margin:0 0 8px; font-weight:600; }
.thinking-effort p { margin:0; opacity:1; }
.effort-default { background:#e1f1ee; border-left:4px solid #007967; padding:6px 10px; }
.effort-levels { margin:8px 0; }
.effort-levels > div { display:grid; grid-template-columns:150px 1fr; gap:8px; padding:5px 0; border-bottom:1px solid #cadce3; }
.effort-levels dt { font-weight:600; color:var(--water); }
.effort-levels dd { margin:0; }
.thinking-effort .effort-caption { font-size:16px; color:var(--muted); line-height:1.4; }
.thinking-effort .effort-path { margin-top:12px; font-size:17px; line-height:1.4; }
.benchmark-reading { border-left:1px solid #b9d5df; padding-left:24px; }
.benchmark-reading svg { display:block; width:100%; margin:4px 0 7px; overflow:visible; }
.axis { fill:none; stroke:#476271; stroke-width:2; }
.axis-label { fill:#123d51; font-size:17px; }
.preferred-zone { fill:#e1f1ee; }
.zone-label { fill:#006b61; font-size:20px; font-weight:600; }
.thinking-effort .benchmark-scope { font-size:18px; margin-top:15px; line-height:1.4; }
.task-cost { border-left:5px solid var(--gold); padding-left:16px; margin-top:14px; font-size:21px; }
.task-cost span { font-size:18px; }
</style>
