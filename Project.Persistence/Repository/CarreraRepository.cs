using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.Carrera;
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
  public class CarreraRepository : ICarreraRepository
  {
    private readonly DocumentalManagement _context;
    public CarreraRepository(DocumentalManagement context)
    {
      _context = context;
    }
    public async Task ActualizarCarrera(UpdateCarreraDTO facultad)
    {
      Carrera carreraEntity = await _context.Carreras.FirstOrDefaultAsync(x => x.Id == facultad.Id);
      if (carreraEntity == null)
      {
        throw new ApplicationException("Carrera no encontrada");
      }
      carreraEntity.Nombre = facultad.Nombre;
      carreraEntity.Descripcion = facultad.Descripcion;
      carreraEntity.IdFacultad = facultad.IdFacultad;
      _context.Carreras.Update(carreraEntity);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarCarrera(int id)
    {
      Carrera carrera = await _context.Carreras.FirstOrDefaultAsync(x => x.Id == id);
      if (carrera == null)
      {
        throw new ApplicationException("Carrera no encontrada");
      }
      _context.Carreras.Remove(carrera);
      await _context.SaveChangesAsync();
    }

    public async Task GuardarCarrera(CreateCarreraDTO carrera)
    {
      Carrera carreraEntity = new Carrera {
        Nombre = carrera.Nombre,
        Descripcion = carrera.Descripcion,
        IdFacultad = carrera.IdFacultad
      };
      _context.Carreras.Add(carreraEntity);
      await _context.SaveChangesAsync();
    }

    public async Task<ReadCarreraDTO> ObtenerCarreraPorId(int id)
    {
      Carrera carrera = await _context.Carreras.FirstOrDefaultAsync(x => x.Id == id);
      if (carrera == null)
      {
        throw new ApplicationException("Carrera no encontrada");
      }
      return new ReadCarreraDTO
      {
        Id = carrera.Id,
        Nombre = carrera.Nombre,
        Descripcion = carrera.Descripcion,
        IdFacultad = carrera.IdFacultad
      };
    }

    public async Task<List<ReadCarreraDTO>> ObtenerCarreras()
    {
      List<Carrera> carreras = await _context.Carreras.ToListAsync();
      List<ReadCarreraDTO> carrerasDTO = new List<ReadCarreraDTO>();
      foreach (Carrera carrera in carreras)
      {
        carrerasDTO.Add(new ReadCarreraDTO
        {
          Id = carrera.Id,
          Nombre = carrera.Nombre,
          Descripcion = carrera.Descripcion,
          IdFacultad = carrera.IdFacultad
        });
      }
      return carrerasDTO;
    }
  }
}
