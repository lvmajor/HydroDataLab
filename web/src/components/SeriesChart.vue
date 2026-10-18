<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import type { Series } from '../types'
import { dateTime, number, segments } from '../format'
const props = withDefaults(defineProps<{ series: Series[]; unit: string; colors?: string[]; compact?: boolean; timezone?: string }>(), { colors: () => ['#0074c8', '#22aeb8'], timezone: 'America/Toronto' })
const selected = ref<number | null>(null)
const width = props.compact ? 560 : 880, height = 300, left = 65, right = 22, top = 28, bottom = 45
const timestamps = computed(() => [...new Set(props.series.flatMap(s => s.points.map(p => Date.parse(p.timestamp))))].sort((a, b) => a - b))
const values = computed(() => props.series.flatMap(s => s.points.map(p => p.value).filter((v): v is number => v != null && Number.isFinite(v))))
const domain = computed(() => {
  if (!values.value.length) return [0, 1]
  const min = Math.min(...values.value), max = Math.max(...values.value), padding = (max - min || Math.abs(max) * .02 || 1) * .15
  return [min - padding, max + padding]
})
const start = computed(() => timestamps.value[0] ?? 0)
const end = computed(() => timestamps.value.at(-1) ?? 1)
const x = (time: number) => left + (time - start.value) / (end.value - start.value || 1) * (width - left - right)
const y = (value: number) => top + (domain.value[1]! - value) / (domain.value[1]! - domain.value[0]!) * (height - top - bottom)
const ticks = computed(() => Array.from({ length: 4 }, (_, i) => domain.value[0]! + (domain.value[1]! - domain.value[0]!) * i / 3))
const lines = computed(() => props.series.map((s, i) => ({ series: s, color: props.colors[i % props.colors.length], segments: segments(s.points, s.intervalMinutes).map(points => points.map(p => `${x(Date.parse(p.timestamp))},${y(p.value!)}`).join(' ')) })))
const activeTime = computed(() => selected.value == null ? null : timestamps.value[selected.value] ?? null)
const activeValues = computed(() => activeTime.value == null ? [] : props.series.map((s, i) => ({ label: s.label, color: props.colors[i % props.colors.length], value: s.points.find(p => Date.parse(p.timestamp) === activeTime.value)?.value ?? null })))
function inspect(event: PointerEvent) {
  const rect = (event.currentTarget as SVGElement).getBoundingClientRect()
  const ratio = Math.max(0, Math.min(1, ((event.clientX - rect.left) / rect.width * width - left) / (width - left - right)))
  const target = start.value + ratio * (end.value - start.value)
  selected.value = timestamps.value.reduce((best, value, index, all) => Math.abs(value - target) < Math.abs(all[best]! - target) ? index : best, 0)
}
function keyboard(event: KeyboardEvent) {
  if (!['ArrowRight', 'ArrowLeft', 'Home', 'End', 'Escape'].includes(event.key)) return
  event.preventDefault()
  if (event.key === 'Escape') { selected.value = null; return }
  if (event.key === 'Home') selected.value = 0
  else if (event.key === 'End') selected.value = timestamps.value.length - 1
  else selected.value = Math.max(0, Math.min(timestamps.value.length - 1, (selected.value ?? 0) + (event.key === 'ArrowRight' ? 1 : -1)))
}
watch(() => props.series, () => { selected.value = null })
</script>
<template>
  <div class="chart" :class="{ compact }">
    <div v-if="!values.length" class="chart-empty">Aucune mesure disponible pour cette période.</div>
    <svg v-else :viewBox="`0 0 ${width} ${height}`" role="img" :aria-label="`${series.map(s => s.label).join(' et ')}, en ${unit}. Flèches gauche et droite pour inspecter les mesures.`" tabindex="0" @pointermove="inspect" @pointerleave="selected = null" @keydown="keyboard">
      <g v-for="tick in ticks" :key="tick"><line :x1="left" :x2="width - right" :y1="y(tick)" :y2="y(tick)" stroke="#dbe5ee" stroke-dasharray="3 5"/><text :x="left - 12" :y="y(tick) + 5" text-anchor="end" class="axis-label">{{ number(tick, unit === 'm' ? 2 : 0) }}</text></g>
      <text :x="left" y="15" class="axis-label unit-label">{{ unit }}</text>
      <g v-for="line in lines" :key="line.series.id">
        <polyline v-for="(segment, i) in line.segments" :key="i" :points="segment" fill="none" :stroke="line.color" stroke-width="3" stroke-linejoin="round" stroke-linecap="round" vector-effect="non-scaling-stroke"/>
        <circle v-for="p in line.series.points.filter(p => p.value != null)" :key="p.timestamp" :cx="x(Date.parse(p.timestamp))" :cy="y(p.value!)" r="2.3" :fill="line.color"/>
      </g>
      <line v-if="activeTime != null" :x1="x(activeTime)" :x2="x(activeTime)" :y1="top" :y2="height - bottom" stroke="#183a5a" stroke-width="1" stroke-dasharray="4 4"/>
      <text :x="left" :y="height - 12" class="axis-label">{{ dateTime(new Date(start).toISOString(), timezone) }}</text>
      <text :x="width - right" :y="height - 12" text-anchor="end" class="axis-label">{{ dateTime(new Date(end).toISOString(), timezone) }}</text>
    </svg>
    <div class="chart-readout" aria-live="polite">
      <template v-if="activeTime != null"><span>{{ dateTime(new Date(activeTime).toISOString(), timezone) }}</span><span v-for="value in activeValues" :key="value.label"><i :style="{ background: value.color }"></i>{{ value.label }} <strong>{{ number(value.value, unit === 'm' ? 2 : 1) }} {{ unit }}</strong></span></template>
      <template v-else><span v-for="(s, i) in series" :key="s.id"><i :style="{ background: colors[i % colors.length] }"></i>{{ s.label }}</span><span class="inspect-hint">Survolez une courbe pour explorer</span></template>
    </div>
  </div>
</template>


