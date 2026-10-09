using System;
using System.Collections.Generic;
using SIGA.Domain.Entities.Catalog.Dynamic;
using SIGA.Domain.Entities.Catalog.Static;
using SIGA.Domain.Exceptions;

namespace SIGA.Domain.Entities.Core;

public partial class PropuestaVersion
{
    public int Id { get; set; }
    public Guid Uuid { get; set; }
    public int PropuestaId { get; set; }
    public int PeriodoLectivoId { get; set; }
    public int ModalidadId { get; set; }
    public int EstadoPropuestaId { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public int MaximoAlumnos { get; set; }
    public int CuposDisponibles { get; set; }
    public decimal? ImporteBase { get; set; }
    public int? Cuotas { get; set; }
    public string? ConceptoPago { get; set; }
    public bool PermiteInscripcionesWeb { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public virtual Propuesta Propuesta { get; set; } = null!;
    public virtual PeriodoLectivo PeriodoLectivo { get; set; } = null!;
    public virtual Modalidad Modalidad { get; set; } = null!;
    public virtual PropuestaEstado PropuestaEstado { get; set; } = null!;

    public virtual ICollection<Preinscripcion> Preinscripciones { get; set; } = new List<Preinscripcion>();
    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    public virtual ICollection<PropuestaDocente> PropuestaDocentes { get; set; } = new List<PropuestaDocente>();

    public static PropuestaVersion Crear(
        int propuestaId,
        int periodoLectivoId,
        int modalidadId,
        DateOnly fechaInicio,
        DateOnly fechaFin,
        int maximoAlumnos,
        decimal? importeBase = null,
        int? cuotas = null,
        string? conceptoPago = null,
        bool permiteInscripcionesWeb = false
    )
    {
        if (fechaInicio >= fechaFin)
            throw new DomainException("La fecha de inicio debe ser anterior a la fecha de fin");

        if (maximoAlumnos <= 0)
            throw new DomainException("El máximo de alumnos debe ser mayor a cero");

        return new PropuestaVersion
        {
            Uuid = Guid.NewGuid(),
            PropuestaId = propuestaId,
            PeriodoLectivoId = periodoLectivoId,
            ModalidadId = modalidadId,
            EstadoPropuestaId = 1, // BORRADOR
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            MaximoAlumnos = maximoAlumnos,
            CuposDisponibles = maximoAlumnos,
            ImporteBase = importeBase,
            Cuotas = cuotas,
            ConceptoPago = conceptoPago?.Trim(),
            PermiteInscripcionesWeb = permiteInscripcionesWeb,
            CreadoEn = DateTime.UtcNow,
            ActualizadoEn = DateTime.UtcNow
        };
    }

    public void Publicar()
    {
        if (PropuestaEstado?.Codigo != "BORRADOR")
            throw new InvalidOperationException("Solo se pueden publicar propuestas en estado borrador");

        if (ImporteBase.HasValue && string.IsNullOrWhiteSpace(ConceptoPago))
            throw new InvalidOperationException("Debe especificar el concepto de pago si la propuesta tiene un importe base");

        EstadoPropuestaId = 2; // PUBLICADO
        ActualizadoEn = DateTime.UtcNow;
    }

    public void Archivar()
    {
        if (PropuestaEstado?.Codigo == "ARCHIVADO")
            throw new InvalidOperationException("La propuesta ya está archivada");

        EstadoPropuestaId = 3; // ARCHIVADO
        PermiteInscripcionesWeb = false;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void SetPermiteInscripcionesWeb(bool permite)
    {
        if (PropuestaEstado?.Codigo == "ARCHIVADO" || PropuestaEstado?.Codigo == "BORRADOR")
            throw new InvalidOperationException("No se pueden activar las inscripciones web de una propuesta archivada o borrador");

        PermiteInscripcionesWeb = permite;
        ActualizadoEn = DateTime.UtcNow;
    }

    public bool InscribirAlumno()
    {
        if (PropuestaEstado?.Codigo != "PUBLICADO")
            throw new InvalidOperationException("Solo se pueden inscribir alumnos en propuestas publicadas");

        if (!PermiteInscripcionesWeb)
            throw new InvalidOperationException("Esta propuesta no permite inscripciones web");

        if (CuposDisponibles <= 0)
            return false;

        CuposDisponibles--;
        ActualizadoEn = DateTime.UtcNow;
        return true;
    }

    public void CancelarInscripcion()
    {
        if (CuposDisponibles < MaximoAlumnos)
        {
            CuposDisponibles++;
            ActualizadoEn = DateTime.UtcNow;
        }
    }
}
