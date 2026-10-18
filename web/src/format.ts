import type { Point, Series } from './types'
export const number = (value: number | null | undefined, digits = 0): string => value == null || !Number.isFinite(value) ? '—' : new Intl.NumberFormat('fr-CA', { maximumFractionDigits: digits }).format(Number(value.toFixed(digits)) || 0)
export function dateTime(value: string | null | undefined, timezone = 'America/Toronto'): string {
  if (!value || !Number.isFinite(Date.parse(value))) return '—'
  try { return new Intl.DateTimeFormat('fr-CA', { timeZone: timezone, month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' }).format(new Date(value)) } catch { return '—' }
}
export function pointsInWindow(points: Point[], hours: number, end: number): Point[] {
  return points.filter(p => Number.isFinite(Date.parse(p.timestamp)) && (!hours || Date.parse(p.timestamp) >= end - hours * 3_600_000)).map(p => ({ ...p, value: p.value != null && Number.isFinite(p.value) ? p.value : null })).sort((a, b) => Date.parse(a.timestamp) - Date.parse(b.timestamp))
}
export function coverage(series: Series[]): number | null {
  if (!series.length || series.some(s => !s.coverage)) return null
  const expected = series.reduce((sum, s) => sum + (s.coverage?.expected ?? 0), 0)
  return expected > 0 ? series.reduce((sum, s) => sum + (s.coverage?.observed ?? 0), 0) / expected * 100 : null
}
export function segments(points: Point[], intervalMinutes?: number): Point[][] {
  const result: Point[][] = []; let part: Point[] = []
  for (const point of points) {
    const previous = part.at(-1)
    if (previous && intervalMinutes && Date.parse(point.timestamp) - Date.parse(previous.timestamp) > intervalMinutes * 60000 * 1.5) { result.push(part); part = [] }
    if (point.value == null) { if (part.length) result.push(part); part = [] } else part.push(point) }
  if (part.length) result.push(part)
  return result
}
export function freshness(value: string, now = Date.now()): string {
  const parsed = Date.parse(value)
  if (!Number.isFinite(parsed)) return 'Date inconnue'
  const minutes = Math.max(0, Math.floor((now - parsed) / 60000))
  if (minutes < 1) return 'à l’instant'
  if (minutes < 60) return `il y a ${minutes} min`
  if (minutes < 1440) return `il y a ${Math.floor(minutes / 60)} h`
  return `il y a ${Math.floor(minutes / 1440)} j`
}
export function safeSourceUrl(value: string): string | undefined {
  try { const url = new URL(value); return ['http:', 'https:'].includes(url.protocol) ? url.href : undefined } catch { return undefined }
}

