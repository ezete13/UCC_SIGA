using PuppeteerSharp;
using PuppeteerSharp.Media;
using Scriban;
using SIGA.Application.Interfaces.Services;

namespace SIGA.Infrastructure.Services;

public class GeneradorPdfService : IGeneradorPdfService
{
    private bool _navegadorDescargado = false;

    private async Task AsegurarNavegadorAsync()
    {
        if (!_navegadorDescargado)
        {
            var fetcher = new BrowserFetcher();
            await fetcher.DownloadAsync();
            _navegadorDescargado = true;
        }
    }

    public async Task<byte[]> GenerarPdfDesdeHtmlAsync(string htmlContent, string? headerHtml = null, string? footerHtml = null)
    {
        await AsegurarNavegadorAsync();

        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
        });

        using var page = await browser.NewPageAsync();
        await page.SetContentAsync(htmlContent);

        var pdfOptions = new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true,
            MarginOptions = new MarginOptions
            {
                Top = "20mm",
                Bottom = "20mm",
                Left = "20mm",
                Right = "20mm"
            }
        };

        if (!string.IsNullOrEmpty(headerHtml) || !string.IsNullOrEmpty(footerHtml))
        {
            pdfOptions.DisplayHeaderFooter = true;
            pdfOptions.HeaderTemplate = headerHtml ?? "<span></span>";
            pdfOptions.FooterTemplate = footerHtml ?? "<span></span>";
        }

        var pdfBytes = await page.PdfDataAsync(pdfOptions);

        await browser.CloseAsync();

        return pdfBytes;
    }

    public async Task<string> RenderizarPlantillaAsync(string templateHtml, object model)
    {
        var template = Template.Parse(templateHtml);
        return await template.RenderAsync(model);
    }
}
