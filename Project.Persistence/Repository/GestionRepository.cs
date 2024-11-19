using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.Gestion;
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
  public class GestionRepository : IGestionRepository
  {
    private readonly DocumentalManagement _context;
    public GestionRepository(DocumentalManagement context) {
      _context = context;
    }
    public async Task ActualizarGestion(UpdateGestionDTO gestion)
    {
      Gestion gestionEntity = await _context.Gestiones.FirstOrDefaultAsync(x => x.Id == gestion.Id);
      if (gestionEntity == null)
      {
        throw new ApplicationException("Gestion no encontrada");
      }
      gestionEntity.AnioGestion = gestion.AnioGestion;
      gestionEntity.CorreoInstitucion = gestion.CorreoInstitucion;
      gestionEntity.FechaPrimerSemestreFin = gestion.FechaPrimerSemestreFin;
      gestionEntity.FechaPrimerSemestreInicio = gestion.FechaPrimerSemestreInicio;
      gestionEntity.FechaSegundoSemestreFin = gestion.FechaSegundoSemestreFin;
      gestionEntity.FechaSegundoSemestreInicio = gestion.FechaSegundoSemestreInicio;
      _context.Gestiones.Update(gestionEntity);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarGestion(int id)
    {
      Gestion gestion = await _context.Gestiones.FirstOrDefaultAsync(x => x.Id == id);
      if (gestion == null)
      {
        throw new ApplicationException("Gestion no encontrada");
      }
      _context.Gestiones.Remove(gestion);
      await _context.SaveChangesAsync();
    }

    public async Task GuardarGestion(CreateGestionDTO gestion)
    {
      Gestion gestionEntity = new Gestion {
        AnioGestion = gestion.AnioGestion,
        CorreoInstitucion = gestion.CorreoInstitucion,
        FechaPrimerSemestreFin = gestion.FechaPrimerSemestreFin,
        FechaPrimerSemestreInicio = gestion.FechaPrimerSemestreInicio,
        FechaSegundoSemestreFin = gestion.FechaSegundoSemestreFin,
        FechaSegundoSemestreInicio = gestion.FechaSegundoSemestreInicio
      };
      _context.Gestiones.Add(gestionEntity);
      await _context.SaveChangesAsync();
    }

    public async Task<List<ReadGestionDTO>> ObtenerGestiones()
    {
      List<Gestion> gestiones = await _context.Gestiones.ToListAsync();
      List<ReadGestionDTO> gestionesDTO = new List<ReadGestionDTO>();
      gestiones.ForEach(gestion =>
      {
        gestionesDTO.Add(new ReadGestionDTO
        {
          Id = gestion.Id,
          AnioGestion = gestion.AnioGestion,
          CorreoInstitucion = gestion.CorreoInstitucion,
          FechaPrimerSemestreFin = gestion.FechaPrimerSemestreFin,
          FechaPrimerSemestreInicio = gestion.FechaPrimerSemestreInicio,
          FechaSegundoSemestreFin = gestion.FechaSegundoSemestreFin,
          FechaSegundoSemestreInicio = gestion.FechaSegundoSemestreInicio
        });
      });
      return gestionesDTO;
    }

    public async Task<ReadGestionDTO> ObtenerGestionPorId(int id)
    {
      Gestion gestion = await _context.Gestiones.FirstOrDefaultAsync(x => x.Id == id);
      if (gestion == null)  
      {
        throw new ApplicationException("Gestion no encontrada");
      }
      return new ReadGestionDTO
      {
        Id = gestion.Id,
        AnioGestion = gestion.AnioGestion,
        CorreoInstitucion = gestion.CorreoInstitucion,
        FechaPrimerSemestreFin = gestion.FechaPrimerSemestreFin,
        FechaPrimerSemestreInicio = gestion.FechaPrimerSemestreInicio,
        FechaSegundoSemestreFin = gestion.FechaSegundoSemestreFin,
        FechaSegundoSemestreInicio = gestion.FechaSegundoSemestreInicio
      };
    }
  }
}
