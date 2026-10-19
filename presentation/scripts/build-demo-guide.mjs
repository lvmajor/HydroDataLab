// Markdown is the editorial source; the generated reader needs no server.
import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'
import MarkdownIt from 'markdown-it'

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../..')
const source = await fs.readFile(path.join(root, 'handouts/conducteur-demos.md'), 'utf8')
const md = new MarkdownIt({ html: false, linkify: true })
const slug = text => text.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/^-|-$/g, '')
const sections = source.split(/^## /m).slice(1).map(section => {
  const title = section.slice(0, section.indexOf('\n'))
  return { title, id: slug(title), content: md.render('## ' + section) }
})
const labels = ['Repères', 'Avant la séance', 'Démo 1 · 4 min', 'Démo 2 · 5 min', 'Transition vers Python', 'Démo 3 · 4 min', 'Secours à 2:10', 'Fin et reprise']
if (sections.length !== labels.length) throw new Error('Mettre à jour la navigation après un changement de sections.')
const links = sections.map((s, i) => `<a href="#${s.id}" data-section="${s.id}"><span>${String(i).padStart(2, '0')}</span>${md.utils.escapeHtml(labels[i])}</a>`).join('\n')
const articles = sections.map((s, i) => `<section id="${s.id}" tabindex="-1">${s.content}<div class="step-navigation">${i ? `<a href="#${sections[i-1].id}">← ${labels[i-1]}</a>` : '<span></span>'}${i < sections.length-1 ? `<a href="#${sections[i+1].id}">${labels[i+1]} →</a>` : `<a href="#${sections[0].id}">Retour aux repères ↑</a>`}</div></section>`).join('\n')
const html = `<!doctype html>
<html lang="fr"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1">
<title>HydroDataLab · Conducteur des trois démos</title>
<style>
:root{color-scheme:light;--ink:#073748;--muted:#466471;--teal:#007f83;--paper:#f7fafb;--line:#ccdde2;--gold:#e6b23c}
*{box-sizing:border-box}body{margin:0;color:var(--ink);background:var(--paper);font:18px/1.6 system-ui,Segoe UI,sans-serif}a{color:#006a80;text-underline-offset:3px}a:focus-visible,button:focus-visible,section:focus-visible{outline:3px solid var(--gold);outline-offset:4px}button{font:inherit;cursor:pointer}header{background:var(--ink);color:white;padding:26px 36px}header p{margin:0;color:#b6dfe1;font-size:14px;letter-spacing:.08em;text-transform:uppercase}h1{margin:5px 0;font-size:30px;line-height:1.2}header small{color:#d4e4e9}.shell{max-width:1320px;margin:auto;display:grid;grid-template-columns:245px minmax(0,1fr);gap:40px;padding:30px 32px 80px}aside{align-self:start;position:sticky;top:24px}nav{display:grid;gap:6px}nav a{display:flex;align-items:center;gap:10px;padding:10px 12px;border-radius:5px;color:var(--ink);text-decoration:none;font-size:15px;font-weight:650}nav a span{color:var(--muted);font-size:12px;font-weight:500}nav a[aria-current=page]{background:var(--ink);color:white}nav a[aria-current=page] span{color:#a6d9dc}nav a:hover{box-shadow:inset 0 0 0 1px var(--teal)}.private{border-left:3px solid var(--gold);padding-left:12px;color:var(--muted);font-size:14px;margin:24px 0}.print{border:1px solid var(--line);background:white;padding:7px 12px;border-radius:5px;font-size:14px}main{min-width:0}section{scroll-margin-top:20px;border:0}.interactive section:not(.active){display:none}h2{font-size:29px;line-height:1.25;margin:0 0 24px;border-bottom:3px solid var(--gold);padding-bottom:18px}h3{font-size:21px;line-height:1.4;margin:36px 0 12px;color:var(--teal)}p{margin:16px 0}strong{font-weight:720}li{margin:6px 0}table{border-collapse:collapse;font-size:15px;line-height:1.5;width:100%;margin:24px 0}th{text-align:left;background:#e5eff2}td,th{padding:12px 10px;border-bottom:1px solid var(--line);vertical-align:top}code{font-family:Consolas,monospace;font-size:.87em;background:#e9f0f2;padding:2px 4px;border-radius:3px;overflow-wrap:anywhere}pre{position:relative;background:#092f40;color:#e2f1f4;border-radius:7px;padding:45px 20px 20px;white-space:pre-wrap;overflow-wrap:anywhere;line-height:1.6;margin:18px 0 24px;font-size:16px}pre code{background:none;padding:0;border:0;color:inherit;font-size:inherit}.copy{position:absolute;top:8px;right:10px;border:1px solid #65838c;background:#163e4f;color:white;padding:3px 9px;border-radius:4px;font-size:12px}.copy-status{font-size:13px;color:var(--muted)}.step-navigation{display:flex;justify-content:space-between;gap:16px;border-top:1px solid var(--line);margin-top:44px;padding-top:20px;font-size:15px}.step-navigation a{text-decoration:none;font-weight:650}footer{border-top:1px solid var(--line);padding:15px 32px;color:var(--muted);font-size:13px}
@media(max-width:850px){header{padding:22px}.shell{display:block;padding:20px}aside{position:static;margin-bottom:30px}nav{grid-template-columns:repeat(2,minmax(0,1fr))}.private{margin:15px 0}h1{font-size:25px}h2{font-size:25px}table{font-size:13px}td,th{padding:8px 5px}}
@media print{body{font-size:11pt;color:#111;background:white}header{background:white;color:#111;padding:0}header p,header small{color:#333}.shell{display:block;padding:0}aside,.copy,.step-navigation,footer,.copy-status{display:none!important}.interactive section:not(.active){display:block}section{break-before:page}h2{font-size:20pt}h3{break-after:avoid;font-size:14pt;margin-top:20px}pre{padding:12px;background:#f1f3f4;color:#111;border:1px solid #ddd;font-size:10pt;break-inside:avoid}table{font-size:9pt}a{color:#111}p{orphans:3;widows:3}@page{size:A4;margin:16mm}}
</style></head><body>
<header><p>Conducteur privé · HydroDataLab · 20 octobre 2026</p><h1>Trois démos, un fil conducteur</h1><small>Comprendre → exécuter → améliorer · 13 minutes devant le public</small></header>
<div class="shell"><aside><nav aria-label="Étapes de répétition">${links}</nav><p class="private">Sur le portable : ce guide.<br>Sur le projecteur : les preuves.<br><strong>Secours démo 3 : à 2:10.</strong></p><button class="print" type="button" id="print">Imprimer le conducteur</button><p class="copy-status" aria-live="polite" id="copy-status"></p></aside><main>${articles}</main></div>
<footer>Source éditable : <a href="conducteur-demos.md">conducteur-demos.md</a> · Fonctionne hors réseau et sans serveur · Aucune commande n’est exécutée par cette page.</footer>
<script>
const sections = [...document.querySelectorAll('main section')];
function navigate() {
  const current = sections.find(s => s.id === location.hash.slice(1)) || sections[0];
  sections.forEach(s => s.classList.toggle('active', s === current));
  document.querySelectorAll('nav a').forEach(a => {
    if(a.dataset.section === current.id) a.setAttribute('aria-current', 'page');
    else a.removeAttribute('aria-current');
  });
  window.scrollTo(0, 0);
}
document.body.classList.add('interactive');
window.addEventListener('hashchange', navigate);
navigate();
document.querySelector('#print').addEventListener('click', () => window.print());
document.querySelectorAll('pre').forEach(pre => {
  const code = pre.querySelector('code');
  const button = document.createElement('button');
  button.type = 'button'; button.className = 'copy';
  button.textContent = code.classList.contains('language-text') ? 'Copier le prompt' : 'Copier les commandes';
  button.addEventListener('click', async () => {
    try {
      await navigator.clipboard.writeText(code.textContent);
      document.querySelector('#copy-status').textContent = 'Copié. Coller dans le terminal ou Copilot, selon le bloc.';
    } catch {
      const range = document.createRange(); range.selectNodeContents(code);
      const selection = window.getSelection(); selection.removeAllRanges(); selection.addRange(range);
      document.querySelector('#copy-status').textContent = 'Texte sélectionné : appuyer sur Ctrl+C.';
    }
  });
  pre.append(button);
});
</script></body></html>`
const target = path.join(root, 'handouts/conducteur-demos.html')
await fs.writeFile(target, html, 'utf8')
console.log(`Conducteur généré : ${target} (${sections.length} rubriques)`)
