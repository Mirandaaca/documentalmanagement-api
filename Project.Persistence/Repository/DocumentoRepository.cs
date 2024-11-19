using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.Documento;
using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
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
  public class DocumentoRepository : IDocumentoRepository
  {
    private readonly DocumentalManagement _context;
    public DocumentoRepository(DocumentalManagement context)
    {
      _context = context;
    }
    public async Task EliminarDocumento(int id)
    {
      var documento = await _context.Documentos
        .Include(d => d.Estudiante)
        .Include(d => d.TipoDocumento)
        .FirstOrDefaultAsync(d => d.Id == id);

      // Verificar si el documento existe
      if (documento == null)
      {
        throw new KeyNotFoundException($"El documento con ID {id} no fue encontrado.");
      }

      // Eliminar el documento
      _context.Documentos.Remove(documento);
      await _context.SaveChangesAsync();

      // Verificar si el estado del estudiante ha cambiado después de la eliminación del documento
      await VerificarEstadoEstudiante(documento.Estudiante.Id);
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

    public async Task<ReadDocumentoDTO> ObtenerDocumentoPorId(int id)
    {
      var documento = await _context.Documentos
        .Include(a=>a.Gestion)
        .Include(a => a.Estudiante)
        .Include(a => a.TipoDocumento)
        .FirstOrDefaultAsync(d=>d.Id==id);

      // Verificar si el documento existe
      if (documento == null)
      {
        throw new KeyNotFoundException($"El documento con ID {id} no fue encontrado.");
      }

      // Mapear el Documento a ReadDocumentoDTO
      var documentoDTO = new ReadDocumentoDTO
      {
        Id = documento.Id,
        IdGestion = documento.IdGestion,
        AnioGestion = documento.Gestion.AnioGestion,
        IdEstudiante = documento.IdEstudiante,
        NombreEstudiante = documento.Estudiante.Nombre,
        RegistroEstudiante = documento.Estudiante.NroRegistro,
        IdUsuario = documento.IdUsuario,
        IdTipoDocumento = documento.IdTipoDocumento,
        NombreTipoDocumento = documento.TipoDocumento.Nombre,
        FechaSubida = documento.FechaSubida,
        Contenido = documento.Contenido,
        Extension = documento.Extension,
        NombreArchivo = documento.NombreArchivo,
        Ruta = documento.Ruta,
        Legalizado = documento.Legalizado
      };

      return documentoDTO;
    }

    public async Task<List<ReadDocumentoDTO>> ObtenerDocumentos()
    {
      var documentos = await _context.Documentos
        .Include(a => a.Gestion)
        .Include(a => a.Estudiante)
        .Include(a => a.TipoDocumento)
        .ToListAsync();

      // Convertir cada Documento en un ReadDocumentoDTO
      var documentosDTO = documentos.Select(d => new ReadDocumentoDTO
      {
        Id = d.Id,
        IdGestion = d.IdGestion,
        IdEstudiante = d.IdEstudiante,
        NombreEstudiante = d.Estudiante.Nombre,
        RegistroEstudiante = d.Estudiante.NroRegistro,
        IdUsuario = d.IdUsuario,
        IdTipoDocumento = d.IdTipoDocumento,
        NombreTipoDocumento = d.TipoDocumento.Nombre,
        AnioGestion = d.Gestion.AnioGestion,
        FechaSubida = d.FechaSubida,
        Contenido = d.Contenido,
        Extension = d.Extension,
        NombreArchivo = d.NombreArchivo,
        Ruta = d.Ruta,
        Legalizado = d.Legalizado
      }).ToList();

      return documentosDTO;
    }

    public async Task UploadDocumento(UploadDocumentoDTO uploadDocumento, IFormFile documento)
    {
      // Verificar que el archivo no esté vacío
      if (documento == null || documento.Length == 0)
      {
        throw new Exception("El documento no puede estar vacío.");
      }

      // Leer el contenido del archivo
      using var ms = new MemoryStream();
      await documento.CopyToAsync(ms);
      var fileBytes = ms.ToArray();

      // Buscar si ya existe un documento similar
      var documentoExistente = await _context.Documentos
          .FirstOrDefaultAsync(d => d.IdGestion == uploadDocumento.IdGestion &&
                                    d.IdEstudiante == uploadDocumento.IdEstudiante &&
                                    d.IdTipoDocumento == uploadDocumento.IdTipoDocumento);

      if (documentoExistente == null)
      {
        // Crear un nuevo documento
        var nuevoDocumento = new Documento
        {
          IdGestion = uploadDocumento.IdGestion,
          IdEstudiante = uploadDocumento.IdEstudiante,
          IdUsuario = uploadDocumento.IdUsuario,
          IdTipoDocumento = uploadDocumento.IdTipoDocumento,
          FechaSubida = DateTime.Now,
          Contenido = fileBytes,
          Extension = Path.GetExtension(documento.FileName),
          NombreArchivo = documento.FileName,
          Ruta = $"ruta/de/almacenamiento/{documento.FileName}",
          Legalizado = uploadDocumento.Legalizado
        };
        _context.Documentos.Add(nuevoDocumento);
      }
      else
      {
        // Actualizar el documento existente
        documentoExistente.Contenido = fileBytes;
        documentoExistente.Extension = Path.GetExtension(documento.FileName);
        documentoExistente.NombreArchivo = documento.FileName;
        documentoExistente.FechaSubida = DateTime.Now;
        documentoExistente.Legalizado = uploadDocumento.Legalizado;

        _context.Documentos.Update(documentoExistente);
      }

      // Guardar cambios
      await _context.SaveChangesAsync();

      // Verificar y actualizar el estado del estudiante
      await VerificarEstadoEstudiante(uploadDocumento.IdEstudiante);
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

    public async Task<List<ReadDocumentoDTO>> ObtenerDocumentosPorEstudiante(int idEstudiante)
    {
      var documentos = await _context.Documentos
                                     .Where(d => d.IdEstudiante == idEstudiante)
                                     .ToListAsync();

      // Convertir cada Documento en un ReadDocumentoDTO
      var documentosDTO = documentos.Select(d => new ReadDocumentoDTO
      {
        Id = d.Id,
        IdGestion = d.IdGestion,
        IdEstudiante = d.IdEstudiante,
        IdUsuario = d.IdUsuario,
        IdTipoDocumento = d.IdTipoDocumento,
        FechaSubida = d.FechaSubida,
        Contenido = d.Contenido,
        Extension = d.Extension,
        NombreArchivo = d.NombreArchivo,
        Ruta = d.Ruta,
        Legalizado = d.Legalizado
      }).ToList();

      return documentosDTO;
    }
    public async Task<ReadDocumentoDTO> ObtenerDocumentoDeEstudiantePorId(int idEstudiante, int idDocumento)
    {
      var documento = await _context.Documentos
                                    .FirstOrDefaultAsync(d => d.IdEstudiante == idEstudiante && d.Id == idDocumento);

      // Verificar si el documento existe
      if (documento == null)
      {
        throw new KeyNotFoundException($"No se encontró el documento con ID {idDocumento} para el estudiante con ID {idEstudiante}.");
      }

      // Mapear el Documento a ReadDocumentoDTO
      var documentoDTO = new ReadDocumentoDTO
      {
        Id = documento.Id,
        IdGestion = documento.IdGestion,
        IdEstudiante = documento.IdEstudiante,
        IdUsuario = documento.IdUsuario,
        IdTipoDocumento = documento.IdTipoDocumento,
        FechaSubida = documento.FechaSubida,
        Contenido = documento.Contenido,
        Extension = documento.Extension,
        NombreArchivo = documento.NombreArchivo,
        Ruta = documento.Ruta,
        Legalizado = documento.Legalizado
      };

      return documentoDTO;
    }

    public async Task<List<ReadDocumentoDTO>> ObtenerDocumentosPorGestion(int idGestion)
    {
      var documentos = await _context.Documentos
                                     .Where(d => d.IdGestion == idGestion)
                                     .ToListAsync();

      // Convertir cada Documento en un ReadDocumentoDTO
      var documentosDTO = documentos.Select(d => new ReadDocumentoDTO
      {
        Id = d.Id,
        IdGestion = d.IdGestion,
        IdEstudiante = d.IdEstudiante,
        IdUsuario = d.IdUsuario,
        IdTipoDocumento = d.IdTipoDocumento,
        FechaSubida = d.FechaSubida,
        Contenido = d.Contenido,
        Extension = d.Extension,
        NombreArchivo = d.NombreArchivo,
        Ruta = d.Ruta,
        Legalizado = d.Legalizado
      }).ToList();

      return documentosDTO;
    }
    public async Task<List<SeguimientoEstudianteDTO>> ObtenerSeguimientoEstudiantesAsync()
    {
      var fechaActual = DateTime.Now.Date;

      // Obtener la gestión actual
      var gestionActual = await _context.Gestiones
          .OrderByDescending(g => g.AnioGestion)
          .FirstOrDefaultAsync();

      if (gestionActual == null)
      {
        throw new Exception("No se encontró una gestión activa.");
      }

      // Obtener las fechas límite por semestre
      var fechasLimite = new List<(string Semestre, DateTime FechaLimite)>
    {
        ("Primer Semestre", gestionActual.FechaPrimerSemestreFin.Date),
        ("Segundo Semestre", gestionActual.FechaSegundoSemestreFin.Date)
    };

      // Obtener estudiantes y sus documentos
      var estudiantes = await _context.Estudiantes
          .Include(e => e.Documentos)
          .ToListAsync();

      var tipoDocumentos = await _context.TipoDocumentos.ToListAsync();

      var seguimiento = new List<SeguimientoEstudianteDTO>();

      foreach (var estudiante in estudiantes)
      {
        var documentosFaltantes = new List<DocumentoFaltanteDTO>();

        foreach (var tipoDocumento in tipoDocumentos)
        {
          // Verificar si el estudiante no tiene este documento
          if (!estudiante.Documentos.Any(d => d.IdTipoDocumento == tipoDocumento.Id))
          {
            foreach (var (semestre, fechaLimite) in fechasLimite)
            {
              var diasRestantes = (fechaLimite - fechaActual).Days;

              if (fechaActual > fechaLimite)
              {
                // Documento ya expiró
                documentosFaltantes.Add(new DocumentoFaltanteDTO
                {
                  NombreDocumento = tipoDocumento.Nombre,
                  DiasRestantes = diasRestantes, // Será negativo
                  EstaExpirado = true,
                  Semestre = semestre
                });
              }
              else
              {
                // Documento está por vencer
                documentosFaltantes.Add(new DocumentoFaltanteDTO
                {
                  NombreDocumento = tipoDocumento.Nombre,
                  DiasRestantes = diasRestantes,
                  EstaExpirado = false,
                  Semestre = semestre
                });
              }
            }
          }
        }

        // Agregar al seguimiento si tiene documentos faltantes
        if (documentosFaltantes.Any())
        {
          var nonExpiredDocuments = documentosFaltantes.Where(d => !d.EstaExpirado).ToList();
          var proximaExpiracion = nonExpiredDocuments.Any()
              ? nonExpiredDocuments.Min(d => d.DiasRestantes)
              : (int?)null;

          seguimiento.Add(new SeguimientoEstudianteDTO
          {
            IdEstudiante = estudiante.Id,
            Nombre = estudiante.Nombre,
            Apellido = estudiante.Apellido,
            Correo = estudiante.Email,
            NroRegistro = estudiante.NroRegistro,
            DocumentosFaltantes = documentosFaltantes,
            FechaProximaExpiracion = proximaExpiracion.HasValue
                  ? fechaActual.AddDays(proximaExpiracion.Value)
                  : (DateTime?)null,
            DiasParaProximaExpiracion = proximaExpiracion
          });
        }
      }

      // Ordenar por documentos con menor tiempo para expirar (o los más vencidos primero)
      return seguimiento
          .OrderBy(s => s.DiasParaProximaExpiracion ?? int.MaxValue)
          .ThenByDescending(s => s.DocumentosFaltantes.Any(d => d.EstaExpirado))
          .ToList();
    }

  }
}
