// Integration evidence for the real fixture pipeline and its Vue presentation.
import { chromium, expect } from '@playwright/test'
import { mkdir, writeFile, readFile } from 'node:fs/promises'
import { fileURLToPath } from 'node:url'
import assert from 'node:assert/strict'
const base = process.env.HYDRO_WEB_URL || 'http://localhost:53170'
const out = fileURLToPath(new URL('../../.runtime/etl-qa/', import.meta.url))
await mkdir(out, { recursive: true })
const browser = await chromium.launch({ channel: process.env.HYDRO_BROWSER_CHANNEL || 'msedge', headless: true })
const context = await browser.newContext({ viewport: { width: 1920, height: 1080 }, reducedMotion: 'reduce', acceptDownloads: true })
const page = await context.newPage(), errors = [], external = []
page.on('pageerror', error => errors.push(error.message))
page.on('request', request => { if (new URL(request.url()).origin !== new URL(base).origin) external.push(request.url()) })
async function finish(id, expected = 'succeeded') {
  let result
  await expect.poll(async () => { result = await (await context.request.get(`${base}/api/runs/${id}/details`)).json(); return result.run.status }, { timeout: 30000 }).toBe(expected)
  return result
}
async function launch(body) {
  const response = await context.request.post(`${base}/api/runs`, { data: { source: 'fixture', ...body } })
  assert.equal(response.status(), 202)
  return (await response.json()).id
}
async function layout() {
  assert.equal(await page.evaluate(() => document.documentElement.scrollWidth <= innerWidth + 1), true, 'Page sans débordement horizontal')
  const small = await page.locator('#etl, #execution').evaluateAll(roots => roots.flatMap(root => [...root.querySelectorAll('*')].filter(el => el.getBoundingClientRect().height && [...el.childNodes].some(n => n.nodeType === 3 && n.textContent.trim()) && parseFloat(getComputedStyle(el).fontSize) < 16).map(el => el.textContent.slice(0, 80))))
  assert.deepEqual(small, [], 'Aucun nouveau texte inférieur à 16 px')
}
try {
  const id = await launch({}), successful = await finish(id)
  assert.equal(successful.report.analysis.hourly.length, 24)
  assert.equal(successful.execution.steps.length, 4)
  assert.equal(successful.report.etl.rawCount, 168)
  await page.goto(base)
  await expect(page.locator('#etl')).toBeVisible()
  await page.locator('#etl').screenshot({ path: `${out}/01-avant.png` })
  await page.getByRole('button', { name: /2 · Transformer/ }).click()
  await expect(page.getByRole('region', { name: 'Échantillon normalisé de demande' })).toContainText('2026-10-17T04:00:00Z')
  await page.locator('#etl').screenshot({ path: `${out}/02-apres.png` })
  await page.getByRole('button', { name: /3 · Charger/ }).click()
  await page.getByRole('button', { name: 'Afficher la synthèse horaire' }).click()
  await expect(page.getByRole('region', { name: 'Synthèse horaire', exact: true }).locator('tbody tr')).toHaveCount(24)
  const downloadPromise = page.waitForEvent('download')
  await page.getByRole('link', { name: 'Télécharger la synthèse CSV' }).click()
  const download = await downloadPromise
  assert.equal(await download.failure(), null)
  assert.equal((await readFile(await download.path(), 'utf8')).trim().split('\n').length, 25)
  await page.locator('#etl').screenshot({ path: `${out}/03-synthese.png` })
  await page.getByLabel('Exécution à consulter (100 dernières)').selectOption(id)
  await expect(page.getByRole('region', { name: 'Étapes mesurées du traitement' }).locator('tbody tr')).toHaveCount(4)
  await page.locator('#execution').screenshot({ path: `${out}/04-execution.png` })
  const failedId = await launch({ simulateFailure: true }), failed = await finish(failedId, 'failed')
  assert.equal(failed.execution.steps[0].status, 'failed')
  assert.equal(failed.execution.steps.length, 1)
  assert.equal(failed.report, null)
  assert.equal((await (await context.request.get(`${base}/api/report`)).json()).runId, id)
  await expect(page.locator('.run-history li').first()).toContainText('Échec')
  await page.locator('.run-history li').first().getByRole('button', { name: 'Voir les détails' }).click()
  await expect(page.locator('#execution .notice')).toContainText('Échec simulé')
  await page.locator('#execution').screenshot({ path: `${out}/05-echec.png` })
  const retried = await context.request.post(`${base}/api/runs/${failedId}/retry`)
  assert.equal(retried.status(), 202)
  const retryId = (await retried.json()).id, retry = await finish(retryId)
  assert.equal(retry.retryOf, failedId)
  await expect(page.locator('.run-history li').first()).toContainText('Terminé')
  await page.getByLabel('Exécution à consulter (100 dernières)').selectOption(retryId)
  await expect(page.locator('#execution')).toContainText(failedId)
  for (const width of [1366, 640, 390, 320]) {
    await page.setViewportSize({ width, height: 900 })
    for (const name of [/1 · Extraire/, /2 · Transformer/, /3 · Charger/]) {
      await page.getByRole('button', { name }).click(); await layout()
    }
  }
  await page.setViewportSize({ width: 1280, height: 900 })
  await page.addStyleTag({ content: 'html { font-size: 200% !important }' })
  await layout()
  await page.reload()
  await page.getByRole('button', { name: /3 · Charger/ }).click()
  await page.getByRole('button', { name: 'Afficher la synthèse horaire' }).click()
  await page.addStyleTag({ content: '* { line-height: 1.5 !important; letter-spacing: .12em !important; word-spacing: .16em !important } p { margin-bottom: 2em !important }' })
  await layout()
  // Older persisted reports remain useful and explicitly request a fresh run for new analyses.
  await context.route('**/api/report', async route => { const old = structuredClone(successful.report); delete old.analysis; delete old.etl; delete old.execution; await route.fulfill({ json: old }) })
  await page.reload()
  await expect(page.getByText(/Ce rapport précède les nouvelles analyses/)).toBeVisible()
  assert.deepEqual(errors, []); assert.deepEqual(external, [])
  const evidence = { completedAt: new Date().toISOString(), status: 'passed', base, ids: { id, failedId, retryId }, indicators: successful.report.analysis.indicators, checks: ['real ETL calculations', 'raw and normalized samples', '24 hourly rows', 'hourly CSV download', '4 measured stages', 'failure diagnostics', 'last successful report preserved', 'linked retry', 'legacy report', '320px reflow', '200% text', 'text spacing', 'no JS errors or external requests'] }
  await writeFile(`${out}/results.json`, JSON.stringify(evidence, null, 2)); console.log(JSON.stringify(evidence, null, 2))
} finally { await browser.close() }
