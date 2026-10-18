export class ApiError extends Error { constructor(public status: number, message: string) { super(message) } }
export async function api<T>(path: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(`/api${path}`, { ...options, headers: { 'Content-Type': 'application/json', ...options.headers }, signal: AbortSignal.timeout(15000) })
  if (!response.ok) {
    let message = `Le service a répondu ${response.status}.`
    const body = await response.text()
    try { const parsed = JSON.parse(body); message = parsed.message || parsed.detail || parsed.title || parsed.error || message } catch { if (body && body.length < 250) message = body }
    throw new ApiError(response.status, message)
  }
  return response.json() as Promise<T>
}
