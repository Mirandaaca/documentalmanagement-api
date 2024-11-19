using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.TipoDocumento;
using Project.Application.Interfaces.Persistence;
using Project.Domain.Entities;
using Project.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
  public class TipoDocumentoRepository : ITipoDocumentoRepository
  {
    private readonly DocumentalManagement _context;
    public TipoDocumentoRepository(DocumentalManagement context) {
      _context = context;
    }
    public async Task ActualizarTipoDocumento(UpdateTipoDocumentoDTO tipoDocumento)
    {
      TipoDocumento tipoDocumentoEntity = await _context.TipoDocumentos.FirstOrDefaultAsync(x => x.Id == tipoDocumento.Id);
      if (tipoDocumentoEntity == null)
      {
        throw new ApplicationException("Tipo de documento no encontrado");
      }
      tipoDocumentoEntity.Nombre = tipoDocumento.Nombre;
      tipoDocumentoEntity.Descripcion = tipoDocumento.Descripcion;
      tipoDocumentoEntity.IdCategoriaDocumento = tipoDocumento.IdCategoriaDocumento;
      tipoDocumentoEntity.FechaExpiracion = tipoDocumento.FechaExpiracion;
      _context.TipoDocumentos.Update(tipoDocumentoEntity);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarTipoDocumento(int id)
    {
      TipoDocumento tipoDocumento = await _context.TipoDocumentos.FirstOrDefaultAsync(x => x.Id == id);
      if (tipoDocumento == null)
      {
        throw new ApplicationException("Tipo de documento no encontrado");
      }
      _context.TipoDocumentos.Remove(tipoDocumento);
      await _context.SaveChangesAsync();
    }

    public async Task GuardarTipoDocumento(CreateTipoDocumentoDTO tipoDocumento)
    {
      TipoDocumento tipoDocumentoEntity = new TipoDocumento
      {
        Nombre = tipoDocumento.Nombre,
        Descripcion = tipoDocumento.Descripcion,
        IdCategoriaDocumento = tipoDocumento.IdCategoriaDocumento,
        FechaExpiracion = tipoDocumento.FechaExpiracion
      };
      _context.TipoDocumentos.Add(tipoDocumentoEntity);
      await _context.SaveChangesAsync();

      // Revalidar el estado de todos los estudiantes
      var estudiantes = await _context.Estudiantes.ToListAsync();
      foreach (var estudiante in estudiantes)
      {
        await VerificarEstadoEstudiante(estudiante.Id);
      }
    }
    private async Task VerificarEstadoEstudiante(int idEstudiante)
    {
      // Obtener los tipos de documentos requeridos por estado
      var documentosAdmision = await _context.TipoDocumentos
          .Where(td => td.CategoriaDocumento.Nombre == "Admision")
          .Select(td => td.Id).ToListAsync();

      var documentosHabilitado = await _context.TipoDocumentos
          .Where(td => td.CategoriaDocumento.Nombre == "Habilitado")
          .Select(td => td.Id).ToListAsync();

      var documentosGraduado = await _context.TipoDocumentos
          .Where(td => td.CategoriaDocumento.Nombre == "Graduado")
          .Select(td => td.Id).ToListAsync();

      // Obtener los documentos que el estudiante ya presentó
      var documentosPresentados = await _context.Documentos
          .Where(d => d.IdEstudiante == idEstudiante)
          .Select(d => d.IdTipoDocumento).ToListAsync();

      // Determinar el estado
      var nuevoEstado = "Sin Documentacion";

      // Lógica de asignación de estado basada en la secuencia de documentos presentados
      if (documentosAdmision.All(d => documentosPresentados.Contains(d)))
      {
        nuevoEstado = "Admision";
      }
      if (documentosHabilitado.All(d => documentosPresentados.Contains(d)) &&
          documentosPresentados.Contains(documentosAdmision.FirstOrDefault()))
      {
        nuevoEstado = "Habilitado";
      }
      if (documentosGraduado.All(d => documentosPresentados.Contains(d)) &&
          documentosPresentados.Contains(documentosHabilitado.FirstOrDefault()))
      {
        nuevoEstado = "Graduado";
      }

      // Actualizar el estado del estudiante si cambió
      var estudiante = await _context.Estudiantes.FindAsync(idEstudiante);
      if (estudiante.Estado != nuevoEstado)
      {
        estudiante.Estado = nuevoEstado;
        _context.Estudiantes.Update(estudiante);
        await _context.SaveChangesAsync();
      }
    }
    //private async Task VerificarEstadoEstudiante(int idEstudiante)
    //{
    //  // Obtener los tipos de documentos requeridos por estado
    //  var documentosAdmision = await _context.TipoDocumentos
    //      .Where(td => td.CategoriaDocumento.Nombre == "Admision")
    //      .Select(td => td.Id).ToListAsync();

    //  var documentosHabilitado = await _context.TipoDocumentos
    //      .Where(td => td.CategoriaDocumento.Nombre == "Habilitado")
    //      .Select(td => td.Id).ToListAsync();

    //  var documentosGraduado = await _context.TipoDocumentos
    //      .Where(td => td.CategoriaDocumento.Nombre == "Graduado")
    //      .Select(td => td.Id).ToListAsync();

    //  // Obtener los documentos que el estudiante ya presentó
    //  var documentosPresentados = await _context.Documentos
    //      .Where(d => d.IdEstudiante == idEstudiante)
    //      .Select(d => d.IdTipoDocumento).ToListAsync();

    //  // Determinar el estado
    //  var nuevoEstado = "Sin Documentacion";
    //  if (documentosAdmision.All(d => documentosPresentados.Contains(d)))
    //  {
    //    nuevoEstado = "Admision";
    //  }
    //  if (documentosHabilitado.All(d => documentosPresentados.Contains(d)))
    //  {
    //    nuevoEstado = "Habilitado";
    //  }
    //  if (documentosGraduado.All(d => documentosPresentados.Contains(d)))
    //  {
    //    nuevoEstado = "Graduado";
    //  }

    //  // Actualizar el estado del estudiante si cambió
    //  var estudiante = await _context.Estudiantes.FindAsync(idEstudiante);
    //  if (estudiante.Estado != nuevoEstado)
    //  {
    //    estudiante.Estado = nuevoEstado;
    //    _context.Estudiantes.Update(estudiante);
    //    await _context.SaveChangesAsync();
    //  }
    //}

    public async Task<ReadTipoDocumentoDTO> ObtenerTipoDocumentoPorId(int id)
    {
      TipoDocumento tipoDocumento = await _context.TipoDocumentos.FirstOrDefaultAsync(x => x.Id == id);
      if (tipoDocumento == null)
      {
        throw new ApplicationException("Tipo de documento no encontrado");
      }
      return new ReadTipoDocumentoDTO
      {
        Id = tipoDocumento.Id,
        Nombre = tipoDocumento.Nombre,
        Descripcion = tipoDocumento.Descripcion,
        IdCategoriaDocumento = tipoDocumento.IdCategoriaDocumento,
        FechaExpiracion = tipoDocumento.FechaExpiracion
      };
    }

    public async Task<List<ReadTipoDocumentoDTO>> ObtenerTiposDocumento()
    {
      List<TipoDocumento> tiposDocumento = await _context.TipoDocumentos.ToListAsync();
      List<ReadTipoDocumentoDTO> tiposDocumentoDTO = new List<ReadTipoDocumentoDTO>();
      foreach (TipoDocumento tipoDocumento in tiposDocumento)
      {
        tiposDocumentoDTO.Add(new ReadTipoDocumentoDTO
        {
          Id = tipoDocumento.Id,
          Nombre = tipoDocumento.Nombre,
          Descripcion = tipoDocumento.Descripcion,
          IdCategoriaDocumento = tipoDocumento.IdCategoriaDocumento,
          FechaExpiracion = tipoDocumento.FechaExpiracion
        });
      }
      return tiposDocumentoDTO;
    }
  }
}
