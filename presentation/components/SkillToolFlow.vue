<template>
  <div class="skill-tool-flow">
    <section class="skill-lane" aria-label="Chargement progressif d’une skill">
      <header><h2>Skill · charger une méthode</h2><span>Choix automatique possible, ou <code>/nom</code>*</span></header>
      <div class="activation-steps">
        <div><b>1 · Repérer</b><span>Nom + description<br>de la skill</span></div><span class="activation-arrow" aria-hidden="true">→</span>
        <div><b>2 · Choisir et charger</b><span>Corps du <code>SKILL.md</code><br>dans le contexte</span></div><span class="activation-arrow" aria-hidden="true">→</span>
        <div><b>3 · Consulter au besoin</b><span>Fichiers de référence<br>et scripts associés</span></div>
      </div>
    </section>
    <p class="activation-bridge">La méthode peut guider les outils. Un outil peut aussi servir sans skill.</p>
    <section class="tool-lane" aria-label="Choix et exécution d’un outil">
      <header><h2>Tool · exécuter une action</h2><span>Selon la demande, le contexte et les outils disponibles</span></header>
      <div class="activation-steps">
        <div><b>1 · Le modèle choisit</b><span>Outil + paramètres<br>ex. terminal → pytest</span></div><span class="activation-arrow" aria-hidden="true">→</span>
        <div><b>2 · Le harnais contrôle</b><span>Permissions, puis<br>exécution autorisée</span></div><span class="activation-arrow" aria-hidden="true">→</span>
        <div><b>3 · Le résultat revient</b><span>Le modèle décide<br>de la suite</span></div>
      </div>
    </section>
    <div class="activation-example"><b>* Notre skill <code>/validate-etl</code> est volontaire :</b> <code>disable-model-invocation: true</code>.<br>Charger ses consignes ne lance pas ses scripts; leur exécution passe par des outils.</div>
  </div>
</template>

<style>
.skill-tool-flow header{display:flex;align-items:baseline;justify-content:space-between;gap:12px;margin-bottom:10px}
.slidev-layout .skill-tool-flow h2{font:600 22px/1.2 'IBM Plex Sans',sans-serif;color:var(--deep);margin:0}
.skill-tool-flow header>span{font-size:16px;line-height:1.3}
.activation-steps{display:grid;grid-template-columns:1fr 30px 1fr 30px 1fr;align-items:stretch}
.activation-steps>div{background:#e7f2f4;border-top:4px solid var(--water);padding:8px 13px}
.tool-lane .activation-steps>div{background:#edf2f7;border-color:#497c99}
.activation-steps b{display:block;font-size:20px;line-height:1.3;margin-bottom:6px}
.activation-steps div>span{display:block;font-size:18px;line-height:1.4}
.activation-arrow{align-self:center;text-align:center;font-size:25px;color:var(--water)}
.slidev-layout .activation-bridge{font-size:17px;line-height:1.4;margin:10px 0;color:var(--muted)}
.activation-example{font-size:17px;line-height:1.5;margin-top:12px;padding-left:12px;border-left:4px solid var(--gold)}
.skill-tool-flow code{font-size:.9em}
</style>
