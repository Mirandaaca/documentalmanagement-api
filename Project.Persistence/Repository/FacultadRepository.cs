using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.Facultad;
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
  public class FacultadRepository : IFacultadRepository
  {
    private readonly DocumentalManagement _context;
    public FacultadRepository(DocumentalManagement context)
    {
      _context = context;
    }
    public async Task ActualizarFacultad(UpdateFacultadDTO facultad)
    {
      Facultad facultadEntity = await _context.Facultades.FirstOrDefaultAsync(x => x.Id == facultad.Id);
      if (facultadEntity == null)
      {
        throw new ApplicationException("Estudiante no encontrado");
      }
      facultadEntity.Nombre = facultad.Nombre;
      facultadEntity.Descripcion = facultad.Descripcion;
      _context.Facultades.Update(facultadEntity);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarFacultad(int id)
    {
      Facultad facultad = _context.Facultades.FirstOrDefault(x => x.Id == id);
      if (facultad == null)
      {
        throw new ApplicationException("Facultad no encontrada");
      }
      _context.Facultades.Remove(facultad);
      await _context.SaveChangesAsync();
    }

    public async Task GuardarFacultad(CreateFacultadDTO facultad)
    {
      Facultad facultadEntity = new Facultad
      {
        Nombre = facultad.Nombre,
        Descripcion = facultad.Descripcion
      };
      _context.Facultades.Add(facultadEntity);
      await _context.SaveChangesAsync();
    }

    public async Task<List<ReadFacultadDTO>> ObtenerFacultades()
    {
      List<Facultad> facultades = await _context.Facultades.ToListAsync();
      List<ReadFacultadDTO> facultadesDTO = new List<ReadFacultadDTO>();
      foreach (Facultad facultad in facultades)
      {
        facultadesDTO.Add(new ReadFacultadDTO
        {
          Id = facultad.Id,
          Nombre = facultad.Nombre,
          Descripcion = facultad.Descripcion
        });
      }
      return facultadesDTO;
    }

    public async Task<ReadFacultadDTO> ObtenerFacultadPorId(int id)
    {
      Facultad facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.Id == id);
      if (facultad == null)
      {
        throw new ApplicationException("Facultad no encontrada");
      }
      return new ReadFacultadDTO
      {
        Id = facultad.Id,
        Nombre = facultad.Nombre,
        Descripcion = facultad.Descripcion
      };
    }
  }
}
