<script setup>
import resources from '../resources/links.json'
</script>

<template>
  <section class="resource-columns" aria-label="Ressources pour poursuivre la formation">
    <div v-for="group in resources.publicGroups" :key="group.title" class="resource-group">
      <h2>{{ group.title }}</h2>
      <div v-for="link in group.links" :key="link.label" class="resource-item">
        <a :href="link.url" target="_blank" rel="noopener noreferrer">{{ link.label }}</a>
        <span>{{ link.description }}</span>
      </div>
    </div>
    <div class="resource-group internal-resources">
      <h2>Chez Hydro-Québec</h2>
      <div v-for="link in resources.internal" :key="link.label" class="resource-item">
        <a v-if="link.url" :href="link.url" target="_blank" rel="noopener noreferrer">{{ link.label }}</a>
        <b v-else>{{ link.label }}</b>
        <span>{{ link.description }}</span>
        <small v-if="!link.url">Lien interne à ajouter</small>
      </div>
    </div>
  </section>
</template>
