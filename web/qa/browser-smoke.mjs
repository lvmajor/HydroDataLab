import { chromium } from '@playwright/test'
import { mkdir, readFile, writeFile } from 'node:fs/promises'
import { resolve } from 'node:path'
const baseURL = process.env.HYDRO_WEB_URL || 'http://127.0.0.1:53170'
const shots = resolve('../docs/screenshots')
await mkdir(shots, { recursive: true })
await mkdir('qa-results', { recursive: true })
const browser = await chromium.launch({ channel: 'msedge', headless: true })
const context = await browser.newContext({ viewport: { width: 1440, height: 1100 }, acceptDownloads: true, reducedMotion: 'reduce' })
const page = await context.newPage()
const errors = []
const externalRequests = []
page.on('request', request => { const url = new URL(request.url()); if (['http:', 'https:'].includes(url.protocol) && url.origin !== new URL(baseURL).origin) externalRequests.push(url.origin + url.pathname) })
page.on('pageerror', e => errors.push(e.message))
const check = (condition, message) => { if (!condition) throw new Error(message) }
const waitNotice = text => page.getByRole('status').filter({ hasText: text }).waitFor({ timeout: 30000 })
async function latestRunStatus(expected) {
  await page.waitForFunction(status => document.querySelector('.run-history li .status-text')?.textContent === status, expected, { timeout: 60000 })
}
try {
  await page.goto(baseURL, { waitUntil: 'networkidle' })
  await page.getByRole('button', { name: 'Lancer la collecte', exact: true }).waitFor()
  await page.screenshot({ path: resolve(shots, '01-poste-controle.png'), fullPage: true })
  await page.getByRole('button', { name: 'Lancer la collecte', exact: true }).click()
  await latestRunStatus('Terminé')
  await page.getByRole('heading', { name: 'Demande électrique', exact: true }).waitFor()
  check(await page.getByRole('img').count() === 3, 'Trois graphiques attendus')
  check(await page.locator('.weather-panel polyline').count() >= 2, 'Deux séries météo distinctes attendues')
  await page.getByRole('button', { name: '6 h', exact: true }).click()
  check(await page.getByRole('button', { name: '6 h', exact: true }).getAttribute('aria-pressed') === 'true', 'Filtre 6 h non activé')
  await page.getByRole('button', { name: 'Tout', exact: true }).click()
  await page.getByRole('img').first().focus()
  await page.keyboard.press('ArrowRight')
  check((await page.locator('.demand-panel .chart-readout').innerText()).includes('MW'), 'Inspection clavier sans unité')
  await page.keyboard.press('Escape')
  await page.getByRole('heading', { name: 'Demande électrique', exact: true }).click()
  await page.screenshot({ path: resolve(shots, '02-observatoire-complet.png'), fullPage: true })
  await page.getByRole('checkbox', { name: /Retirer une mesure/ }).check()
  await page.getByRole('button', { name: 'Lancer la collecte', exact: true }).click()
  await latestRunStatus('Terminé')
  await page.locator('.warnings').waitFor()
  const missingReport = await (await page.request.get(baseURL + '/api/report')).json()
  check(missingReport.series.find(s => s.id === 'water_level').coverage.percent < 100, 'La mesure absente doit réduire la couverture')
  await page.getByRole('button', { name: 'Afficher le tableau' }).click()
  check(await page.locator('.table-scroll tbody tr').count() > 0, 'Tableau de mesures vide')
  await page.locator('.coverage-details summary').click()
  check((await page.locator('.coverage-details').innerText()).includes('95,83'), 'Couverture du Gouin non visible')
  await page.screenshot({ path: resolve(shots, '03-qualite-mesure-absente.png'), fullPage: true })
  await page.locator('.coverage-details summary').click()
  await page.getByRole('button', { name: 'Masquer le tableau' }).click()
  await page.getByRole('checkbox', { name: /Retirer une mesure/ }).uncheck()
  await page.getByRole('button', { name: 'Démontrer un échec contrôlé' }).click()
  await latestRunStatus('Échec')
  await page.screenshot({ path: resolve(shots, '04-echec-controle.png'), fullPage: true })
  await page.getByRole('button', { name: 'Relancer ce traitement' }).first().click()
  await latestRunStatus('Terminé')
  await page.locator('.schedule-section summary').click()
  await page.getByRole('checkbox', { name: 'Activer la planification' }).check()
  await page.getByLabel('Intervalle en minutes').fill('5')
  await page.getByRole('button', { name: 'Enregistrer', exact: true }).click()
  await waitNotice('Planification activée.')
  await page.getByRole('checkbox', { name: 'Activer la planification' }).uncheck()
  await page.getByRole('button', { name: 'Enregistrer', exact: true }).click()
  await waitNotice('Planification désactivée.')
  const schedule = await (await page.request.get(`${baseURL}/api/schedule`)).json()
  check(schedule.enabled === false, 'Planification doit rester désactivée après le test')
  for (const format of ['CSV', 'Parquet']) {
    const [download] = await Promise.all([page.waitForEvent('download'), page.getByRole('link', { name: new RegExp(`^${format}`) }).click()])
    check(await download.failure() === null, `${format}: téléchargement échoué`)
    const bytes = await readFile(await download.path())
    check(bytes.length > 50, `${format}: fichier vide`)
    if (format === 'Parquet') check(bytes.subarray(0, 4).toString() === 'PAR1', 'Signature Parquet absente')
  }
  await page.screenshot({ path: resolve(shots, '05-relance-et-exports.png'), fullPage: true })
  await page.setViewportSize({ width: 390, height: 844 })
  await page.screenshot({ path: resolve(shots, '06-mobile.png'), fullPage: true })
  check(await page.evaluate(() => document.documentElement.scrollWidth <= innerWidth), 'Débordement horizontal sur mobile')
  check(externalRequests.length === 0, 'Requêtes navigateur externes: ' + externalRequests.join('; '))
  check(errors.length === 0, `Erreurs JavaScript: ${errors.join('; ')}`)
  const result = { status: 'passed', baseURL, checks: ['fixture', '3 charts', '2 weather series', '6h filter', 'keyboard chart', 'missing value coverage', 'failed run', 'retry', 'schedule on/off', 'CSV download', 'Parquet signature', 'mobile width', 'no JavaScript errors', 'no external browser requests'], externalRequests, screenshots: shots, completedAt: new Date().toISOString() }
  await writeFile('qa-results/browser-smoke.json', JSON.stringify(result, null, 2))
  console.log(JSON.stringify(result, null, 2))
} finally { await browser.close() }




