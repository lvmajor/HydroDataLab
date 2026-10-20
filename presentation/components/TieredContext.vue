<template>
  <section class="tiered-context" aria-label="Portée du contexte : projet, fichiers et tâche">
    <p class="tier-intro"><b>Tiered-context architecture</b> · Premier axe : à quel travail cela s’applique?</p>
    <div class="tier-grid">
      <article class="tier project">
        <span class="tier-index">Project-wide · Tout le projet</span>
        <h2>Règles communes</h2>
        <p class="tier-example">« Préserver les données brutes et leur provenance. »</p>
        <p class="tier-file"><code>.github/<br>copilot-instructions.md</code></p>
      </article>
      <article class="tier files">
        <span class="tier-index">File-specific · Fichiers ciblés</span>
        <h2>Règles Python</h2>
        <p class="tier-example">« Une valeur absente ne devient pas zéro. »</p>
        <p class="tier-file"><code>python.instructions.md<br>applyTo: "etl/**/*.py"</code></p>
      </article>
      <article class="tier task">
        <span class="tier-index">Task scoped · Tâche ciblée</span>
        <h2>But et méthode</h2>
        <p class="tier-example">« Ajouter la couverture, puis vérifier le calcul. »</p>
        <p class="tier-file">Demande + résultat attendu<br>Skill de validation, si utilisée</p>
      </article>
    </div>
    <div class="scope-loading"><b>Une skill peut être les deux :</b><br><span>Task scoped = portée ciblée</span><b class="plus">+</b><span>On demand = chargée au besoin</span></div>
  </section>
</template>
<style scoped>
.tiered-context{font-size:18px;line-height:1.35}.tiered-context p{margin:0;opacity:1}
.tier-intro{margin:0 0 12px!important;font-size:19px}
.tier-grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:14px}
.tier{padding:13px 14px;background:#edf3f5;border-top:5px solid #006b83}
.files{background:#e1f1ee;border-color:#007b68}.task{background:#fcf3db;border-color:#a66c00}
.tier-index{font-size:16px;font-weight:600}.tier h2{font-family:inherit;font-size:22px;margin:8px 0 12px;line-height:1.2}
.tier .tier-example{font-size:20px;line-height:1.35;min-height:81px}
.tier .tier-file{font-size:16px;line-height:1.45;min-height:60px;margin-top:10px}.tier code{font-size:16px}
.scope-loading{margin-top:12px;border-left:5px solid #073748;padding:8px 12px;font-size:18px;line-height:1.5;background:#edf3f5}
.scope-loading .plus{display:inline-block;margin:0 15px;color:#006b83}
</style>
