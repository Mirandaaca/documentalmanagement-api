using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Context
{
  public class DocumentalManagement : IdentityDbContext<Usuario>
  {
    public DocumentalManagement(DbContextOptions<DocumentalManagement> options) : base(options)
    {
    }
    public virtual DbSet<CategoriaDocumento> CategoriasDocumento { get; set; }
    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }
    public virtual DbSet<Facultad> Facultades { get; set; }
    public virtual DbSet<Carrera> Carreras { get; set; }
    public virtual DbSet<Estudiante> Estudiantes { get; set; }
    public virtual DbSet<Bitacora> Bitacoras { get; set; }
    public virtual DbSet<Gestion> Gestiones { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Documento> Documentos { get; set; }
  }
}
