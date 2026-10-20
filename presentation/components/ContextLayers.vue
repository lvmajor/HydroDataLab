<script setup>
import {useSharedSlideNumber} from '../composables/useSharedSlideNumber.js'
const selected=useSharedSlideNumber('context-layer',0,0,4)
const layers=[
 {english:'Always on',french:'Règles communes',example:'copilot-instructions.md',when:'Commun',text:'Préserver les données brutes. Garder les calculs testables hors réseau.',timing:'Les règles essentielles figurent dans les instructions du dépôt; le reste de la documentation se lit au besoin.'},
 {english:'Task scoped',french:'Règles Python',example:'applyTo: "etl/**/*.py"',when:'Chemin',text:'Conserver les valeurs manquantes. Compter zéro comme une mesure valide.',timing:'Le travail porte sur un fichier Python correspondant au motif : la règle ciblée contribue au contexte.'},
 {english:'Task scoped',french:'Rôle et demande',example:'« Ajoute la couverture, puis teste. »',when:'Tâche',text:'Implémenter la couverture décrite dans la spécification. Respecter le périmètre demandé.',timing:'La demande guide ce travail. Un agent personnalisé ajouterait ses consignes s’il était sélectionné.'},
 {english:'On demand',french:'Documents et code',example:'DATA_DICTIONARY.md + coverage.py',when:'Lecture',text:'Une mesure par heure est attendue. Lire le fuseau et les critères avant de calculer.',timing:'Joindre la spécification et faire lire le dictionnaire. Un fichier ouvert ou un lien seul ne prouve pas une lecture complète.'},
 {english:'On demand',french:'Résultats des outils',example:'pytest → 23/24 = 95,83 %',when:'Résultat',text:'Les tests observent le résultat : 23 mesures valides sur 24 donnent 95,83 %.',timing:'La sortie de l’outil alimente la prochaine requête. Elle constitue une observation, pas une nouvelle règle du projet.'},
]
</script>
<template>
 <section class="interactive context-map" aria-label="Composition du contexte pour la tâche de couverture">
  <p class="map-intro">Exemple : « Ajoute le calcul de couverture et vérifie-le. »</p>
  <div class="context-diagram">
   <div class="context-layers">
    <button v-for="(layer,i) in layers" :key="i" :class="['context-layer',{active:selected===i}]" :aria-pressed="selected===i" @click="selected=i">
     <span class="layer-number">{{i+1}}</span>
     <span class="layer-copy"><b>{{layer.english}}</b><span class="translation">{{layer.french}}</span><span class="layer-example">{{layer.example}}</span></span>
    </button>
   </div>
   <svg class="context-connectors" viewBox="0 0 28 315" role="img" aria-label="Les apports sélectionnés alimentent le contexte du modèle">
    <g v-for="(_,i) in layers" :key="i" :class="{selected:selected===i}">
     <path :d="'M0 '+(i*63+30)+' H25'"/><path :d="'m19 '+(i*63+25)+' 6 5 -6 5'"/>
    </g>
   </svg>
   <div class="model-context" aria-live="polite">
    <div class="context-heading"><b>Model context</b><span>Contexte envoyé au modèle</span></div>
    <div class="context-preview" :key="selected"><span class="preview-label">Apport {{selected+1}} · {{layers[selected].when}}</span><p>{{layers[selected].text}}</p><small>{{layers[selected].timing}}</small></div>
   </div>
  </div>
  <p class="map-note">Aussi : système + historique. Vérifier <b>References</b>, lectures et résultats. Contexte limité.</p>
 </section>
</template>
<style scoped>
.context-map .map-intro{font-size:20px;margin:0 0 12px}
.context-diagram{display:grid;grid-template-columns:500px 28px 1fr;align-items:stretch}
.context-layers{display:grid;grid-template-rows:repeat(5,58px);gap:5px}
.context-map button.context-layer{display:grid;grid-template-columns:23px 1fr;align-items:center;gap:9px;width:100%;padding:5px 9px;text-align:left;background:#edf3f5;border:0;border-left:4px solid #9eb9c3;color:#123d51;transition:background .25s}
.context-map button.context-layer.active{background:#dceff0;border-left-color:#006b83;color:#123d51}
.layer-number{font-size:18px;color:#006b83;font-weight:600}
.layer-copy b{font-size:18px}.translation{font-size:17px;margin-left:10px}
.layer-example{display:block;font-size:16px;margin-top:2px}
.context-connectors{height:310px;width:28px}.context-connectors path{fill:none;stroke:#b6cbd3;stroke-width:2}
.context-connectors .selected path{stroke:#006b83;stroke-width:3}
.model-context{border:2px solid #006b83;background:#f0f7f8;min-height:0}
.context-heading{background:#053448;color:white;padding:9px 11px}.context-heading b{display:block;font-size:21px}.context-heading span{font-size:16px}
.context-preview{padding:11px;animation:context-in .3s ease-out}.preview-label{font-size:16px;color:#476271}
.context-preview p{font-size:20px;line-height:1.35;margin:9px 0 12px}.context-preview small{font-size:16px;line-height:1.35;display:block}
.context-map .map-note{font-size:16px;margin:7px 0 0;color:#476271;line-height:1.3}
@keyframes context-in{from{opacity:.35;transform:translateX(-5px)}to{opacity:1;transform:translateX(0)}}
@media(prefers-reduced-motion:reduce){.context-preview{animation:none}.context-map button.context-layer{transition:none}}
@media print{.context-preview{animation:none}}
</style>
