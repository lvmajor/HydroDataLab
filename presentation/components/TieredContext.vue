<template>
  <section class="tiered-context" aria-label="Architecture du contexte par paliers">
    <p class="tier-intro">Architecture du contexte par paliers : <b>quoi charger, et quand?</b></p>
    <div class="tier-grid">
      <article class="tier always">
        <span class="tier-index">01 · Always on</span>
        <h2>Règles communes</h2>
        <ul><li>Dépôt + utilisateur</li><li>Organisation, si activée</li><li>Architecture : résumé et standards essentiels</li></ul>
        <p>Incluses si reconnues par le harnais.</p>
      </article>
      <article class="tier scoped">
        <span class="tier-index">02 · Task scoped</span>
        <h2>Selon la tâche</h2>
        <ul><li>Règles de chemin : <code>applyTo</code></li><li>Rôle et prompt choisis</li><li>Skill chargée</li></ul>
        <p>Chargement conditionnel : fichier, rôle ou travail demandé.</p>
      </article>
      <article class="tier demand">
        <span class="tier-index">03 · On demand</span>
        <h2>Au besoin</h2>
        <ul><li>Documents et code utiles</li><li>Sélection IDE, contexte CLI</li><li>Lectures et sortie terminal</li></ul>
        <p>Joint, récupéré par un outil ou fourni par l’interface.</p>
      </article>
    </div>
    <div class="harness-band"><b>Harnais : outils disponibles · serveurs MCP · permissions</b><br>Ils encadrent les actions. Configurer un serveur ne charge pas ses données.</div>
  </section>
</template>
<style scoped>
.tiered-context{font-size:18px;line-height:1.35}.tiered-context p{margin:0;opacity:1}
.tier-intro{margin:0 0 12px!important;font-size:20px}
.tier-grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:14px}
.tier{padding:13px 14px 12px;background:#edf3f5;border-top:5px solid #006b83}
.scoped{background:#e1f1ee;border-color:#007b68}.demand{background:#fcf3db;border-color:#a66c00}
.tier-index{font-size:17px;font-weight:600}.tier h2{font-family:inherit;font-size:22px;margin:6px 0 10px;line-height:1.2}
.tier ul{padding-left:19px;margin:0;min-height:124px}.tier li{margin:0 0 8px;padding:0;font-size:18px;line-height:1.35}
.tier p{font-size:16px;line-height:1.35;border-top:1px solid #9eb9c3;padding-top:9px}.tier code{font-size:16px}
.harness-band{margin-top:8px;border-left:5px solid #073748;padding:3px 12px;font-size:17px;line-height:1.45;background:#edf3f5}
</style>
