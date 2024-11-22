using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Identity;

namespace Gestion_Del_Presupuesto.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ConvenioModel> Convenios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Historial_Actividad> Historial_Actividad { get; set; }
        public DbSet<Institucion_Salud> InstitucionesSalud { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PresupuestoModel> Presupuestos { get; set; }
        public DbSet<RetribucionModel> Retribuciones { get; set; }
        public DbSet<Solicitud_Retribucion> SolicitudesRetribucion { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Devengado> Devengados { get; set; }
        public DbSet<Costo> Costo { get; set; }
        public DbSet<FacturacionModel> Facturacion { get; set; }
        public DbSet<CentroSaludModel> CentrosDeSalud { get; set; }
        public DbSet<ProvisionModel> Provision { get; set; }
        public DbSet<PlanillasModel> Planillas { get; set; }
        public DbSet<IndicadorEconomico> IndicadorEcono { get; set; }
        public DbSet<CarreraModel> Carreras { get; set; }
        public DbSet<SeriesData> SerieDatas { get; set; }
        public DbSet<ObsData> ObsDatas { get; set; }
        public DbSet<RegistroModel> Registro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Necesario para Identity

            // Relación Historial_Actividad → Usuario
            modelBuilder.Entity<Historial_Actividad>()
                .HasOne(h => h.Usuarios)
                .WithMany(u => u.Historial_Actividades)
                .HasForeignKey(h => h.Id_Usuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Estudiante → Planilla
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Planilla)
                .WithOne(p => p.Estudiante)
                .HasForeignKey<PlanillasModel>(p => p.Id_Estudiante)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Estudiante → Carrera
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Carrera)
                .WithMany(c => c.Estudiantes)
                .HasForeignKey(e => e.CarreraId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Estudiante → Convenio
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Convenio)
                .WithMany(c => c.Estudiantes)
                .HasForeignKey(e => e.Id_Convenio)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación PlanillasModel → Carrera
            modelBuilder.Entity<PlanillasModel>()
                .HasOne(p => p.Carrera)
                .WithMany(c => c.Planillas)
                .HasForeignKey(p => p.CarreraId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación PlanillasModel → FacturacionModel
            modelBuilder.Entity<PlanillasModel>()
                .HasOne(p => p.Facturacion)
                .WithMany(f => f.Planillas)
                .HasForeignKey(p => p.FacturacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación PlanillasModel → Devengado
            modelBuilder.Entity<PlanillasModel>()
                .HasOne(p => p.Devengado)
                .WithMany(d => d.Planillas)
                .HasForeignKey(p => p.Id_Planillas)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación PlanillasModel → IndicadorEconomico
            modelBuilder.Entity<PlanillasModel>()
                .HasOne(p => p.Indicador)
                .WithMany()
                .HasForeignKey(p => p.Id_IndicadorEco)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Devengado → Convenio
            modelBuilder.Entity<Devengado>()
                .HasOne(d => d.Convenio)
                .WithMany(c => c.Devengados)
                .HasForeignKey(d => d.ConvenioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Devengado → PlanillasModel
            modelBuilder.Entity<Devengado>()
                .HasMany(d => d.Planillas)
                .WithOne(p => p.Devengado)
                .HasForeignKey(p => p.Id_Planillas)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación FacturacionModel → Convenio
            modelBuilder.Entity<FacturacionModel>()
                .HasOne(f => f.Convenios)
                .WithMany(c => c.Facturacion)
                .HasForeignKey(f => f.ConvenioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Registro) 
                .WithOne(r => r.Usuarios) 
                .HasForeignKey<RegistroModel>(r => r.Id_Usuarios);

            modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithOne(r => r.Usuarios)
            .HasForeignKey<Usuario>(u => u.Id_Rol)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

