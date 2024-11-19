using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.CategoriaDocumento;
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
  public class CategoriaDocumentoRepository : ICategoriaDocumentoRepository
  {
    private readonly DocumentalManagement _context;
    public CategoriaDocumentoRepository(DocumentalManagement context)
    {
      _context = context;
    }

    public async Task ActualizarCategoriaDocumento(UpdateCategoriaDocumentoDTO categoriaDocumento)
    {
      CategoriaDocumento categoriaDocumentoEntity = await _context.CategoriasDocumento.FirstOrDefaultAsync(x => x.Id == categoriaDocumento.Id);
      if (categoriaDocumentoEntity == null)
      {
        throw new ApplicationException("Categoria de documento no encontrada");
      }
      categoriaDocumentoEntity.Nombre = categoriaDocumento.Nombre;
      categoriaDocumentoEntity.Descripcion = categoriaDocumento.Descripcion;
      _context.CategoriasDocumento.Update(categoriaDocumentoEntity);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarCategoriaDocumento(int id)
    {
      CategoriaDocumento categoriaDocumento = await _context.CategoriasDocumento.FirstOrDefaultAsync(x => x.Id == id);
      if (categoriaDocumento == null)
      {
        throw new ApplicationException("Categoria de documento no encontrada");
      }
      _context.CategoriasDocumento.Remove(categoriaDocumento);
      await _context.SaveChangesAsync();
    }

    public async Task GuardarCategoriaDocumento(CreateCategoriaDocumentoDTO categoriaDocumento)
    {
      CategoriaDocumento categoriaDocumentoEntity = new CategoriaDocumento {
        Nombre = categoriaDocumento.Nombre,
        Descripcion = categoriaDocumento.Descripcion
      };
      _context.CategoriasDocumento.Add(categoriaDocumentoEntity);
      await _context.SaveChangesAsync();
    }

    public async Task<ReadCategoriaDocumentoDTO> ObtenerCategoriaDocumentoPorId(int id)
    {
      CategoriaDocumento categoriaDocumento = await _context.CategoriasDocumento.FirstOrDefaultAsync(x => x.Id == id);
      if (categoriaDocumento == null)
      {
        throw new ApplicationException("Categoria de documento no encontrada");
      }
      return new ReadCategoriaDocumentoDTO
      {
        Id = categoriaDocumento.Id,
        Nombre = categoriaDocumento.Nombre,
        Descripcion = categoriaDocumento.Descripcion
      };
    }

    public async Task<List<ReadCategoriaDocumentoDTO>> ObtenerCategoriasDocumento()
    {
      List<CategoriaDocumento> categoriasDocumento = await _context.CategoriasDocumento.ToListAsync();
      return categoriasDocumento.Select(x => new ReadCategoriaDocumentoDTO
      {
        Id = x.Id,
        Nombre = x.Nombre,
        Descripcion = x.Descripcion
      }).ToList();
    }
  }
}
