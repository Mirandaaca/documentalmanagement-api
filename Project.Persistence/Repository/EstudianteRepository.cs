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
  public class EstudianteRepository : IEstudianteRepository
  {
    private readonly DocumentalManagement _context;
    public EstudianteRepository(DocumentalManagement context)
    {
      _context = context;
    }
    public async Task ActualizarEstudiante(UpdateEstudianteDTO estudiante)
    {
      Estudiante estudianteEntity = await _context.Estudiantes.FirstOrDefaultAsync(x => x.Id == estudiante.Id);
      if (estudianteEntity == null)
      {
        throw new ApplicationException("Estudiante no encontrado");
      }
      estudianteEntity.IdCarrera = estudiante.IdCarrera;
      estudianteEntity.Nombre = estudiante.Nombre;
      estudianteEntity.Apellido = estudiante.Apellido;
      estudianteEntity.NroRegistro = estudiante.NroRegistro;
      estudianteEntity.FechaNacimiento = estudiante.FechaNacimiento;
      estudianteEntity.Telefono = estudiante.Telefono;
      estudianteEntity.Email = estudiante.Email;
      _context.Estudiantes.Update(estudianteEntity);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarEstudiante(int id)
    {
      Estudiante estudiante = await _context.Estudiantes.FirstOrDefaultAsync(x => x.Id == id);
      if (estudiante == null) {
        throw new ApplicationException("Estudiante no encontrado");
      }
      _context.Estudiantes.Remove(estudiante);
      await _context.SaveChangesAsync();
    }

    public async Task GuardarEstudiante(CreateEstudianteDTO estudiante)
    {
      Estudiante estudianteEntity = new Estudiante
      {
        IdCarrera = estudiante.IdCarrera,
        Nombre = estudiante.Nombre,
        Apellido = estudiante.Apellido,
        NroRegistro = estudiante.NroRegistro,
        FechaNacimiento = estudiante.FechaNacimiento,
        Estado = "Sin Documentacion",
        Telefono = estudiante.Telefono,
        Email = estudiante.Email
      };
      _context.Estudiantes.Add(estudianteEntity);
      await _context.SaveChangesAsync();
    }
    public async Task<List<ReadEstudiantesConDocumentosFaltantesYPresentadosDTO>> ObtenerEstudiantesConDocumentosFaltantesYPresentados()
    {
      // Obtener todas las categorías y sus tipos de documentos
      var categoriasConDocumentos = await _context.CategoriasDocumento
          .Include(c => c.TiposDocumento)
          .ToListAsync();

      // Obtener todos los estudiantes con sus documentos
      var estudiantes = await _context.Estudiantes
          .Include(e => e.Documentos)
          .ThenInclude(d => d.TipoDocumento) // Incluir los tipos de documento asociados a los documentos
          .ToListAsync();

      var resultado = new List<ReadEstudiantesConDocumentosFaltantesYPresentadosDTO>();

      foreach (var estudiante in estudiantes)
      {
        var documentosFaltantesPorCategoria = new List<DocumentosFaltantesPorCategoriaDTO>();
        var documentosPresentadosPorCategoria = new List<DocumentosPresentadosPorCategoriaDTO>();

        foreach (var categoria in categoriasConDocumentos)
        {
          var documentosFaltantes = categoria.TiposDocumento
              .Where(td => !estudiante.Documentos.Any(d => d.IdTipoDocumento == td.Id))
              .Select(td => td.Nombre)
              .ToList();

          var documentosPresentados = estudiante.Documentos
              .Where(d => categoria.TiposDocumento.Any(td => td.Id == d.IdTipoDocumento))
              .Select(d => new ReadDocumentoPresentadoDTO
              {
                IdDocumento = d.Id,
                NombreArchivo = d.NombreArchivo,
                FechaSubida = d.FechaSubida,
                TipoDocumentoNombre = d.TipoDocumento.Nombre
              })
              .ToList();

          // Solo agregar si hay documentos faltantes o presentados
          if (documentosFaltantes.Any())
          {
            documentosFaltantesPorCategoria.Add(new DocumentosFaltantesPorCategoriaDTO
            {
              Categoria = categoria.Nombre,
              TiposDeDocumentoFaltantes = documentosFaltantes
            });
          }

          if (documentosPresentados.Any())
          {
            documentosPresentadosPorCategoria.Add(new DocumentosPresentadosPorCategoriaDTO
            {
              Categoria = categoria.Nombre,
              DocumentosPresentados = documentosPresentados
            });
          }
        }

        // Si hay documentos faltantes o presentados, agregar al resultado
        if (documentosFaltantesPorCategoria.Any() || documentosPresentadosPorCategoria.Any())
        {
          resultado.Add(new ReadEstudiantesConDocumentosFaltantesYPresentadosDTO
          {
            IdEstudiante = estudiante.Id,
            NombreCompleto = $"{estudiante.Nombre} {estudiante.Apellido}",
            Registro = estudiante.NroRegistro,
            Email = estudiante.Email,
            DocumentosFaltantes = documentosFaltantesPorCategoria,
            DocumentosPresentados = documentosPresentadosPorCategoria
          });
        }
      }

      return resultado;
    }


    public async Task<List<ReadEstudiantesConDocumentosFaltantesPorCategoriaDTO>> ObtenerEstudiantesConDocumentosFaltantesPorCategoria()
    {
      // Obtener todas las categorías y sus tipos de documentos
      var categoriasConDocumentos = await _context.CategoriasDocumento
          .Include(c => c.TiposDocumento)
          .ToListAsync();

      // Obtener todos los estudiantes con sus documentos
      var estudiantes = await _context.Estudiantes
          .Include(e => e.Documentos)
          .ThenInclude(d => d.TipoDocumento)
          .ToListAsync();

      var resultado = new List<ReadEstudiantesConDocumentosFaltantesPorCategoriaDTO>();

      foreach (var estudiante in estudiantes)
      {
        var documentosFaltantesPorCategoria = new List<DocumentosFaltantesPorCategoriaDTO>();

        foreach (var categoria in categoriasConDocumentos)
        {
          var documentosFaltantes = categoria.TiposDocumento
              .Where(td => !estudiante.Documentos.Any(d => d.IdTipoDocumento == td.Id))
              .Select(td => td.Nombre)
              .ToList();

          // Solo agregar si hay documentos faltantes
          if (documentosFaltantes.Any())
          {
            documentosFaltantesPorCategoria.Add(new DocumentosFaltantesPorCategoriaDTO
            {
              Categoria = categoria.Nombre,
              TiposDeDocumentoFaltantes = documentosFaltantes
            });
          }
        }

        // Si hay documentos faltantes, agregar al resultado
        if (documentosFaltantesPorCategoria.Any())
        {
          resultado.Add(new ReadEstudiantesConDocumentosFaltantesPorCategoriaDTO
          {
            IdEstudiante = estudiante.Id,
            NombreCompleto = $"{estudiante.Nombre} {estudiante.Apellido}",
            Registro = estudiante.NroRegistro,
            Email = estudiante.Email,
            DocumentosFaltantes = documentosFaltantesPorCategoria
          });
        }
      }

      return resultado;
    }
    public async Task<List<ReadEstudiantesConDocumentosPresentadosPorCategoriaDTO>> ObtenerEstudiantesConDocumentosPresentadosPorCategoria()
    {
      // Obtener todas las categorías y sus tipos de documentos
      var categoriasConDocumentos = await _context.CategoriasDocumento
          .Include(c => c.TiposDocumento)
          .ToListAsync();

      // Obtener todos los estudiantes con sus documentos
      var estudiantes = await _context.Estudiantes
          .Include(e => e.Documentos)
          .ThenInclude(d => d.TipoDocumento)
          .ToListAsync();

      var resultado = new List<ReadEstudiantesConDocumentosPresentadosPorCategoriaDTO>();

      foreach (var estudiante in estudiantes)
      {
        var documentosPresentadosPorCategoria = new List<DocumentosPresentadosPorCategoriaDTO>();

        foreach (var categoria in categoriasConDocumentos)
        {
          var documentosPresentados = estudiante.Documentos
    .Where(d => categoria.TiposDocumento.Any(td => td.Id == d.IdTipoDocumento))
    .Select(d => new ReadDocumentoPresentadoDTO
    {
      IdDocumento = d.Id,
      NombreArchivo = d.NombreArchivo,
      FechaSubida = d.FechaSubida,
      TipoDocumentoNombre = d.TipoDocumento.Nombre
    })
    .ToList();

          // Solo agregar si hay documentos presentados
          if (documentosPresentados.Any())
          {
            documentosPresentadosPorCategoria.Add(new DocumentosPresentadosPorCategoriaDTO
            {
              Categoria = categoria.Nombre,
              DocumentosPresentados = documentosPresentados
            });
          }
        }

        // Si hay documentos presentados, agregar al resultado
        if (documentosPresentadosPorCategoria.Any())
        {
          resultado.Add(new ReadEstudiantesConDocumentosPresentadosPorCategoriaDTO
          {
            IdEstudiante = estudiante.Id,
            NombreCompleto = $"{estudiante.Nombre} {estudiante.Apellido}",
            Registro = estudiante.NroRegistro,
            Email = estudiante.Email,
            DocumentosPresentados = documentosPresentadosPorCategoria
          });
        }
      }

      return resultado;
    }



    public async Task<ReadEstudianteDTO> ObtenerEstudiantePorId(int id)
    {
      Estudiante estudiante = await _context.Estudiantes.FirstOrDefaultAsync(x => x.Id == id);
      if (estudiante == null)
      {
        throw new ApplicationException("Estudiante no encontrado");
      }
      return new ReadEstudianteDTO
      {
        Id = estudiante.Id,
        IdCarrera = estudiante.IdCarrera,
        Nombre = estudiante.Nombre,
        Apellido = estudiante.Apellido,
        NroRegistro = estudiante.NroRegistro,
        FechaNacimiento = estudiante.FechaNacimiento,
        Estado = estudiante.Estado,
        Telefono = estudiante.Telefono,
        Email = estudiante.Email
      };
    }

    public async Task<List<ReadEstudianteDTO>> ObtenerEstudiantes()
    {
      List<Estudiante> estudiantes = await _context.Estudiantes
        .Include(x => x.Carrera)
        .ToListAsync();
      return estudiantes.Select(x => new ReadEstudianteDTO
      {
        Id = x.Id,
        IdCarrera = x.IdCarrera,
        Carrera = x.Carrera.Nombre,
        Nombre = x.Nombre,
        Apellido = x.Apellido,
        NroRegistro = x.NroRegistro,
        FechaNacimiento = x.FechaNacimiento,
        Estado = x.Estado,
        Telefono = x.Telefono,
        Email = x.Email
      }).ToList();
    }
  }
}
