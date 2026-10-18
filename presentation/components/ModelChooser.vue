<script setup>
import { computed, ref } from 'vue'
const task = ref('transform')
const chosen = ref('sol')
const attempts = ref(1)
const models = [{id:'luna',name:'GPT-6 Luna',input:.1,output:.5},{id:'haiku',name:'Claude Haiku 5.5',input:.1,output:.5},{id:'sonnet',name:'Claude Sonnet 5.5',input:2,output:10},{id:'sol',name:'GPT-6.1 Sol',input:2,output:10},{id:'opus',name:'Claude Opus 5.5',input:4,output:20}]
const tasks = {explain:{label:'Expliquer 30 lignes',advice:'Auto Efficiency, Luna ou Haiku : commencer petit et vérifier la réponse.'},transform:{label:'Modifier un ETL testé',advice:'Auto Balance, Sol ou Sonnet : suivre la spec, exécuter les tests.'},debug:{label:'Diagnostiquer un échec complexe',advice:'Planifier avec un modèle capable; Opus si la difficulté le justifie.'}}
const selected = computed(()=>models.find(m=>m.id===chosen.value))
const cost = computed(()=>((20000*selected.value.input+2000*selected.value.output)/1e6*attempts.value))
</script>
<template><section class="interactive" aria-label="Comparer tâche modèle et coût illustratif">
<div class="controls"><label>Tâche <select v-model="task"><option v-for="(t,id) in tasks" :key="id" :value="id">{{t.label}}</option></select></label><label>Modèle <select v-model="chosen"><option v-for="m in models" :key="m.id" :value="m.id">{{m.name}}</option></select></label></div>
<div class="detail"><h3>{{tasks[task].advice}}</h3><p>Simulation : <strong>{{cost.toFixed(3)}} $ US</strong> = {{(cost*100).toFixed(1)}} crédits</p><div class="controls"><button @click="attempts=Math.max(1,attempts-1)">−</button><span>{{attempts}} tentative(s)</span><button @click="attempts=Math.min(8,attempts+1)">+</button></div></div>
<p class="context-summary">Hypothèse par tentative : 20 000 tokens d’entrée + 2 000 de sortie, sans cache ni remise Auto. Ni mesure réelle, ni classement de qualité. Tarifs standards du 18 octobre 2026.</p>
</section></template>
