<script setup>
import { ref } from 'vue'
const selected = ref(0)
const layers = [
 { name:'Socle', title:'Ce qui reste vrai pour tout le dépôt', text:'Python transforme. L’API orchestre. Vue présente. Les données brutes sont conservées.', file:'.github/copilot-instructions.md' },
 { name:'Composant', title:'Ce qui s’applique à la tâche Python', text:'Horodatages UTC, valeurs nulles explicites et tests du domaine. Une observation à zéro reste valide.', file:'.github/instructions/python.instructions.md' },
 { name:'Contrat', title:'Ce que les colonnes veulent dire', text:'Schéma, unité, fréquence, fuseau, provenance, qualité. MW ≠ MWh; un niveau en mètres ≠ une réserve en GWh.', file:'docs/CONTRACT.md + dictionnaire de données' },
 { name:'Tâche', title:'Ce que nous voulons changer maintenant', text:'Ajouter observed / expected / percent à une série, sur une journée civile choisie.', file:'specs/coverage.md + fonction Python ciblée' },
 { name:'Preuves', title:'Ce qui permet de dire « terminé »', text:'24/24, 23/24, zéro valide, doublon, journées de 23 et 25 heures. Lire les tests et le résultat.', file:'tests Python + report.json' },
]
</script>
<template><section class="interactive" aria-label="Explorer les paliers de contexte">
<div class="controls"><button v-for="(layer,i) in layers" :key="layer.name" :class="{active:selected===i}" @click="selected=i">{{i+1}}. {{layer.name}}</button></div>
<div class="detail" aria-live="polite"><h3>{{layers[selected].title}}</h3><p>{{layers[selected].text}}</p><code>{{layers[selected].file}}</code></div>
<p class="context-summary">Vue d’ensemble : socle → composant → contrat → tâche → preuves.<br>Choisir les détails utiles, sans joindre tous les fichiers à chaque demande.</p>
</section></template>
