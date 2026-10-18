<script setup>
import {ref} from 'vue'
const language=ref('python')
const selected=ref(2)
const layers=[
 {name:'Socle',title:'Les décisions communes',text:'But du pipeline, carte du dépôt et commandes essentielles. Un socle court, relu et partagé.'},
 {name:'Composant',title:'Les règles de la partie modifiée',text:'Python : conserver les valeurs absentes; tester les calculs sans réseau. Vue : afficher unités et provenance.'},
 {name:'Contrat',title:'Ce que les colonnes veulent dire',text:'Dictionnaire : unité, fréquence, fuseau, provenance. MW ≠ MWh; un niveau en mètres ≠ une réserve en GWh.'},
 {name:'Tâche',title:'La modification demandée maintenant',text:'Joindre specs/coverage.md, la fonction à compléter et quelques données : zéro, absence, doublon.'},
 {name:'Preuves',title:'Le résultat observé',text:'Tests 24/24 et 23/24, journées de 23 et 25 heures, puis rapport JSON. Demander seulement la sortie utile.'},
]
</script>
<template>
 <section class="interactive composition" aria-label="Composition des instructions et du contexte">
  <div class="controls"><span>Fichier modifié :</span><button :class="{active:language==='python'}" @click="language='python'" :aria-pressed="language==='python'">etl/transform.py</button><button :class="{active:language==='vue'}" @click="language='vue'" :aria-pressed="language==='vue'">web/App.vue</button></div>
  <div class="instruction-lanes">
   <div><b>Projet · commun</b><code>.github/copilot-instructions.md</code><span>Toutes les tâches du projet</span></div>
   <div :class="{matched:language==='python'}"><b>Python · ciblé</b><code>applyTo: "etl/**/*.py"</code><span>{{language==='python'?'✓ Motif correspondant':'— Motif non correspondant'}}</span></div>
   <div :class="{matched:language==='vue'}"><b>Vue · ciblé</b><code>applyTo: "web/**/*.vue"</code><span>{{language==='vue'?'✓ Motif correspondant':'— Motif non correspondant'}}</span></div>
  </div>
  <svg :key="language" class="composition-arrows" viewBox="0 0 868 40" role="img" aria-label="Les règles communes et les règles ciblées applicables se combinent">
   <path d="M140 2 V14 Q140 22 150 22 H424 Q434 22 434 32"/>
   <path v-if="language==='python'" d="M434 2 V32"/>
   <path v-else d="M728 2 V14 Q728 22 718 22 H444 Q434 22 434 32"/>
   <path class="arrow-head" d="m427 29 7 7 7-7"/>
  </svg>
  <div class="composition-result">Les consignes applicables s’ajoutent au contexte de la tâche.</div>
  <div class="controls layer-tabs"><button v-for="(layer,i) in layers" :key="layer.name" :class="{active:selected===i}" :aria-pressed="selected===i" @click="selected=i">{{i+1}}. {{layer.name}}</button></div>
  <div class="layer-detail" aria-live="polite"><b>{{layers[selected].title}}</b><p>{{layers[selected].text}}</p></div>
  <p class="composition-note">Schéma du ciblage par fichier. Une description pertinente ou un ajout manuel peut aussi charger une règle.<br>Portée ≠ priorité : éviter les contradictions; un lien vers un document ne garantit pas sa lecture.</p>
 </section>
</template>
<style scoped>
.composition .controls{align-items:center;margin-bottom:8px;font-size:16px}
.instruction-lanes{display:grid;grid-template-columns:repeat(3,1fr);gap:12px}
.instruction-lanes>div{background:#edf3f5;border-top:4px solid #a7bdc7;padding:6px 12px;font-size:17px;transition:background .35s,border-color .35s}
.instruction-lanes>div:first-child,.instruction-lanes>div.matched{background:#def0f1;border-top-color:#006b83}
.instruction-lanes code{display:block;font-size:13px;margin:5px 0;white-space:nowrap}
.instruction-lanes span{font-size:15px}.composition-arrows{height:24px;width:100%;display:block}
.composition-arrows path{fill:none;stroke:#006b83;stroke-width:3;stroke-dasharray:400;animation:connect .7s ease-out both}
.composition-arrows .arrow-head{animation:none;stroke-dasharray:none}
.composition-result{text-align:center;background:#053448;color:white;padding:8px;font-size:19px}
.composition .layer-tabs{margin:16px 0 9px;justify-content:space-between}.layer-tabs button{padding:6px 10px}
.layer-detail{border-left:4px solid #e8b540;padding-left:16px;font-size:18px;min-height:64px}.layer-detail p{font-size:17px;margin:5px 0}
.composition .composition-note{font-size:13px;margin:9px 0 0;color:#476271;line-height:1.4}
@keyframes connect{from{stroke-dashoffset:400}to{stroke-dashoffset:0}}
@media print{.composition-arrows path{animation:none}}
</style>
