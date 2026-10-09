using System;
using System.Collections.Generic;
using SIGA.Domain.Entities.Catalog.Dynamic;
using SIGA.Domain.Exceptions;

namespace SIGA.Domain.Entities.Core;

public partial class Propuesta
{
    public int Id { get; set; }
    public Guid guid { get; set; }
    public int UnidadId { get; set; }
    public int TipoPropuestaId { get; set; }
    public int UsuarioId { get; set; }

    public required string Titulo { get; set; }
    public string? Descripcion { get; set; }
    
    public int CantidadHoras { get; set; }
    public string? EmailEncargado { get; set; }
    public string? PlanEstudioPdf { get; set; }
    public string? LugarRealizacion { get; set; }
    public bool? Estado { get; set; }
    public string? ContactoInfo { get; set; }
    public string? PagosInfo { get; set; }
    public bool WebVisible { get; set; }
    
    public DateTime? CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public virtual Unidad Unidad { get; set; } = null!;
    public virtual TipoPropuesta TipoPropuesta { get; set; } = null!;
    public virtual PropuestaWeb? PropuestaWeb { get; set; }
    public virtual ICollection<Temario> PropuestaContenidos { get; set; } = new List<Temario>();
    
    // Nueva relacion a versiones
    public virtual ICollection<PropuestaVersion> Versiones { get; set; } = new List<PropuestaVersion>();

    public static Propuesta Crear(
        int unidadId,
        int tipoPropuestaId,
        string titulo,
        string? descripcion,
        int cantidadHoras,
        int usuarioId,
        string? emailEncargado = null,
        string? planEstudioPdf = null,
        string? lugarRealizacion = null,
        string? contactoInfo = null,
        string? pagosInfo = null,
        bool webVisible = false
    )
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("El título es requerido");

        if (cantidadHoras <= 0)
            throw new DomainException("La cantidad de horas debe ser mayor a cero");

        return new Propuesta
        {
            UnidadId = unidadId,
            TipoPropuestaId = tipoPropuestaId,
            UsuarioId = usuarioId,
            Titulo = titulo.Trim(),
            Descripcion = descripcion?.Trim(),
            CantidadHoras = cantidadHoras,
            EmailEncargado = emailEncargado?.Trim(),
            PlanEstudioPdf = planEstudioPdf,
            LugarRealizacion = lugarRealizacion?.Trim(),
            ContactoInfo = contactoInfo?.Trim(),
            PagosInfo = pagosInfo?.Trim(),
            WebVisible = webVisible,
            Estado = true,
            CreadoEn = DateTime.UtcNow,
            ActualizadoEn = DateTime.UtcNow,
        };
    }

    public void Actualizar(
        string titulo,
        string? descripcion,
        int cantidadHoras,
        string? emailEncargado,
        string? planEstudioPdf,
        string? lugarRealizacion,
        string? contactoInfo,
        string? pagosInfo,
        bool webVisible,
        int? unidadId = null,
        int? tipoPropuestaId = null
    )
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("El título es requerido");

        if (cantidadHoras <= 0)
            throw new DomainException("La cantidad de horas debe ser mayor a cero");

        if (unidadId.HasValue) UnidadId = unidadId.Value;
        if (tipoPropuestaId.HasValue) TipoPropuestaId = tipoPropuestaId.Value;

        Titulo = titulo.Trim();
        Descripcion = descripcion?.Trim();
        CantidadHoras = cantidadHoras;
        EmailEncargado = emailEncargado?.Trim();
        PlanEstudioPdf = planEstudioPdf;
        LugarRealizacion = lugarRealizacion?.Trim();
        ContactoInfo = contactoInfo?.Trim();
        PagosInfo = pagosInfo?.Trim();
        WebVisible = webVisible;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void SetWebVisible(bool visible)
    {
        WebVisible = visible;
        ActualizadoEn = DateTime.UtcNow;
    }
}
