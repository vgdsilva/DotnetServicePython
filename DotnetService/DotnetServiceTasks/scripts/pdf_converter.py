from weasyprint import HTML
import sys

def html_para_pdf(caminho_html, caminho_pdf):
    try:
        HTML(filename=caminho_html).write_pdf(caminho_pdf)
    except Exception as e:
        print(f"Erro: {e}", file=sys.stderr)
        sys.exit(1)

if __name__ == "__main__":
    caminho_html = sys.argv[1]
    caminho_pdf = sys.argv[2]
    html_para_pdf(caminho_html, caminho_pdf)