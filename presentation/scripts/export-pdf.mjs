import fs from 'node:fs';
import path from 'node:path';
import {fileURLToPath} from 'node:url';
import {spawnSync} from 'node:child_process';
import {chromium} from 'playwright-chromium';
const root=path.resolve(path.dirname(fileURLToPath(import.meta.url)),'..');
const candidates=[process.env.SLIDEV_BROWSER_PATH,process.env.PROGRAMFILES&&path.join(process.env.PROGRAMFILES,'Google/Chrome/Application/chrome.exe'),process.env['PROGRAMFILES(X86)']&&path.join(process.env['PROGRAMFILES(X86)'],'Microsoft/Edge/Application/msedge.exe'),process.env.PROGRAMFILES&&path.join(process.env.PROGRAMFILES,'Microsoft/Edge/Application/msedge.exe'),chromium.executablePath()].filter(Boolean);
const browser=candidates.find(p=>fs.existsSync(p));
if(!browser){console.error('Aucun navigateur trouvé. Définir SLIDEV_BROWSER_PATH vers Chrome/Chromium/Edge, ou installer Chromium Playwright avant la séance.');process.exit(1)}
const cli=path.join(root,'node_modules/@slidev/cli/bin/slidev.mjs');
console.log('Export PDF avec '+browser);
const run=spawnSync(process.execPath,[cli,'export','slides.md','--output','exports/formation-hydrodatalab.pdf','--executable-path',browser,...process.argv.slice(2)],{cwd:root,stdio:'inherit',windowsHide:true});
if(run.error){console.error(run.error.message);process.exit(1)}process.exit(run.status??1);
