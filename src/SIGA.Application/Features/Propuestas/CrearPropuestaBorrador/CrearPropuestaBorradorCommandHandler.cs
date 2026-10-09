using SIGA.Application.Common.Dispatcher.Interfaces;
using SIGA.Domain.Entities.Core;
using SIGA.Persistence;

namespace SIGA.Application.Features.Propuestas.CrearPropuestaBorrador;

public class CrearPropuestaBorradorCommandHandler
    : IUseCaseHandler<CrearPropuestaBorradorCommand, int>
{
    private readonly ApplicationDbContext _dbContext;

    public CrearPropuestaBorradorCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(
        CrearPropuestaBorradorCommand command,
        CancellationToken cancellationToken
    )
    {
        var propuesta = Propuesta.Crear(
            unidadId: command.UnidadId,
            tipoPropuestaId: command.TipoPropuestaId,
            titulo: command.Titulo,
            descripcion: null,
            cantidadHoras: command.CantidadHoras,
            usuarioId: command.UsuarioId,
            emailEncargado: command.EmailEncargado,
            planEstudioPdf: command.PlanEstudioPdf,
            lugarRealizacion: command.LugarRealizacion,
            contactoInfo: command.ContactoInfo,
            pagosInfo: command.PagosInfo,
            webVisible: command.WebVisible
        );

        var version = PropuestaVersion.Crear(
            propuestaId: 0,
            periodoLectivoId: command.PeriodoLectivoId,
            modalidadId: command.ModalidadId,
            fechaInicio: command.FechaInicio,
            fechaFin: command.FechaFin,
            maximoAlumnos: command.MaximoAlumnos,
            importeBase: command.ImporteBase,
            cuotas: command.Cuotas,
            conceptoPago: command.ConceptoPago,
            permiteInscripcionesWeb: command.PermiteInscripcionesWeb
        );

        propuesta.Versiones.Add(version);

        await _dbContext.Propuestas.AddAsync(propuesta, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return propuesta.Id;
    }
}
