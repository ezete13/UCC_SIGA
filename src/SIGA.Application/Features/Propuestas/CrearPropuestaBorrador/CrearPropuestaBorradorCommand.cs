using SIGA.Application.Common.Dispatcher.Interfaces;

namespace SIGA.Application.Features.Propuestas.CrearPropuestaBorrador;

public record CrearPropuestaBorradorCommand(
    int UnidadId,
    int ModalidadId,
    int TipoPropuestaId,
    int PeriodoLectivoId,
    int EstadoPropuestaId,
    int UsuarioId,
    string Titulo,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    int MaximoAlumnos,
    int CantidadHoras,
    decimal? ImporteBase,
    int? Cuotas,
    string? ConceptoPago,
    string? EmailEncargado,
    string? PlanEstudioPdf,
    string? LugarRealizacion,
    string? ContactoInfo,
    string? PagosInfo,
    bool WebVisible,
    bool PermiteInscripcionesWeb
) : IUseCase<int>;
