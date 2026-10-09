using SIGA.Domain.Entities.Catalog.Static;
using SIGA.Domain.Enums;
using SIGA.Domain.Exceptions;

namespace SIGA.Domain.Entities.Core;

public class Preinscripcion
{
    public int Id { get; set; }

    public required Guid Uuid { get; set; } = Guid.NewGuid();

    public int? AlumnoId { get; set; }

    public required int PropuestaVersionId { get; set; }

    public required int EstadoPreinscripcionId { get; set; }

    public required int TipoDocumentoId { get; set; }

    public required string Documento { get; set; }

    public required string Apellido { get; set; }

    public required string Nombre { get; set; }

    public required string Email { get; set; }

    public string? Telefono { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public DateTime? ActualizadoEn { get; set; }

    public virtual PropuestaVersion PropuestaVersion { get; set; } = null!;

    public virtual Inscripcion Inscripcion { get; set; } = null!;

    public virtual TipoDocumento TipoDocumento { get; set; } = null!;

    public virtual PreinscripcionEstado PreinscripcionEstado { get; set; } = null!;

    public virtual Alumno? Alumno { get; set; }

    public static Preinscripcion Crear(
        int propuestaVersionId,
        int tipoDocumentoId,
        string documento,
        string apellido,
        string nombre,
        string email
    )
    {
        return new Preinscripcion
        {
            Uuid = Guid.NewGuid(),
            PropuestaVersionId = propuestaVersionId,
            EstadoPreinscripcionId = (int)EstadoPreinscripcionEnum.Pendiente,
            TipoDocumentoId = tipoDocumentoId,
            Documento = documento,
            Apellido = apellido,
            Nombre = nombre,
            Email = email,
            CreadoEn = DateTime.UtcNow,
        };
    }

    public void Aprobar(int alumnoId)
    {
        if (this.EstadoPreinscripcionId == (int)EstadoPreinscripcionEnum.Revocada)
            throw new DomainException(
                "No se puede aprobar una preinscripción que ya ha sido revocada."
            );

        if (this.EstadoPreinscripcionId == (int)EstadoPreinscripcionEnum.Aprobada)
            return;

        if (alumnoId <= 0)
            throw new DomainException(
                "Debe proporcionar un Alumno válido para aprobar la preinscripción."
            );

        this.AlumnoId = alumnoId;
        this.EstadoPreinscripcionId = (int)EstadoPreinscripcionEnum.Aprobada;
        this.ActualizadoEn = DateTime.UtcNow;
    }

    public void Revocar(string motivo)
    {
        if (this.EstadoPreinscripcionId == (int)EstadoPreinscripcionEnum.Aprobada)
            throw new DomainException(
                "No se puede revocar una preinscripción que ya ha sido aprobada."
            );

        this.EstadoPreinscripcionId = (int)EstadoPreinscripcionEnum.Revocada;
        this.Observaciones = motivo;
        this.ActualizadoEn = DateTime.UtcNow;
    }
}
