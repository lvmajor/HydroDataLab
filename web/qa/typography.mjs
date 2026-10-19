// Read-only UI review. Alternate states are intercepted in this browser only.
import { chromium } from '@playwright/test'
import { mkdir, writeFile } from 'node:fs/promises'
import { fileURLToPath } from 'node:url'
import assert from 'node:assert/strict'

const baseURL = process.env.HYDRO_WEB_URL || 'http://localhost:53170'
const output = fileURLToPath(new URL('../../.runtime/typography-qa/', import.meta.url))
await mkdir(output, { recursive: true })
const browser = await chromium.launch({ channel: process.env.HYDRO_BROWSER_CHANNEL || 'msedge', headless: true })
const errors = [], requests = [], results = []
const context = await browser.newContext({ viewport: { width: 1366, height: 768 }, reducedMotion: 'reduce' })
context.on('page', page => {
  page.on('pageerror', error => errors.push(error.message))
  page.on('request', request => {
    const url = new URL(request.url())
    if (['http:', 'https:'].includes(url.protocol)) requests.push({ origin: url.origin, method: request.method() })
  })
})
const page = await context.newPage()
try {
  const response = await context.request.get(`${baseURL}/api/report`)
  assert.equal(response.status(), 200, 'Charger d’abord un rapport de démonstration dans Vue.')
  const report = await response.json()
  const runResponse = await context.request.get(`${baseURL}/api/runs`)
  const runs = await runResponse.json()
  let state = 'complete'
  await context.route('**/api/**', async route => {
    const pathname = new URL(route.request().url()).pathname
    assert.equal(route.request().method(), 'GET', 'Ce contrôle ne doit modifier aucun service.')
    if (state === 'offline') return route.fulfill({ status: 503, body: '{}' })
    if (state === 'loading') await new Promise(resolve => setTimeout(resolve, 1200))
    if (pathname === '/api/report') {
      if (state === 'empty') return route.fulfill({ status: 404, body: '{}' })
      const data = structuredClone(report)
      if (state === 'unknown') data.series.forEach(series => { series.coverage = null })
      if (state === 'missing') {
        const water = data.series.find(series => series.id === 'water_level')
        water.coverage = { observed: 23, expected: 24, percent: 95.83 }
        data.summary.observationCount = 167
        data.warnings = ['Une mesure de niveau d’eau est absente; la couverture est de 95,83 %.']
      }
      return route.fulfill({ json: data })
    }
    if (pathname === '/api/runs') {
      const sample = { id: 'typography-only', source: 'fixture', status: state === 'active' ? 'running' : state, stage: 'extract-transform-export', createdAtUtc: report.generatedAtUtc, startedAtUtc: report.generatedAtUtc, qualityScenario: 'complete' }
      if (state === 'failed' || state === 'interrupted') sample.error = 'La collecte a été interrompue. Le dernier rapport reste disponible; vous pouvez relancer ce traitement.'
      return route.fulfill({ json: state === 'empty' ? [] : ['active', 'failed', 'interrupted'].includes(state) ? [sample, ...runs] : runs })
    }
    if (pathname === '/api/schedule') return route.fulfill({ json: { enabled: false, intervalMinutes: 5 } })
    throw new Error(`API inattendue : ${pathname}`)
  })

  const audit = async name => {
    const result = await page.evaluate(() => {
      const color = value => (value.match(/[\d.]+/g) || []).map(Number)
      const blend = (a, b) => a.slice(0, 3).map((c, i) => c * (a[3] ?? 1) + b[i] * (1 - (a[3] ?? 1)))
      const luminance = rgb => rgb.slice(0, 3).map(c => c / 255).map(c => c <= .04045 ? c / 12.92 : ((c + .055) / 1.055) ** 2.4).reduce((v, c, i) => v + c * [.2126, .7152, .0722][i], 0)
      const background = el => {
        const stack = []
        for (let node = el; node; node = node.parentElement) stack.push(color(getComputedStyle(node).backgroundColor))
        return stack.reverse().reduce((bg, fg) => blend(fg, bg), [255, 255, 255])
      }
      const small = [], contrast = [], overflow = [], svgOverflow = []
      let minSize = Infinity, minContrast = Infinity, textCount = 0
      for (const el of document.querySelectorAll('body *')) {
        if (!el.checkVisibility() || ['OPTION', 'SCRIPT', 'STYLE'].includes(el.tagName) || el.closest('[aria-hidden="true"]')) continue
        const text = [...el.childNodes].filter(n => n.nodeType === Node.TEXT_NODE).map(n => n.textContent).join('').trim()
        if (!text) continue
        const style = getComputedStyle(el), size = parseFloat(style.fontSize)
        const sample = { tag: el.tagName, class: el.getAttribute('class'), text: text.slice(0, 70) }
        minSize = Math.min(minSize, size); textCount++
        if (size < 15.99) small.push({ ...sample, size })
        if (!el.closest(':disabled') && Number(style.opacity) === 1) {
          const bg = background(el), fg = blend(color(el instanceof SVGElement ? style.fill : style.color), bg)
          const a = luminance(fg), b = luminance(bg), ratio = (Math.max(a, b) + .05) / (Math.min(a, b) + .05)
          minContrast = Math.min(minContrast, ratio)
          if (ratio < 4.5) contrast.push({ ...sample, ratio })
        }
        if (el instanceof SVGTextElement) {
          const box = el.getBoundingClientRect(), chart = el.ownerSVGElement.getBoundingClientRect()
          if (box.left < chart.left - 1 || box.right > chart.right + 1 || box.top < chart.top - 1 || box.bottom > chart.bottom + 1) svgOverflow.push(sample)
        } else if (!el.closest('.table-scroll')) {
          const box = el.getBoundingClientRect()
          if (box.right > innerWidth + 1 || box.left < -1) overflow.push(sample)
        }
      }
      return { width: innerWidth, pageWidth: document.documentElement.scrollWidth, textCount, minSize, minContrast, small, contrast, overflow, svgOverflow }
    })
    results.push({ name, ...result })
    await writeFile(`${output}/results.json`, JSON.stringify({ status: 'in-progress', results, errors }, null, 2))
    assert.deepEqual(result.small, [], `${name}: textes trop petits`)
    assert.deepEqual(result.contrast, [], `${name}: contraste insuffisant`)
    assert.deepEqual(result.overflow, [], `${name}: texte hors fenêtre`)
    assert.deepEqual(result.svgOverflow, [], `${name}: texte SVG coupé`)
    assert.ok(result.pageWidth <= result.width + 1, `${name}: défilement horizontal de page`)
  }
  const load = async () => {
    await page.goto(baseURL)
    if (state !== 'loading') await page.getByRole('heading', { name: /Demande électrique|Le territoire attend|Le poste de contrôle/ }).first().waitFor()
    if (!['loading', 'empty', 'offline'].includes(state)) await page.locator('.demand-panel svg').waitFor()
    // Let ResizeObserver apply SVG dimensions before measuring or capturing.
    await page.evaluate(() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve))))
  }
  for (const [name, width, height] of [['laptop', 1366, 768], ['projection', 1920, 1080], ['desktop', 1280, 900], ['mobile', 390, 844], ['reflow', 320, 800], ['zoom-200-layout', 640, 450]]) {
    await page.setViewportSize({ width, height })
    await load()
    await page.locator('.coverage-details summary').click()
    await page.getByRole('button', { name: 'Afficher le tableau' }).click()
    await page.locator('.schedule-section summary').click()
    await audit(name)
    if (['laptop', 'projection', 'mobile'].includes(name)) {
      await page.getByRole('button', { name: 'Masquer le tableau' }).click()
      await page.locator('.coverage-details summary').click()
      await page.locator('.schedule-section summary').click()
      await page.evaluate(() => scrollTo(0, 0))
      await page.screenshot({ path: `${output}/${name}.png`, fullPage: name === 'projection' })
    }
  }
  await page.setViewportSize({ width: 1280, height: 900 })
  await load()
  await page.addStyleTag({ content: 'html { font-size: 200% !important; }' })
  await page.evaluate(() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve))))
  await audit('text-200')
  await page.screenshot({ path: `${output}/text-200.png` })
  for (const width of [1280, 320]) {
    await page.setViewportSize({ width, height: 900 })
    await load()
    await page.addStyleTag({ content: '* { line-height:1.5 !important; letter-spacing:.12em !important; word-spacing:.16em !important; } p { margin-bottom:2em !important; }' })
    await audit(`text-spacing-${width}`)
  }
  await page.setViewportSize({ width: 1366, height: 768 })
  for (state of ['unknown', 'missing', 'active', 'failed', 'interrupted', 'empty', 'offline', 'loading']) {
    await load()
    if (state === 'empty') await page.getByRole('heading', { name: 'Le territoire attend ses premières mesures.' }).waitFor()
    if (state === 'offline') await page.getByRole('alert').waitFor()
    if (state === 'loading') await page.getByRole('heading', { name: 'Connexion à l’observatoire' }).waitFor()
    await audit(state)
  }
  state = 'complete'
  await load()
  for (const chart of await page.getByRole('img').all()) {
    await chart.focus()
    assert.notEqual(await chart.evaluate(el => getComputedStyle(el).outlineStyle), 'none')
    await page.keyboard.press('End')
    await page.keyboard.press('Home')
    assert.match(await chart.locator('..').locator('.chart-readout').innerText(), /\d/)
    await page.keyboard.press('Escape')
  }
  await page.getByRole('button', { name: '6 h', exact: true }).click()
  assert.equal(await page.getByRole('button', { name: '6 h', exact: true }).getAttribute('aria-pressed'), 'true')
  assert.deepEqual(errors, [])
  assert.ok(requests.every(r => r.origin === new URL(baseURL).origin && r.method === 'GET'))
  await writeFile(`${output}/results.json`, JSON.stringify({ status: 'passed', date: new Date().toISOString(), results, errors, checks: ['browser-only alternate states; no service mutations', 'keyboard chart inspection and focus', '6h filter', 'no external requests'], limits: ['Targeted review, not WCAG certification', '200% layout uses a 640 CSS-pixel viewport; actual room projector and browser zoom require human rehearsal'] }, null, 2))
  console.log(`${results.length} états/formats vérifiés, textes >=16px, contraste >=4.5:1, aucune erreur JavaScript. Captures : ${output}`)
} finally { await browser.close() }
