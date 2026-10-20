<script setup>
import {useSharedSlideNumber} from '../composables/useSharedSlideNumber.js'
const selected=useSharedSlideNumber('context-loading',0,0,2)
const layers=[
 {english:'Automatic',french:'Inclus automatiquement',example:'Les instructions communes reconnues',when:'Au départ',text:'« Préserver les données brutes. » La règle du dépôt accompagne la demande de couverture.',timing:'Le harnais doit reconnaître le fichier. La présence de documents dans Git ne charge pas tout leur contenu.'},
 {english:'Conditional',french:'Activé sous condition',example:'applyTo correspondant · skill choisie',when:'Quand c’est pertinent',text:'Modifier coverage.py : règle Python. /validate-etl charge notre procédure de validation.',timing:'Une skill peut être choisie par l’agent ou invoquée. Ici, validate-etl est volontaire : activation automatique désactivée.'},
 {english:'Retrieve',french:'Lire ou recevoir',example:'Dictionnaire consulté · résultat de pytest',when:'Pendant le travail',text:'Le dictionnaire précise les créneaux attendus. Un test vérifie que 23/24 donne 95,83 %.',timing:'Lecture par l’agent, fichier joint ou retour d’outil : le contexte s’enrichit. « Au besoin » ne signifie pas « manuel seulement ».'},
]
</script>
<template>
 <section class="interactive context-map" aria-label="Chargement progressif pour la tâche de couverture">
  <p class="map-intro">Même tâche : ajouter la couverture. <b>Quand et comment arrive le contexte?</b></p>
  <div class="context-diagram">
   <div class="context-layers">
    <button v-for="(layer,i) in layers" :key="i" :class="['context-layer',{active:selected===i}]" :aria-pressed="selected===i" @click="selected=i">
     <span class="layer-number">{{i+1}}</span>
     <span class="layer-copy"><b>{{layer.english}}</b><span class="translation">{{layer.french}}</span><span class="layer-example">{{layer.example}}</span></span>
    </button>
   </div>
   <svg class="context-connectors" viewBox="0 0 28 268" role="img" aria-label="Détail du mode de chargement sélectionné">
    <g v-for="(_,i) in layers" :key="i" :class="{selected:selected===i}">
     <path :d="'M0 '+(i*92+42)+' H25'"/><path :d="'m19 '+(i*92+37)+' 6 5 -6 5'"/>
    </g>
   </svg>
   <div class="model-context" aria-live="polite">
    <div class="context-heading"><b>Zoom sur le chargement</b></div>
    <div class="context-preview" :key="selected"><span class="preview-label">Apport {{selected+1}} · {{layers[selected].when}}</span><p>{{layers[selected].text}}</p><small>{{layers[selected].timing}}</small></div>
   </div>
  </div>
  <div class="skill-loading"><b>Skill · chargement progressif</b><span>Nom + description <b>→</b> <code>SKILL.md</code> <b>→</b> références utiles</span></div>
  <p class="map-note">Les apports se combinent. Vérifier <b>References</b>, lectures et résultats; la portée reste distincte.</p>
 </section>
</template>
<style scoped>
.context-map .map-intro{font-size:19px;margin:0 0 12px}
.context-diagram{display:grid;grid-template-columns:470px 28px 1fr;align-items:stretch}
.context-layers{display:grid;grid-template-rows:repeat(3,84px);gap:8px}
.context-map button.context-layer{display:grid;grid-template-columns:23px 1fr;align-items:center;gap:9px;width:100%;padding:8px 10px;text-align:left;background:#edf3f5;border:0;border-left:4px solid #9eb9c3;color:#123d51;transition:background .25s}
.context-map button.context-layer.active{background:#dceff0;border-left-color:#006b83;color:#123d51}
.layer-number{font-size:20px;color:#006b83;font-weight:600}
.layer-copy b{font-size:19px}.translation{font-size:18px;margin-left:10px}
.layer-example{display:block;font-size:17px;margin-top:6px}
.context-connectors{height:268px;width:28px}.context-connectors path{fill:none;stroke:#b6cbd3;stroke-width:2}
.context-connectors .selected path{stroke:#006b83;stroke-width:3}
.model-context{border:2px solid #006b83;background:#f0f7f8;min-height:0}
.context-heading{background:#053448;color:white;padding:9px 11px}.context-heading b{display:block;font-size:21px}
.context-preview{padding:10px 11px;animation:context-in .3s ease-out}.preview-label{font-size:16px;color:#476271}
.context-preview p{font-size:20px;line-height:1.35;margin:8px 0 10px}.context-preview small{font-size:16px;line-height:1.35;display:block}
.skill-loading{display:flex;justify-content:space-between;align-items:center;gap:10px;background:#e1f1ee;margin-top:8px;padding:7px 12px;font-size:17px;line-height:1.4}.skill-loading span>b{margin:0 5px}.skill-loading code{font-size:17px}
.context-map .map-note{font-size:16px;margin:5px 0 0;color:#476271;line-height:1.3}
@keyframes context-in{from{opacity:.35;transform:translateX(-5px)}to{opacity:1;transform:translateX(0)}}
@media(prefers-reduced-motion:reduce){.context-preview{animation:none}.context-map button.context-layer{transition:none}}
@media print{.context-preview{animation:none}}
</style>
