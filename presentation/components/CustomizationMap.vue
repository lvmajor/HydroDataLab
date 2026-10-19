<template>
  <div class="customization-map">
    <figure class="customization-capture">
      <div class="customization-menu-crop"><img src="/vscode-customizations-overview.png" alt="Menu réel Agent Customizations : Overview, Plugins, MCP Servers, Skills, Instructions, Agents, Hooks, Tools." /></div>
      <figcaption>Capture réelle · Copilot dans VS Code</figcaption>
    </figure>
    <div class="customization-groups">
      <div class="customization-entry"><b>Overview</b> : retrouver les personnalisations.<br><b>Customize Your Agent</b> : préparer des fichiers à relire.</div>
      <section>
        <h2>Guider le travail</h2>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Instructions</a> · règles : préserver les absences.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/agent-skills">Skills</a> · procédures : valider l’ETL.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/custom-agents">Agents</a> · rôles : réviseur BI.</p>
      </section>
      <section>
        <h2>Donner des moyens d’agir</h2>
        <p><a href="https://code.visualstudio.com/docs/agents/run/tools">Tools</a> · outils disponibles : lire, lancer un test.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/mcp-servers">MCP Servers</a> · connexions : accéder à SQL.</p>
      </section>
      <section>
        <h2>Automatiser et partager</h2>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/hooks">Hooks</a> · événements : journaliser un appel.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/agent-plugins">Plugins</a> · paquets : distribuer skill + MCP.</p>
      </section>
    </div>
  </div>
</template>

<style>
.customization-map{display:grid;grid-template-columns:300px 1fr;gap:28px;align-items:start}
.customization-capture{margin:0}
.customization-menu-crop{position:relative;width:300px;height:344px;overflow:hidden;background:#181818;border:1px solid #526776}
.customization-menu-crop img{position:absolute;width:1900px;max-width:none;left:-17px;top:-62px}
.customization-capture figcaption{font-size:15px;line-height:1.4;margin-top:10px;color:var(--muted)}
.customization-entry{font-size:17px;line-height:1.4;margin-bottom:12px}
.customization-groups section{border-left:4px solid var(--water);padding-left:14px;margin-bottom:12px}
.customization-groups section:nth-of-type(2){border-color:#497c99}
.customization-groups section:nth-of-type(3){border-color:var(--gold)}
.slidev-layout .customization-groups h2{font:600 22px/1.2 'IBM Plex Sans',sans-serif;color:var(--deep);margin:0 0 5px}
.slidev-layout .customization-groups p{font-size:18px;line-height:1.45;margin:0}
.customization-groups a{font-weight:600;text-decoration:underline;text-underline-offset:3px}
</style>
