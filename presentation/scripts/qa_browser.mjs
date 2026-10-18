import { chromium } from 'playwright-chromium';
import fs from 'node:fs/promises';
const browser=await chromium.launch({headless:true,executablePath:'C:/Program Files/Google/Chrome/Application/chrome.exe'});
const page=await browser.newPage({viewport:{width:1600,height:900},deviceScaleFactor:1});
const errors=[]; const external=new Set(); const issues=[]; const titles=[];
page.on('pageerror',e=>errors.push(e.message));
page.on('request',r=>{const u=r.url();if(!u.startsWith('http://127.0.0.1:53173')&&!u.startsWith('data:')&&!u.startsWith('blob:'))external.add(u)});
await page.route('**/*',route=>{const u=route.request().url();if(!u.startsWith('http://127.0.0.1:53173')&&!u.startsWith('data:')&&!u.startsWith('blob:'))return route.abort();return route.continue()});
for(let i=1;i<=24;i++){
 await page.goto(`http://127.0.0.1:53173/${i}`,{waitUntil:'networkidle'});
 const slide=page.locator('.slidev-layout:visible').first();
 await slide.waitFor();
 titles.push(await slide.locator('h1').innerText());
 const out=await slide.evaluate(el=>{const r=el.getBoundingClientRect();const footers=[...el.querySelectorAll('.doc-link,.source')];const limit=Math.min(r.bottom-25,...footers.map(n=>n.getBoundingClientRect().top-4));return [...el.children].filter(n=>!['source','doc-link','slide-number'].some(c=>n.classList.contains(c))).map(n=>({tag:n.tagName,text:n.textContent.slice(0,70),r:n.getBoundingClientRect()})).filter(n=>n.r.bottom>limit||n.r.right>r.right+2).map(n=>({tag:n.tag,text:n.text,bottom:n.r.bottom-limit,right:n.r.right-r.right}))});
 if(out.length)issues.push({slide:i,out});
}
await page.goto('http://127.0.0.1:53173/10',{waitUntil:'networkidle'});
await page.getByRole('button',{name:'3. Contrat',exact:true}).click();
if(!await page.getByText('Ce que les colonnes veulent dire',{exact:true}).isVisible())throw new Error('Context interaction failed');
await page.getByRole('button',{name:'web/App.vue',exact:true}).click();
if(!await page.locator('.instruction-lanes>div').nth(2).getByText('✓ Motif correspondant',{exact:true}).isVisible())throw new Error('Vue scope failed');
if(!await page.locator('.instruction-lanes>div').nth(1).getByText('— Motif non correspondant',{exact:true}).isVisible())throw new Error('Python scope failed');
await page.emulateMedia({reducedMotion:'reduce'});
await page.getByRole('button',{name:'etl/transform.py',exact:true}).click();
if(await page.locator('.composition-arrows path').first().evaluate(el=>parseFloat(getComputedStyle(el).animationDuration))!==0)throw new Error('Reduced motion failed');
await page.screenshot({path:'exports/qa/context-interactive.png'});
await page.goto('http://127.0.0.1:53173/13',{waitUntil:'networkidle'});
const expected={explain:[.00055,.00055,.011,.011,.022],transform:[.003,.003,.06,.06,.12],complex:[.01,.01,.2,.2,.4],debug:[.01,.01,.2,.2,.4]};
for(const [task,values] of Object.entries(expected)){
 await page.getByLabel('Type de tâche',{exact:true}).selectOption(task);
 const actual=await page.locator('.cost-value').allTextContents();
 for(let i=0;i<5;i++){const n=parseFloat(actual[i].replace(',','.'));if(Math.abs(n-values[i])>.000051)throw new Error(`Cost ${task}/${i}: ${n}`)}
}
await page.getByRole('button',{name:'Ajouter une tentative',exact:true}).click();
if(parseFloat((await page.locator('[data-model=opus] .cost-value').innerText()).replace(',','.'))!==.8)throw new Error('Retries failed');
await page.getByLabel('Type de tâche',{exact:true}).selectOption('transform');
await page.screenshot({path:'exports/qa/model-interactive.png'});
for(const i of [7,8,9,16,19,21]){await page.goto(`http://127.0.0.1:53173/${i}`,{waitUntil:'networkidle'});await page.screenshot({path:`exports/qa/revision-${i}.png`})}
await page.goto('http://127.0.0.1:53173/8',{waitUntil:'networkidle'});
if(!await page.getByRole('link',{name:'Microsoft · format et activation',exact:true}).isVisible())throw new Error('Official format reference missing');
await page.goto('http://127.0.0.1:53173/presenter/17',{waitUntil:'networkidle'});
if(!(await page.locator('body').innerText()).includes('solution préparée'))throw new Error('Presenter notes missing');
await fs.writeFile('exports/qa/browser-report.json',JSON.stringify({slideCount:titles.length,titles,issues,errors,external:[...external],interactions:'passed',offline:'external resources blocked throughout'},null,2));
await browser.close();
console.log(JSON.stringify({slideCount:titles.length,issues,errors,external:[...external],interactions:'passed'}));
if(issues.length||errors.length||external.size)process.exitCode=1;
