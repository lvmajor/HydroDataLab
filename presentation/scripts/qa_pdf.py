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

