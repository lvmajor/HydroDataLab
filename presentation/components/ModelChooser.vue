<script setup>
import {ref} from 'vue'
const attempts=ref(1)
const models=[
 {id:'luna',name:'GPT-6 Luna',input:.1,output:.5},
 {id:'haiku',name:'Claude Haiku 5.5',input:.1,output:.5},
 {id:'sonnet',name:'Claude Sonnet 5.5',input:2,output:10},
 {id:'sol',name:'GPT-6.1 Sol',input:2,output:10},
 {id:'opus',name:'Claude Opus 5.5',input:4,output:20},
]
const tasks=[
 {id:'explain',label:'Simple',example:'Expliquer du code',input:3000,output:500,picks:['luna','haiku']},
 {id:'transform',label:'Délimitée',example:'Modifier un ETL',input:20000,output:2000,picks:['sonnet']},
 {id:'complex',label:'Complexe',example:'Plusieurs fichiers',input:60000,output:8000,picks:['sol']},
 {id:'debug',label:'Diagnostic difficile',example:'Trouver une cause',input:60000,output:8000,picks:['opus']},
]
const cost=(m,t)=>(t.input*m.input+t.output*m.output)/1e6*attempts.value
const dollars=n=>n.toLocaleString('fr-CA',{minimumFractionDigits:3,maximumFractionDigits:5})
const number=n=>n.toLocaleString('fr-CA')
</script>
<template>
 <section class="interactive cost-matrix" aria-label="Comparatif de quatre tâches et cinq modèles">
  <div class="matrix-controls"><span><b>Auto Balance</b> au quotidien · choix manuels ci-dessous</span><div><button aria-label="Réduire les tentatives" @click="attempts=Math.max(1,attempts-1)">−</button><span>{{attempts}} essai(s)</span><button aria-label="Ajouter une tentative" @click="attempts=Math.min(8,attempts+1)">+</button></div></div>
  <div class="matrix-grid" role="table" aria-label="Coût simulé en dollars US par modèle et type de tâche">
   <div class="matrix-row header-row" role="row">
    <div class="model-header" role="columnheader">Modèle<br><small>Tokens entrée / sortie →</small></div>
    <div class="task-header" v-for="t in tasks" :key="t.id" role="columnheader"><b>{{t.label}}</b><span>{{t.example}}</span><small>{{number(t.input)}} / {{number(t.output)}}</small></div>
   </div>
   <div class="matrix-row data-row" v-for="m in models" :key="m.id" :data-model="m.id" role="row">
    <b class="model-name" role="rowheader">{{m.name}}</b>
    <div v-for="t in tasks" :key="t.id" :data-task="t.id" :class="['cost-cell',{recommended:t.picks.includes(m.id)}]" role="cell">
     <div class="cell-value"><span class="choice" v-if="t.picks.includes(m.id)" aria-label="Modèle conseillé pour cette tâche">◆</span><span class="cost-value">{{dollars(cost(m,t))}}</span></div>
     <div class="bar-track"><div class="bar" :style="{width:(cost(m,t)/(.4*attempts)*100)+'%'}"></div></div>
    </div>
   </div>
  </div>
  <div class="matrix-legend"><span>◆ Choix conseillé à évaluer</span><span>Échelle commune des barres : 0 → {{dollars(.4*attempts)}} $ US</span></div>
  <p class="matrix-assumptions"><b>Coûts simulés en $ US.</b> Volumes fictifs par essai, identiques entre modèles pour chaque tâche.<br>Sans cache ni remise Auto. Comparer le coût total d’un résultat validé; ce n’est pas un benchmark.</p>
 </section>
</template>
<style scoped>
.matrix-controls{display:flex;justify-content:space-between;align-items:center;font-size:17px;margin:0 0 10px}
.matrix-controls>div{display:flex;gap:8px;align-items:center}.matrix-controls button{padding:2px 9px;font-size:17px}
.matrix-grid{width:100%}.matrix-row{display:grid;grid-template-columns:180px repeat(4,minmax(0,1fr));gap:10px}
.header-row{align-items:stretch;margin-bottom:7px}.model-header{font-size:18px;padding-top:5px}.model-header small{font-size:12px;display:block;margin-top:10px;color:#476271}
.task-header{display:grid;grid-template-rows:23px 22px 18px;align-content:start;height:76px;border-bottom:3px solid #a9c1cb;padding:4px 6px;background:#edf3f5;color:#123d51}
.header-row b{font-size:17px;display:block}.header-row span{font-size:14px;display:block;margin:3px 0}.header-row small{font-size:12px;display:block}
.data-row{align-items:center;border-bottom:1px solid #d7e4e9;min-height:37px}
.model-name{font-size:17px;line-height:1.2}.cost-cell{padding:4px 6px;background:#f4f8fa;min-height:35px}
.cost-cell.recommended{background:#def0eb}.cell-value{display:flex;justify-content:flex-end;gap:7px;align-items:center;font-size:18px;line-height:1.1;font-variant-numeric:tabular-nums}
.choice{font-size:13px;color:#00634f}.bar-track{height:6px;background:#d7e4e9;margin-top:4px}
.bar{height:100%;background:#7b9baa;transition:width .4s}.recommended .bar{background:#00715b}
.matrix-legend{display:flex;justify-content:space-between;font-size:13px;color:#476271;margin:7px 0}
.cost-matrix .matrix-assumptions{font-size:14px;margin:8px 0 0;line-height:1.4}
</style>
