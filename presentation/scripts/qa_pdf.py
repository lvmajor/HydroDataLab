from pathlib import Path
from PIL import Image, ImageOps, ImageDraw
from pypdf import PdfReader
root=Path(__file__).resolve().parents[1]
pdf=root/'exports/formation-hydrodatalab.pdf'
reader=PdfReader(pdf)
assert len(reader.pages)==24, len(reader.pages)
for i,p in enumerate(reader.pages,1):
    text=p.extract_text() or ''
    assert len(text)>70, (i,text)
    for a in p.get('/Annots', []):
        uri=str(a.get_object().get('/A', {}).get('/URI', ''))
        assert not uri.startswith(('http://localhost:', 'http://127.0.0.1:')), (i,uri)
expected_links={8:'custom-instructions#instructions-file-format',13:'models-and-pricing',14:'early-access/copilot/hydrafusion',16:'spec-kit',21:'use-canvases-with-the-copilot-harness'}
for page,fragment in expected_links.items():
    uris=[str(a.get_object().get('/A',{}).get('/URI','')) for a in reader.pages[page-1].get('/Annots',[])]
    assert any(fragment in uri for uri in uris), (page,fragment)
assert '66,67' in reader.pages[6].extract_text()
assert 'Spec-Driven Development' in reader.pages[15].extract_text()
context_text=reader.pages[9].extract_text()
for term in ['Project instructions','File-specific instructions','Reference documents','Task prompt','Tool results','Model context']:
    assert term in context_text, term
model_text=reader.pages[12].extract_text()
for term in ['GPT-6 Luna','Claude Haiku 5.5','Claude Sonnet 5.5','GPT-6.1 Sol','Claude Opus 5.5','Simple','Délimitée','Complexe','Diagnostic difficile','Coûts simulés']:
    assert term in model_text, term
for page,target in [(7,19),(18,21),(19,7),(21,18)]:
    dests=[a.get_object().get('/Dest') for a in reader.pages[page-1].get('/Annots',[])]
    assert any(d and d[0]==reader.pages[target-1].indirect_reference for d in dests), (page,target)
for group in range(2):
    canvas=Image.new('RGB',(1600,1280),'#dce8ed')
    draw=ImageDraw.Draw(canvas)
    for slot in range(12):
        index=group*12+slot+1
        im=Image.open(root/f'exports/qa/page-{index:02}.png').convert('RGB')
        im.thumbnail((520,280))
        x=(slot%3)*533
        y=(slot//3)*320
        canvas.paste(im,(x,y+18))
        draw.text((x+8,y+2),str(index),fill='#123d51')
    canvas.save(root/f'exports/qa/contact-{group+1}.jpg',quality=90)
print('24 pages; textes présents; 2 planches de contrôle créées.')

