namespace SIGA.Application.Interfaces.Services;

public interface IGeneradorPdfService
{
    Task<byte[]> GenerarPdfDesdeHtmlAsync(string htmlContent, string? headerHtml = null, string? footerHtml = null);
    Task<string> RenderizarPlantillaAsync(string templateHtml, object model);
}
