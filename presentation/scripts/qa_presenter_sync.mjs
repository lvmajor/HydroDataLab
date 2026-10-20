import assert from 'node:assert/strict'
import fs from 'node:fs/promises'
import path from 'node:path'
import { chromium } from 'playwright-chromium'

const origin = new URL(process.env.PRESENTATION_URL || 'http://127.0.0.1:53173').origin
const output = process.env.PRESENTATION_QA_DIR || 'exports/qa'
await fs.mkdir(output, { recursive: true })
const browser = await chromium.launch({
  headless: true,
  executablePath: process.env.SLIDEV_BROWSER_PATH || 'C:/Program Files/Google/Chrome/Application/chrome.exe',
})
const errors = [], external = new Set(), checks = []
const context = await browser.newContext({ viewport: { width: 1600, height: 900 } })
context.on('page', page => page.on('pageerror', error => errors.push(error.message)))
await context.route('**/*', route => {
  const url = route.request().url()
  if (new URL(url).origin !== origin && !/^(data|blob):/.test(url)) {
    external.add(url)
    return route.abort()
  }
  return route.continue()
})

const presenter = await context.newPage()
let audience
const current = () => presenter.locator('.grid-section.main .slidev-layout:visible')
const waitLayer = (page, layer) => page.waitForFunction(index => {
  const scope = document.querySelector('.grid-section.main') || document
  const buttons = scope.querySelectorAll('.context-layer')
  const preview = scope.querySelector('.context-preview')
  return buttons[index]?.getAttribute('aria-pressed') === 'true'
    && preview?.textContent.includes(`Apport ${index + 1}`)
    && scope.querySelectorAll('.context-connectors .selected').length === 1
}, layer)
const waitAttempts = (page, attempts) => page.waitForFunction(number => {
  const scope = document.querySelector('.grid-section.main') || document
  const label = scope.querySelector('.matrix-controls')?.textContent
  const cost = scope.querySelector('[data-model=opus] [data-task=debug] .cost-value')?.textContent
  return label?.includes(`${number} essai(s)`)
    && Math.abs(Number(cost?.replace(',', '.')) - .4 * number) < .000001
}, attempts)
const checkAllCosts = async () => {
  const values = await current().locator('.cost-value').allTextContents()
  assert.equal(values.length, 20)
  assert.deepEqual(await audience.locator('.cost-value').allTextContents(), values)
  assert.equal(await audience.locator('.matrix-legend').innerText(), await current().locator('.matrix-legend').innerText())
}
const openAudience = async slide => {
  const page = await context.newPage()
  await page.goto(`${origin}/${slide}`, { waitUntil: 'networkidle' })
  return page
}

try {
  await presenter.goto(`${origin}/presenter/11`, { waitUntil: 'networkidle' })
  await current().getByRole('button', { name: /Documents et code/ }).click()
  audience = await openAudience(11)
  await waitLayer(audience, 3)
  checks.push('Audience ouverte après la sélection : même couche immédiatement')

  for (let layer = 0; layer < 5; layer++) {
    await current().locator('.context-layer').nth(layer).click()
    await Promise.all([waitLayer(presenter, layer), waitLayer(audience, layer)])
    assert.equal(await audience.locator('.context-preview').innerText(), await current().locator('.context-preview').innerText())
    assert.equal(new URL(presenter.url()).pathname, '/presenter/11')
    assert.equal(new URL(audience.url()).pathname, '/11')
  }
  checks.push('Cinq couches : sélection, texte et flèche synchronisés, sans changer de diapositive')

  await audience.getByRole('button', { name: /Rôle et demande/ }).click()
  await waitLayer(presenter, 2)
  await presenter.getByTitle('Go to next slide', { exact: true }).click()
  await audience.waitForURL(url => url.pathname === '/12')
  await presenter.getByTitle('Go to previous slide', { exact: true }).click()
  await audience.waitForURL(url => url.pathname === '/11')
  await Promise.all([waitLayer(presenter, 2), waitLayer(audience, 2)])
  await presenter.reload({ waitUntil: 'networkidle' })
  await audience.reload({ waitUntil: 'networkidle' })
  await Promise.all([waitLayer(presenter, 2), waitLayer(audience, 2)])
  checks.push('Retour audience → présentateur, navigation native Slidev et rechargement conservant la couche')
  await presenter.screenshot({ path: path.join(output, 'sync-context-presenter.png') })
  await audience.screenshot({ path: path.join(output, 'sync-context-audience.png') })

  await presenter.goto(`${origin}/presenter/17`, { waitUntil: 'networkidle' })
  await audience.goto(`${origin}/17`, { waitUntil: 'networkidle' })
  await Promise.all([waitAttempts(presenter, 1), waitAttempts(audience, 1)])
  for (let attempts = 2; attempts <= 8; attempts++) {
    await current().getByRole('button', { name: 'Ajouter une tentative', exact: true }).click()
    await Promise.all([waitAttempts(presenter, attempts), waitAttempts(audience, attempts)])
    await checkAllCosts()
  }
  await current().getByRole('button', { name: 'Ajouter une tentative', exact: true }).click()
  await Promise.all([waitAttempts(presenter, 8), waitAttempts(audience, 8)])
  checks.push('20 coûts, compteur et légende synchronisés à chaque hausse, plafond de 8 essais')

  await audience.getByRole('button', { name: 'Réduire les tentatives', exact: true }).click()
  await waitAttempts(presenter, 7)
  await audience.close()
  audience = await openAudience(17)
  await waitAttempts(audience, 7)
  await presenter.reload({ waitUntil: 'networkidle' })
  await Promise.all([waitAttempts(presenter, 7), waitAttempts(audience, 7)])
  checks.push('Coûts conservés à la réouverture audience et au rechargement présentateur; retour dans les deux sens')

  for (let attempts = 6; attempts >= 1; attempts--) {
    await current().getByRole('button', { name: 'Réduire les tentatives', exact: true }).click()
    await Promise.all([waitAttempts(presenter, attempts), waitAttempts(audience, attempts)])
    await checkAllCosts()
  }
  await current().getByRole('button', { name: 'Réduire les tentatives', exact: true }).click()
  await Promise.all([waitAttempts(presenter, 1), waitAttempts(audience, 1)])
  assert.equal(new URL(presenter.url()).pathname, '/presenter/17')
  assert.equal(new URL(audience.url()).pathname, '/17')
  checks.push('Baisse synchronisée, plancher de 1 essai, aucun changement de page involontaire')
  await presenter.screenshot({ path: path.join(output, 'sync-costs-presenter.png') })
  await audience.screenshot({ path: path.join(output, 'sync-costs-audience.png') })

  await presenter.goto(`${origin}/presenter/11`, { waitUntil: 'networkidle' })
  await waitLayer(presenter, 2)
  checks.push('Les réglages des deux composants restent indépendants après un aller-retour')
  assert.deepEqual(errors, [])
  assert.deepEqual([...external], [])
  await fs.writeFile(path.join(output, 'presenter-sync.json'), JSON.stringify({
    status: 'passed', testedAtUtc: new Date().toISOString(), origin,
    checks, errors, external: [...external],
    conditions: 'Deux pages du même contexte Chromium; vue présentateur réelle; requêtes externes bloquées',
  }, null, 2) + '\n')
  console.log(JSON.stringify({ status: 'passed', checks, errors, external: [...external] }))
} finally {
  await browser.close()
}
