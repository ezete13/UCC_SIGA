using SIGA.Domain.Entities.Catalog.Static;
using SIGA.Domain.Enums;
using SIGA.Domain.Exceptions;

namespace SIGA.Domain.Entities.Core;

public class Inscripcion
{
    public int Id { get; set; }

    public required Guid Uuid { get; set; } = Guid.NewGuid();

    public required int AlumnoId { get; set; }

    public required int PropuestaVersionId { get; set; }

    public required int InscripcionEstadoId { get; set; }

    public int? PreinscripcionId { get; set; }

    public required DateTime FechaInscripcion { get; set; }

    public bool EsBaja { get; set; } = false;

    public DateTime? FechaBaja { get; set; }

    public string? MotivoBaja { get; set; }

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public DateTime? ActualizadoEn { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;

    public virtual PropuestaVersion PropuestaVersion { get; set; } = null!;

    public virtual InscripcionEstado InscripcionEstado { get; set; } = null!;

    public virtual Preinscripcion? Preinscripcion { get; set; }

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    // Método de Fábrica: Asegura que toda inscripción nazca de una preinscripción aprobada
    public static Inscripcion CrearDesdePreinscripcion(Preinscripcion preinscripcion, int alumnoId)
    {
        // Regla de Negocio: No existe inscripción sin preinscripción aprobada
        if (preinscripcion.EstadoPreinscripcionId != (int)EstadoPreinscripcionEnum.Aprobada)
            throw new DomainException(
                "No se puede generar una inscripción de una preinscripción no aprobada."
            );

        return new Inscripcion
        {
            Uuid = Guid.NewGuid(),
            AlumnoId = alumnoId,
            PropuestaVersionId = preinscripcion.PropuestaVersionId,
            PreinscripcionId = preinscripcion.Id,
            InscripcionEstadoId = (int)EstadoInscripcionEnum.Activa,
            FechaInscripcion = DateTime.UtcNow,
            CreadoEn = DateTime.UtcNow,
        };
    }

    // Regla de Negocio: La vigencia depende exclusivamente del estado 'Activa'
    public bool EsVigente()
    {
        return InscripcionEstadoId == (int)EstadoInscripcionEnum.Activa;
    }

    // Regla de Negocio: Finalización Irreversible
    public void Finalizar()
    {
        if (InscripcionEstadoId == (int)EstadoInscripcionEnum.Baja)
            throw new DomainException(
                "No se puede finalizar una inscripción que ya tiene la baja."
            );

        this.InscripcionEstadoId = (int)EstadoInscripcionEnum.Finalizada;
        this.ActualizadoEn = DateTime.UtcNow;
    }

    // Regla de Negocio: Proceso de Baja
    public void DarDeBaja(string motivo)
    {
        if (InscripcionEstadoId == (int)EstadoInscripcionEnum.Finalizada)
            throw new DomainException("No se puede dar de baja una inscripción ya finalizada.");

        this.InscripcionEstadoId = (int)EstadoInscripcionEnum.Baja;
        this.EsBaja = true;
        this.FechaBaja = DateTime.UtcNow;
        this.MotivoBaja = motivo;
        this.ActualizadoEn = DateTime.UtcNow;
    }
}
