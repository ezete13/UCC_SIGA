using Microsoft.Extensions.DependencyInjection;
using SIGA.Application.Common.Dispatcher;
using SIGA.Application.Common.Dispatcher.Interfaces;
using SIGA.Application.Features.Modalidades.CrearModalidad;
using SIGA.Application.Features.Modalidades.ObtenerReporteCsv;
using SIGA.Application.Features.Preinscripciones.CrearPreinscripcionPendiente;
using SIGA.Application.Features.Propuestas.CrearPropuestaBorrador;

using SIGA.Domain.Entities.Catalog.Dynamic;

namespace SIGA.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrar Dispatcher
        services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

        // Propuestas
        services.AddScoped<
            IUseCaseHandler<CrearPropuestaBorradorCommand, int>,
            CrearPropuestaBorradorCommandHandler
        >();

        // Preinscripciones
        services.AddScoped<
            IUseCaseHandler<CrearPreinscripcionPendienteCommand, Guid>,
            CrearPreinscripcionPendienteCommandHandler
        >();

        services.AddScoped<
            IUseCaseHandler<CrearModalidadCommand, Modalidad>,
            CrearModalidadCommandHandler
        >();
        services.AddScoped<
            IUseCaseHandler<ObtenerReporteCsvQuery, MemoryStream>,
            ObtenerReporteQueryHandler
        >();

        return services;
    }
}
