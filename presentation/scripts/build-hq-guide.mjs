import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'
import MarkdownIt from 'markdown-it'

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../..')
const source = await fs.readFile(path.join(root, 'packaging/hq/LIRE-MOI-HQ.md'), 'utf8')
const md = new MarkdownIt({html:false,linkify:true})
const slug = text => text.normalize('NFD').replace(/[\u0300-\u036f]/g,'').toLowerCase().replace(/[^a-z0-9]+/g,'-').replace(/^-|-$/g,'')
const headings=[]
md.renderer.rules.heading_open = (tokens,i,options,env,self) => {
  if(tokens[i].tag==='h2') {
    const label=tokens[i+1].content, id=slug(label)
    tokens[i].attrSet('id',id);headings.push({label,id})
  }
  return self.renderToken(tokens,i,options)
}
md.renderer.rules.table_open=()=>'<div class="table-scroll" tabindex="0" role="region" aria-label="Tableau à faire défiler"><table>'
md.renderer.rules.table_close=()=>'</table></div>'
const content=md.render(source)
const nav=headings.map(h=>`<a href="#${h.id}">${md.utils.escapeHtml(h.label)}</a>`).join('')
const html=`<!doctype html><html lang="fr"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>HydroDataLab · Préparer le portable HQ</title>
<style>
:root{color-scheme:light;--ink:#123d51;--teal:#006b83;--line:#bdd0d8}*{box-sizing:border-box}body{margin:0;background:#f4f8fa;color:var(--ink);font:18px/1.55 "Segoe UI",Aptos,system-ui,sans-serif}a{color:var(--teal);text-underline-offset:3px}a,button,summary{overflow-wrap:anywhere}a:focus-visible,button:focus-visible,[tabindex]:focus-visible{outline:3px solid #a67800;outline-offset:4px}.shell{max-width:1360px;margin:auto;padding:32px;display:grid;grid-template-columns:260px minmax(0,1fr);gap:40px}aside{position:sticky;top:24px;align-self:start;max-height:94vh;overflow:auto;font-size:16px}nav{display:grid;gap:8px}nav a{padding:6px 0;text-decoration:none}nav a:hover{text-decoration:underline}main{min-width:0;background:white;padding:32px;border:1px solid var(--line);border-radius:8px}h1{font-size:36px;line-height:1.2;margin:0 0 24px}h2{font-size:26px;line-height:1.3;margin:48px 0 20px;padding-top:22px;border-top:3px solid #dca92c;scroll-margin-top:18px}p,li{max-width:72ch}li{margin:8px 0}code{font: .95em Consolas,monospace;overflow-wrap:anywhere}pre{position:relative;margin:24px 0;background:#092f40;color:#f1f8fb;border-radius:6px;padding:50px 20px 20px;white-space:pre-wrap;overflow-wrap:anywhere;font-size:16px;line-height:1.6}pre code{font-size:inherit}button{font:inherit;cursor:pointer;background:white;color:var(--ink);border:1px solid var(--line);border-radius:5px;padding:8px 12px}.copy{position:absolute;right:10px;top:10px;font-size:16px;padding:4px 10px}.status{font-size:16px;min-height:3em;margin-top:14px}.table-scroll{overflow-x:auto;margin:24px 0}table{width:100%;border-collapse:collapse;font-size:16px;line-height:1.5;min-width:540px}td,th{padding:12px;text-align:left;vertical-align:top;border-bottom:1px solid var(--line)}th{background:#eaf3f6}.hint{font-size:16px;color:#35566a}@media(max-width:900px){.shell{display:block;padding:20px}aside{position:static;max-height:none;margin-bottom:24px}nav{grid-template-columns:repeat(2,minmax(0,1fr));gap:8px 20px}main{padding:20px}h1{font-size:30px}}@media(max-width:480px){.shell{padding:12px}main{padding:16px}nav{grid-template-columns:1fr}h1{font-size:28px}h2{font-size:24px}pre{padding:52px 12px 12px}}@media print{body{background:white;color:#111;font-size:11pt}.shell{display:block;padding:0}aside,.copy{display:none}main{border:0;padding:0}h1{font-size:24pt}h2{font-size:17pt;break-after:avoid}pre{padding:14px;background:#f4f4f4;color:#111;font-size:10pt;break-inside:avoid}.table-scroll{overflow:visible}table{min-width:0;font-size:10pt}a{color:#111}@page{size:A4;margin:16mm}}
</style></head><body><div class="shell"><aside><nav aria-label="Étapes de préparation">${nav}</nav><p class="hint">À utiliser avant la séance.<br>Aucune commande ne se lance automatiquement.</p><button id="print" type="button">Imprimer le guide</button><p class="status" aria-live="polite" id="status"></p></aside><main>${content}</main></div>
<script>
document.querySelector('#print').addEventListener('click',()=>window.print());
document.querySelectorAll('pre').forEach(pre=>{const code=pre.querySelector('code'),button=document.createElement('button');button.type='button';button.className='copy';button.textContent='Copier';button.addEventListener('click',async()=>{try{await navigator.clipboard.writeText(code.textContent);document.querySelector('#status').textContent='Copié. Vérifier les chemins avant de coller dans PowerShell.'}catch{const range=document.createRange();range.selectNodeContents(code);const selection=window.getSelection();selection.removeAllRanges();selection.addRange(range);document.querySelector('#status').textContent='Texte sélectionné : Ctrl+C pour copier.'}});pre.append(button)});
</script></body></html>`
await fs.writeFile(path.join(root,'packaging/hq/LIRE-MOI-HQ.html'),html,'utf8')
console.log(`Guide HQ : ${headings.length} sections; HTML autonome généré.`)
