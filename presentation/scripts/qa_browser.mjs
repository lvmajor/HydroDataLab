import { chromium } from 'playwright-chromium';
import fs from 'node:fs/promises';
const browser=await chromium.launch({headless:true,executablePath:'C:/Program Files/Google/Chrome/Application/chrome.exe'});
const page=await browser.newPage({viewport:{width:1600,height:900},deviceScaleFactor:1});
const errors=[]; const external=new Set(); const issues=[]; const titles=[];
page.on('pageerror',e=>errors.push(e.message));
page.on('request',r=>{const u=r.url();if(!u.startsWith('http://127.0.0.1:53173')&&!u.startsWith('data:')&&!u.startsWith('blob:'))external.add(u)});
await page.route('**/*',route=>{const u=route.request().url();if(!u.startsWith('http://127.0.0.1:53173')&&!u.startsWith('data:')&&!u.startsWith('blob:'))return route.abort();return route.continue()});
for(let i=1;i<=28;i++){
 await page.goto(`http://127.0.0.1:53173/${i}`,{waitUntil:'networkidle'});
 const slide=page.locator('.slidev-layout:visible').first();
 await slide.waitFor();
 titles.push(await slide.locator('h1').innerText());
 const out=await slide.evaluate(el=>{const r=el.getBoundingClientRect();const footers=[...el.querySelectorAll('.doc-link,.source')];const limit=Math.min(r.bottom-25,...footers.map(n=>n.getBoundingClientRect().top-4));return [...el.children].filter(n=>!['source','doc-link','slide-number'].some(c=>n.classList.contains(c))).map(n=>({tag:n.tagName,text:n.textContent.slice(0,70),r:n.getBoundingClientRect()})).filter(n=>n.r.bottom>limit||n.r.right>r.right+2).map(n=>({tag:n.tag,text:n.text,bottom:n.r.bottom-limit,right:n.r.right-r.right}))});
 if(out.length)issues.push({slide:i,out});
}
if(titles.length!==28||titles[1]!=='Votre terrain de développement')throw new Error('Opening slide order');
await page.goto('http://127.0.0.1:53173/13',{waitUntil:'networkidle'});
for(const term of ['Project instructions','File-specific instructions','Reference documents','Task prompt','Tool results']){
 if(!await page.getByRole('button',{name:new RegExp(term)}).isVisible())throw new Error('Missing bilingual layer: '+term);
 await page.getByRole('button',{name:new RegExp(term)}).click();
 const overflow=await page.locator('.context-map').evaluate(el=>{const footer=el.closest('.slidev-layout').querySelector('.doc-link').getBoundingClientRect();return el.getBoundingClientRect().bottom>footer.top-4});
 if(overflow)issues.push({slide:13,layer:term,problem:'Context overlaps references'});
}
await page.getByRole('button',{name:/Reference documents/}).click();
if(!(await page.locator('.context-preview').innerText()).includes('Une série horaire'))throw new Error('Reference layer failed');
await page.emulateMedia({reducedMotion:'reduce'});
await page.getByRole('button',{name:/Tool results/}).click();
if(!(await page.locator('.context-preview').innerText()).includes('95,83'))throw new Error('Tool results failed');
if(await page.locator('.context-preview').evaluate(el=>parseFloat(getComputedStyle(el).animationDuration))!==0)throw new Error('Reduced motion failed');
await page.screenshot({path:'exports/qa/context-interactive.png'});
await page.goto('http://127.0.0.1:53173/16',{waitUntil:'networkidle'});
const expected={explain:[.00055,.00055,.011,.011,.022],transform:[.003,.003,.06,.06,.12],complex:[.01,.01,.2,.2,.4],debug:[.01,.01,.2,.2,.4]};
for(const [task,values] of Object.entries(expected)){
 const actual=await page.locator(`[data-task=${task}] .cost-value`).allTextContents();
 if(actual.length!==5)throw new Error('Matrix should show all five models per task');
 for(let i=0;i<5;i++){const n=parseFloat(actual[i].replace(',','.'));if(Math.abs(n-values[i])>.000051)throw new Error(`Cost ${task}/${i}: ${n}`)}
}
await page.getByRole('button',{name:'Ajouter une tentative',exact:true}).click();
if(parseFloat((await page.locator('[data-model=opus] [data-task=debug] .cost-value').innerText()).replace(',','.'))!==.8)throw new Error('Retries failed');
if(await page.locator('.cost-value:visible').count()!==20)throw new Error('Comparison must remain simultaneous');
if(await page.locator('.cost-cell.recommended').count()!==5)throw new Error('Expected five recommendations across four tasks');
const headerOverflow=await page.locator('.task-header').evaluateAll(headers=>headers.filter(h=>[...h.children].some(c=>{const r=c.getBoundingClientRect(),p=h.getBoundingClientRect();return r.bottom>p.bottom-3||r.right>p.right-3})).map(h=>h.textContent));
if(headerOverflow.length)issues.push({slide:16,headers:headerOverflow,problem:'Header content overflow'});
await page.getByRole('button',{name:'Réduire les tentatives',exact:true}).click();
await page.screenshot({path:'exports/qa/model-interactive.png'});
for(const i of [2,8,9,10,11,12,19,21,22,23,25]){await page.goto(`http://127.0.0.1:53173/${i}`,{waitUntil:'networkidle'});await page.screenshot({path:`exports/qa/revision-${i}.png`})}
await page.goto('http://127.0.0.1:53173/9',{waitUntil:'networkidle'});
if(!await page.getByRole('link',{name:'Microsoft · format et activation',exact:true}).isVisible())throw new Error('Official format reference missing');
await page.goto('http://127.0.0.1:53173/presenter/20',{waitUntil:'networkidle'});
if(!(await page.locator('body').innerText()).includes('solution préparée'))throw new Error('Presenter notes missing');
await page.goto('http://127.0.0.1:53173/22',{waitUntil:'networkidle'});
if(await page.locator('.resource-columns a').count()!==7)throw new Error('Expected seven public resources');
if(await page.locator('.internal-resources a').count()!==0)throw new Error('No HQ URL supplied: placeholders must not be links');
if(await page.getByText('Lien interne à ajouter',{exact:true}).count()!==3)throw new Error('Expected three editable HQ slots');
if(titles[10]!=='Une base « vanille », des ajouts ciblés'||titles[11]!=='Des skills à choisir selon votre tâche'||titles[21]!=='Pour continuer et rester à jour')throw new Error('New slide order');
await fs.writeFile('exports/qa/browser-report.json',JSON.stringify({slideCount:titles.length,titles,issues,errors,external:[...external],interactions:'passed',offline:'external resources blocked throughout'},null,2));
await browser.close();
console.log(JSON.stringify({slideCount:titles.length,issues,errors,external:[...external],interactions:'passed'}));
if(issues.length||errors.length||external.size)process.exitCode=1;
