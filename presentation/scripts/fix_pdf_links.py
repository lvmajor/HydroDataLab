"""Convert Slidev's local slide URLs into offline PDF page destinations."""
import re
import sys
from pathlib import Path
from urllib.parse import urlsplit
from pypdf import PdfReader, PdfWriter
from pypdf.generic import ArrayObject, NameObject

pdf = Path(sys.argv[1]).resolve()
writer = PdfWriter(clone_from=pdf)
count = 0
for page in writer.pages:
    for annotation in page.get('/Annots', []):
        item = annotation.get_object()
        action = item.get('/A', {})
        uri = str(action.get('/URI', ''))
        url = urlsplit(uri)
        match = re.fullmatch(r'/(\d+)/?', url.path)
        if url.hostname not in {'localhost', '127.0.0.1'} or not match:
            continue
        index = int(match[1]) - 1
        if not 0 <= index < len(writer.pages):
            raise ValueError(f'Invalid slide destination: {uri}')
        item[NameObject('/Dest')] = ArrayObject([writer.pages[index].indirect_reference, NameObject('/Fit')])
        del item['/A']
        count += 1
temporary = pdf.with_suffix('.links.tmp.pdf')
writer.write(temporary)
if len(PdfReader(temporary).pages) != len(writer.pages):
    raise ValueError('Page count changed while fixing PDF links')
temporary.replace(pdf)
print(f'{count} internal slide links converted to offline PDF destinations.')
