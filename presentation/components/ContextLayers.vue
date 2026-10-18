<script setup>
import {ref} from 'vue'
const selected=ref(0)
const layers=[
 {english:'Project instructions',french:'Consignes du projet',example:'.github/copilot-instructions.md',when:'Règles communes',text:'Préserver les données brutes et leur provenance. Indiquer où se trouve le code et comment le valider.',timing:'Consignes durables du projet, découvertes par le harnais.'},
 {english:'File-specific instructions',french:'Consignes ciblées',example:'.github/instructions/python.instructions.md',when:'Selon le fichier',text:'Pour le code Python : conserver les valeurs manquantes et compter zéro comme une mesure valide.',timing:'Chargement selon le fichier, la description ou un ajout manuel.'},
 {english:'Reference documents',french:'Documents de référence',example:'docs/DATA_DICTIONARY.md',when:'À joindre ou lire',text:'Une série horaire attend une mesure par heure. Son unité et son fuseau définissent le sens du calcul.',timing:'Le document doit être joint à la demande ou effectivement lu.'},
 {english:'Task prompt',french:'Demande courante',example:'« Ajoute la couverture décrite dans specs/coverage.md. »',when:'Cette demande',text:'Modifier le calcul Python de couverture. Respecter les critères de la spécification et le périmètre demandé.',timing:'La requête précise le résultat attendu maintenant.'},
 {english:'Tool results',french:'Résultats des outils',example:'Sortie pytest + report.json',when:'Pendant le travail',text:'Le test confirme : 23 mesures valides sur 24 donnent 95,83 %. Le rapport permet de vérifier le résultat.',timing:'Les outils renvoient ce qu’ils ont réellement observé.'},
]
</script>
<template>
 <section class="interactive context-map" aria-label="Couches de contexte bilingues">
  <p class="map-intro">Des consignes durables → une demande précise → des résultats observés.</p>
  <div class="context-diagram">
   <div class="context-layers">
    <button v-for="(layer,i) in layers" :key="layer.english" :class="['context-layer',{active:selected===i}]" :aria-pressed="selected===i" @click="selected=i">
     <span class="layer-number">{{i+1}}</span>
     <span class="layer-copy"><b>{{layer.english}}</b><span class="translation">{{layer.french}}</span><code>{{layer.example}}</code></span>
     <span class="activation">{{layer.when}}</span>
    </button>
   </div>
   <svg class="context-connectors" viewBox="0 0 28 315" role="img" aria-label="Les cinq couches alimentent le contexte du modèle">
    <g v-for="(_,i) in layers" :key="i" :class="{selected:selected===i}">
     <path :d="'M0 '+(i*63+30)+' H25'"/><path :d="'m19 '+(i*63+25)+' 6 5 -6 5'"/>
    </g>
   </svg>
   <div class="model-context" aria-live="polite">
    <div class="context-heading"><b>Model context</b><span>Contexte du modèle</span></div>
    <div class="context-preview" :key="selected"><span class="preview-label">Apport de la couche {{selected+1}}</span><p>{{layers[selected].text}}</p><small>{{layers[selected].timing}}</small></div>
   </div>
  </div>
  <p class="map-note">Couches complémentaires, sans priorité imposée. Organisation pédagogique du cours, non officielle.</p>
 </section>
</template>
<style scoped>
.context-map .map-intro{font-size:19px;margin:0 0 13px}
.context-diagram{display:grid;grid-template-columns:570px 28px 1fr;align-items:stretch}
.context-layers{display:grid;grid-template-rows:repeat(5,58px);gap:5px}
.context-map button.context-layer{display:grid;grid-template-columns:23px 1fr 96px;align-items:center;gap:9px;width:100%;padding:5px 9px;text-align:left;background:#edf3f5;border:0;border-left:4px solid #9eb9c3;color:#123d51;transition:background .25s}
.context-map button.context-layer.active{background:#dceff0;border-left-color:#006b83;color:#123d51}
.layer-number{font-size:18px;color:#006b83;font-weight:600}
.layer-copy b{font-size:17px}.translation{font-size:16px;margin-left:8px}
.layer-copy code{display:block;font-size:11px;margin-top:4px;white-space:nowrap}
.activation{font-size:13px;line-height:1.2;color:#476271;border-left:1px solid #9eb9c3;padding-left:8px}
.context-connectors{height:310px;width:28px}
.context-connectors path{fill:none;stroke:#b6cbd3;stroke-width:2}
.context-connectors .selected path{stroke:#006b83;stroke-width:3}
.model-context{border:2px solid #006b83;background:#f0f7f8;min-height:0}
.context-heading{background:#053448;color:white;padding:9px 11px}.context-heading b{display:block;font-size:21px}.context-heading span{font-size:16px}
.context-preview{padding:11px;animation:context-in .3s ease-out}.preview-label{font-size:13px;color:#476271}
.context-preview p{font-size:18px;line-height:1.3;margin:9px 0 12px}.context-preview small{font-size:13px;line-height:1.35;display:block}
.context-map .map-note{font-size:14px;margin:13px 0 0;color:#476271;line-height:1.4}
@keyframes context-in{from{opacity:.35;transform:translateX(-5px)}to{opacity:1;transform:translateX(0)}}
@media print{.context-preview{animation:none}}
</style>
