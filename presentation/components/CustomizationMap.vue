<template>
  <div class="customization-map">
    <div class="customization-entry"><b>Overview</b> : repères. <b>Customize Your Agent</b> : préparer des fichiers à relire.</div>
    <div class="customization-groups">
      <section>
        <h2>Guider le travail</h2>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Instructions</a> : règles du projet.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/agent-skills">Skills</a> : procédures réutilisables.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/custom-agents">Agents</a> : rôles spécialisés.</p>
      </section>
      <section>
        <h2>Donner des moyens d’agir</h2>
        <p><a href="https://code.visualstudio.com/docs/agents/run/tools">Tools</a> : outils disponibles.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/mcp-servers">MCP Servers</a> : connexions externes.</p>
      </section>
      <section>
        <h2>Automatiser et partager</h2>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/hooks">Hooks</a> : actions sur événement.</p>
        <p><a href="https://code.visualstudio.com/docs/agent-customization/agent-plugins">Plugins</a> : paquets à partager.</p>
      </section>
    </div>
    <aside class="mcp-locations" aria-label="Emplacements MCP selon le harnais">
      <h2>Repères MCP pour VS Code 1.142</h2>
      <p class="mcp-release">Transition récente : Copilot devient progressivement le harnais par défaut.</p>
      <div class="mcp-scopes">
        <section>
          <h3>Copilot <span>format <code>mcpServers</code></span></h3>
          <p>Personnel, par défaut :<br><code>%USERPROFILE%\.copilot\<wbr>mcp-config.json</code></p>
          <p>Projet : <code>.mcp.json</code> à la racine*</p>
        </section>
        <section>
          <h3>Local <span>format <code>servers</code></span></h3>
          <p>Personnel : <code>mcp.json</code> du profil VS Code</p>
          <p>Projet : <code>.vscode/mcp.json</code></p>
          <p class="mcp-compatibility">Emplacements conservés pour compatibilité.</p>
        </section>
      </div>
      <p class="mcp-caveat">* Destination encore expérimentale dans l’assistant d’ajout. Migration détaillée dans l’aide-mémoire.</p>
    </aside>
  </div>
</template>

<style>
.customization-entry{font-size:17px;line-height:1.35;margin-bottom:12px}
.customization-groups{display:grid;grid-template-columns:1fr 1fr 1fr;gap:18px}
.customization-groups section{border-left:4px solid var(--water);padding-left:12px}
.customization-groups section:nth-of-type(2){border-color:#497c99}
.customization-groups section:nth-of-type(3){border-color:var(--gold)}
.slidev-layout .customization-groups h2{font:600 20px/1.2 'IBM Plex Sans',sans-serif;color:var(--deep);margin:0 0 6px}
.slidev-layout .customization-groups p{font-size:17px;line-height:1.4;margin:0}
.customization-groups a{font-weight:600;text-decoration:underline;text-underline-offset:3px}
.mcp-locations{margin-top:16px;border:1px solid #9ab6c2;border-left:5px solid var(--water);background:var(--ice);padding:12px 16px}
.slidev-layout .mcp-locations h2{font:600 22px/1.2 'IBM Plex Sans',sans-serif;margin:0;color:var(--deep)}
.slidev-layout .mcp-locations p{font-size:17px;line-height:1.35;margin:5px 0 0}
.mcp-scopes{display:grid;grid-template-columns:1fr 1fr;gap:24px;margin-top:9px}
.slidev-layout .mcp-scopes h3{font:600 19px/1.25 'IBM Plex Sans',sans-serif;margin:0 0 5px}
.mcp-scopes h3 span{font-size:16px;font-weight:400;margin-left:8px}
.slidev-layout .mcp-locations code{font-size:16px;background:transparent;padding:0}
.slidev-layout .mcp-locations .mcp-caveat{font-size:15px;margin-top:10px;color:var(--muted)}
.slidev-layout .mcp-locations .mcp-compatibility{font-size:16px;color:var(--muted)}
</style>
