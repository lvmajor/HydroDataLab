export type Source = 'fixture' | 'live'
export type QualityScenario = 'complete' | 'missing'
export interface Point { timestamp: string; value: number | null }
export interface Series { id: string; label: string; station: string; unit: string; kind: string; intervalMinutes: number; points: Point[]; minimum: number | null; maximum: number | null; latest: number | null; coverage: { observed: number; expected: number; percent: number } | null }
export interface Report { schemaVersion: number; runId: string; source: Source; generatedAtUtc: string; period: { start: string; end: string; timezone: string }; qualityScenario: QualityScenario; summary: { observationCount: number; seriesCount: number; invalidCount: number; duplicateCount: number }; series: Series[]; provenance: { label: string; url: string; license: string; retrievedAtUtc: string }[]; warnings: string[]; artifacts: { csv: string; parquet: string; report: string } }
export interface Run { id: string; source: Source; status: 'queued' | 'running' | 'succeeded' | 'failed' | 'interrupted'; stage: string; createdAtUtc: string; startedAtUtc: string | null; completedAtUtc: string | null; error: string | null; qualityScenario: QualityScenario }
export interface Schedule { enabled: boolean; intervalMinutes: number }
