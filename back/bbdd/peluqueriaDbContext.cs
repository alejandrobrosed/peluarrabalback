// puente entre la bbdd y c#
using Microsoft.EntityFrameworkCore;
using back.modelos;
using back.controladores;

namespace back.bbdd;

public class PeluqueriaDbContext: DbContext
{
    public PeluqueriaDbContext(DbContextOptions<PeluqueriaDbContext> opciones)
        : base(opciones){ }

    public DbSet<Servicio>Servicios { get; set; }
    public DbSet<Reserva>Reservas {get; set;}
    public DbSet<Empleado>Empleados {get; set;}
    public DbSet<Usuario>Usuarios {get; set;}
    public DbSet<Horario>Horarios {get; set;}
    public DbSet<Producto>Productos {get; set;}
    public DbSet<Venta>Ventas {get; set;}
    public DbSet<VentaDetalle>VentaDetalles {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("usuarios");
        modelBuilder.Entity<Servicio>().ToTable("servicios");
        modelBuilder.Entity<Producto>().ToTable("productos");
        modelBuilder.Entity<Empleado>().ToTable("empleados");
        modelBuilder.Entity<Horario>().ToTable("horarios");
        modelBuilder.Entity<Reserva>().ToTable("reservas");
        modelBuilder.Entity<Venta>().ToTable("ventas");
        modelBuilder.Entity<VentaDetalle>().ToTable("ventas_detalles");
    }
    
}

//poner los onmodelcreating con el nombre de la tabla en minuscula