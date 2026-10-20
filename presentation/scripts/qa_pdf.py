from pathlib import Path
from PIL import Image, ImageOps, ImageDraw
from pypdf import PdfReader
root=Path(__file__).resolve().parents[1]
pdf=root/'exports/formation-hydrodatalab.pdf'
reader=PdfReader(pdf)
assert len(reader.pages)==31, len(reader.pages)
for i,p in enumerate(reader.pages,1):
    text=p.extract_text() or ''
    assert len(text)>70, (i,text)
    for a in p.get('/Annots', []):
        uri=str(a.get_object().get('/A', {}).get('/URI', ''))
        assert not uri.startswith(('http://localhost:', 'http://127.0.0.1:')), (i,uri)
expected_links={2:'agents/run/security',9:'custom-instructions#instructions-file-format',13:'agent-skills#how-copilot-uses-skills',14:'migrate-customizations',23:'microsoft/playwright-cli',17:'models-and-pricing',19:'early-access/copilot/hydrafusion',21:'spec-kit',25:'changelog/label/copilot',28:'use-canvases-with-the-copilot-harness'}
for page,fragment in expected_links.items():
    uris=[str(a.get_object().get('/A',{}).get('/URI','')) for a in reader.pages[page-1].get('/Annots',[])]
    assert any(fragment in uri for uri in uris), (page,fragment)
effort_text=reader.pages[17].extract_text()
for term in ['Thinking effort','Low','High','Max','Artificial Analysis','0,89','1,55','14,19','74 %','logarithmique']:
    assert term in effort_text, term
effort_links=[str(a.get_object().get('/A',{}).get('/URI','')) for a in reader.pages[17].get('/Annots',[])]
for fragment in ['configure-thinking-effort','artificialanalysis.ai/agents/coding-agents','methodology/coding-agents-benchmarking']:
    assert any(fragment in uri for uri in effort_links), fragment
assert '66,67' in reader.pages[7].extract_text()
assert 'Spec-Driven Development' in reader.pages[20].extract_text()
context_text=reader.pages[9].extract_text()+reader.pages[10].extract_text()
for term in ['Context scope','Tiered-context architecture','Task scoped','On demand','Règles communes','Progressive loading','applyTo','SKILL.md','Nom + description']:
    assert term in context_text, term
model_text=reader.pages[16].extract_text()
for term in ['GPT-6 Luna','Claude Haiku 5.5','Claude Sonnet 5.5','GPT-6.1 Sol','Claude Opus 5.5','Simple','Délimitée','Complexe','Diagnostic difficile','Coûts simulés']:
    assert term in model_text, term
for term in ['Overview','Customize Your Agent','Plugins','MCP Servers','Skills','Instructions','Agents','Hooks','Tools']:
    assert term in reader.pages[11].extract_text(), term
for term in ['SKILL.md','disable-model-invocation','Le modèle choisit','Le harnais contrôle']:
    assert term in reader.pages[12].extract_text(), term
for page,target in [(8,26),(12,26),(25,28),(26,8),(26,12),(28,25)]:
    dests=[a.get_object().get('/Dest') for a in reader.pages[page-1].get('/Annots',[])]
    assert any(d and d[0]==reader.pages[target-1].indirect_reference for d in dests), (page,target)
assert 'Votre terrain de développement' in reader.pages[1].extract_text()
assert 'en exploration' in reader.pages[22].extract_text()
assert 'Chez Hydro' in reader.pages[24].extract_text()
for group in range((len(reader.pages)+11)//12):
    canvas=Image.new('RGB',(1600,1280),'#dce8ed')
    draw=ImageDraw.Draw(canvas)
    for slot in range(12):
        index=group*12+slot+1
        if index>len(reader.pages):
            break
        im=Image.open(root/f'exports/qa/page-{index:02}.png').convert('RGB')
        im.thumbnail((520,280))
        x=(slot%3)*533
        y=(slot//3)*320
        canvas.paste(im,(x,y+18))
        draw.text((x+8,y+2),str(index),fill='#123d51')
    canvas.save(root/f'exports/qa/contact-{group+1}.jpg',quality=90)
print(f'{len(reader.pages)} pages; textes présents; 3 planches de contrôle créées.')

