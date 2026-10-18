import { describe, expect, it } from 'vitest'
import { coverage, pointsInWindow, segments, number, safeSourceUrl, freshness } from './format'
import type { Point, Series } from './types'
const point = (hour: string, value: number | null): Point => ({ timestamp: `2026-10-17T${hour}:00:00Z`, value })
const sample = (value: Series['coverage']): Series => ({ id: 'demand', label: 'Demande', station: 'Québec', unit: 'MW', kind: 'demand', intervalMinutes: 15, points: [], minimum: null, maximum: null, latest: null, coverage: value })
describe('Aucune mesure absente ne devient une certitude', () => {
  it('conserve une couverture inconnue lorsque l’une des séries ne la calcule pas', () => expect(coverage([sample({ observed: 96, expected: 96, percent: 100 }), sample(null)])).toBeNull())
  it('pondère les fréquences au lieu de moyenner les pourcentages', () => expect(coverage([sample({ observed: 90, expected: 100, percent: 90 }), sample({ observed: 1, expected: 2, percent: 50 })])).toBeCloseTo(91 / 102 * 100))
  it('ne relie pas les mesures de part et d’autre d’une absence', () => expect(segments([point('10', 1), point('11', null), point('12', 3)])).toEqual([[point('10', 1)], [point('12', 3)]]))
  it('conserve zéro comme une vraie mesure', () => expect(number(0)).toBe('0'))
  it('affiche un tiret pour une mesure non finie', () => expect(number(Number.NaN)).toBe('—'))
})
describe('Fenêtre temporelle explicite', () => {
  it('filtre par le temps des données, trie et conserve les trous', () => expect(pointsInWindow([point('12', 2), point('05', 9), point('10', null)], 6, Date.parse(point('12', 0).timestamp))).toEqual([point('10', null), point('12', 2)]))
  it('écarte les dates invalides et transforme les valeurs non finies en absence', () => expect(pointsInWindow([{ timestamp: 'invalide', value: 3 }, point('10', Infinity)], 0, 0)).toEqual([point('10', null)]))
  it('n’invente pas une ancienneté négative en cas de décalage de l’horloge', () => expect(freshness('2026-10-18T12:01:00Z', Date.parse('2026-10-18T12:00:00Z'))).toBe('à l’instant'))
})
it('refuse les liens exécutables dans la provenance', () => { expect(safeSourceUrl('javascript:alert(1)')).toBeUndefined(); expect(safeSourceUrl('https://donnees.hydroquebec.com/')).toBe('https://donnees.hydroquebec.com/') })

it('coupe aussi une série lorsqu’un horodatage attendu manque entièrement', () => expect(segments([point('10', 1), point('12', 3)], 60)).toEqual([[point('10', 1)], [point('12', 3)]]))
it('évite une graduation négative zéro', () => expect(number(-0.00001, 0)).toBe('0'))
