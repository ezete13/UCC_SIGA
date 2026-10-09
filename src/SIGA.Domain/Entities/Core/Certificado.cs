using SIGA.Domain.Entities.Catalog.Static;
using SIGA.Domain.Exceptions;

namespace SIGA.Domain.Entities.Core;

public partial class Certificado
{
    public int Id { get; set; }
    public required Guid Uuid { get; set; }
    public required string Token { get; set; }
    public required int InscripcionId { get; set; }
    public required int AlumnoId { get; set; }
    public required int CertificadoEstadoId { get; set; }
    public int Version { get; set; } = 1;
    public bool EsVersionActual { get; set; } = true;
    public required string HashSeguridad { get; set; }
    public required string TituloCertificado { get; set; }
    public string? TextoCertificado { get; set; }
    public int? HorasCertificadas { get; set; }
    public string? NotaFinal { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFinalizacion { get; set; }
    public required DateOnly FechaEmision { get; set; }
    public int? UsuarioId { get; set; }
    public DateTime? FechaValidacion { get; set; }
    public string? IpValidacion { get; set; }
    public string? UrlVerificacion { get; set; }
    public string? RutaAlmacenamientoPdf { get; set; }
    public DateTime? FechaRevocacion { get; set; }
    public string? MotivoRevocacion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ActualizadoEn { get; set; }
    public virtual required Alumno Alumno { get; set; }
    public virtual required Inscripcion Inscripcion { get; set; }
    public virtual required CertificadoEstado CertificadoEstado { get; set; }

    public static Certificado Crear(
        Inscripcion inscripcion,
        Alumno alumno,
        CertificadoEstado estado,
        DateOnly? fechaInicio,
        DateOnly? fechaFin,
        int? usuarioId
    )
    {
        if (inscripcion.InscripcionEstado.Codigo != "FINALIZADA")
            throw new DomainException("El estado de inscripción a la propuesa no está finalizada");

        if (alumno.AlumnoEstado.Codigo != "ALTA")
            throw new DomainException("El alumno no está en estado ALTA");

        if (inscripcion.AlumnoId != alumno.Id)
            throw new DomainException("La inscripción no corresponde al alumno");

        var certificado = new Certificado
        {
            Uuid = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString("N"),
            InscripcionId = inscripcion.Id,
            AlumnoId = alumno.Id,
            CertificadoEstadoId = estado.Id,
            Version = 1,
            EsVersionActual = true,
            HashSeguridad = "HashSeguridad",
            TituloCertificado = inscripcion.PropuestaVersion.Propuesta.Titulo,
            HorasCertificadas = inscripcion.PropuestaVersion.Propuesta.CantidadHoras,
            FechaInicio = fechaInicio,
            FechaFinalizacion = fechaFin,
            FechaEmision = DateOnly.FromDateTime(DateTime.UtcNow),
            UsuarioId = usuarioId,
            Activo = true,
            Alumno = alumno,
            Inscripcion = inscripcion,
            CertificadoEstado = estado,
        };

        // certificado.CambiarEstado(estado, usuarioId, "Creación del certificado", null);

        return certificado;
    }

    public void CambiarEstado(
        CertificadoEstado nuevoEstado,
        int? usuarioId,
        string? observacion,
        string? ip
    )
    {
        CertificadoEstadoId = nuevoEstado.Id;
    }
}
