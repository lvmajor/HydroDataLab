export type Source = 'fixture' | 'live'
export type QualityScenario = 'complete' | 'missing'
export interface Point { timestamp: string; value: number | null }
export interface Series { id: string; label: string; station: string; unit: string; kind: string; intervalMinutes: number; points: Point[]; minimum: number | null; maximum: number | null; latest: number | null; coverage: { observed: number; expected: number; percent: number } | null }
export interface Hour { startUtc: string; localHour: string; observed: number; expected: number; meanDemandMW: number | null }
export interface Analysis { hourly: Hour[]; completeHours: number; finishedHours: number; indicators: { peakHourlyMW: number | null; peakHourUtc: string | null; estimatedEnergyMWh: number | null; waterChangeCm: number | null; waterStartUtc: string | null; waterEndUtc: string | null } }
export interface Etl { rawCount: number; normalizedCount: number; validCount: number; missingCount: number; scenarioRemovedCount: number; rawDemandSample: { date: string; valeurs_demandetotal: unknown }[]; normalizedDemandSample: { timestamp_utc: string; value: number | null; unit: string }[] }
export interface Execution { steps: { id: string; label: string; status: string; startedAtUtc: string; completedAtUtc: string; durationMs: number; error?: string }[] }
export interface Report { schemaVersion: number; runId: string; source: Source; generatedAtUtc: string; period: { start: string; end: string; timezone: string }; qualityScenario: QualityScenario; summary: { observationCount: number; seriesCount: number; invalidCount: number; duplicateCount: number }; series: Series[]; provenance: { label: string; url: string; license: string; retrievedAtUtc: string }[]; warnings: string[]; artifacts: { csv: string; parquet: string; report: string; hourly?: string }; analysis?: Analysis; etl?: Etl; execution?: Execution }
export interface RunDetails { run: Run; retryOf: string | null; simulateFailure: boolean | null; report: Report | null; execution: Execution | null }
export interface Run { id: string; source: Source; status: 'queued' | 'running' | 'succeeded' | 'failed' | 'interrupted'; stage: string; createdAtUtc: string; startedAtUtc: string | null; completedAtUtc: string | null; error: string | null; qualityScenario: QualityScenario }
export interface Schedule { enabled: boolean; intervalMinutes: number }
