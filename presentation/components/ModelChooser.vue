<script setup>
import { computed, ref } from 'vue'
const task = ref('transform')
const attempts = ref(1)
const models = [
 {id:'luna',name:'GPT-6 Luna',input:.1,output:.5},
 {id:'haiku',name:'Claude Haiku 5.5',input:.1,output:.5},
 {id:'sonnet',name:'Claude Sonnet 5.5',input:2,output:10},
 {id:'sol',name:'GPT-6.1 Sol',input:2,output:10},
 {id:'opus',name:'Claude Opus 5.5',input:4,output:20},
]
const tasks = {
 explain:{label:'Petite : expliquer une fonction',input:3000,output:500,picks:['luna','haiku'],advice:'Luna / Haiku : une explication courte, à vérifier sur le code.'},
 transform:{label:'Délimitée : modifier un ETL testé',input:20000,output:2000,picks:['sonnet'],advice:'Sonnet : une fonctionnalité délimitée avec critères et tests.'},
 complex:{label:'Complexe : changer plusieurs composants',input:60000,output:8000,picks:['sol'],advice:'Sol : des changements liés entre plusieurs fichiers.'},
 debug:{label:'Difficile : diagnostiquer une anomalie',input:60000,output:8000,picks:['opus'],advice:'Opus : à essayer si le diagnostic reste difficile.'},
}
const current = computed(()=>tasks[task.value])
const cost = m => (current.value.input*m.input+current.value.output*m.output)/1e6*attempts.value
const ceiling = .4
const number = n => n.toLocaleString('fr-CA')
const dollars = n => n.toLocaleString('fr-CA',{minimumFractionDigits:4,maximumFractionDigits:5})+' $'
</script>
<template>
 <section class="interactive cost-chart" aria-label="Coût illustratif par tâche et modèle">
  <div class="controls">
   <label>Tâche <select v-model="task" aria-label="Type de tâche"><option v-for="(t,id) in tasks" :key="id" :value="id">{{t.label}}</option></select></label>
   <button aria-label="Réduire les tentatives" @click="attempts=Math.max(1,attempts-1)">−</button><span>{{attempts}} essai(s)</span><button aria-label="Ajouter une tentative" @click="attempts=Math.min(8,attempts+1)">+</button>
  </div>
  <div class="chart-advice" aria-live="polite">{{current.advice}} <span>◆ Choix à essayer</span></div>
  <div class="cost-row" v-for="m in models" :key="m.id" :data-model="m.id">
   <span class="model-label">{{m.name}} <b v-if="current.picks.includes(m.id)">◆</b></span>
   <div class="bar-track"><div class="bar" :class="{recommended:current.picks.includes(m.id)}" :style="{width:(cost(m)/(ceiling*attempts)*100)+'%'}"></div></div>
   <strong class="cost-value">{{dollars(cost(m))}}</strong>
  </div>
  <div class="axis"><span>0</span><span>{{dollars(ceiling*attempts/2)}}</span><span>{{dollars(ceiling*attempts)}} US</span></div>
  <p class="chart-assumption">Volumes fictifs par essai : <b>{{number(current.input)}} entrée + {{number(current.output)}} sortie</b>.<br>Mêmes volumes pour chaque modèle; sans cache ni remise. Ce n’est pas un benchmark.</p>
  <p class="chart-summary">Au quotidien : <b>Auto Balance</b>. Comparer le coût total d’un résultat validé.</p>
 </section>
</template>
<style scoped>
.cost-chart .controls{align-items:center;margin-bottom:10px;flex-wrap:nowrap;font-size:16px;gap:8px}
.cost-chart select{max-width:430px;padding:6px}
.chart-advice{font-size:19px;margin:8px 0 12px;min-height:27px}.chart-advice span{float:right;font-size:14px;color:#006b83}
.cost-row{display:grid;grid-template-columns:220px 1fr 115px;gap:14px;align-items:center;margin:10px 0;font-size:19px}
.model-label b{color:#006b83}.bar-track{height:21px;background:#e1ebef;border-left:1px solid #476271}
.bar{height:100%;background:#8ca8b5;transition:width .6s ease,background .3s}.bar.recommended{background:#006b83}
.cost-value{text-align:right;font-variant-numeric:tabular-nums;font-size:18px}
.axis{margin:0 129px 0 234px;display:flex;justify-content:space-between;font-size:13px;border-top:1px solid #9bb5c0;padding-top:3px}
.cost-chart .chart-assumption{font-size:16px;line-height:1.4;margin:13px 0 8px}.cost-chart .chart-summary{font-size:19px;margin:8px 0}
</style>
