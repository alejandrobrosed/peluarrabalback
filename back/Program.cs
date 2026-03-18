using Microsoft.EntityFrameworkCore;
using back.bbdd;
using back.servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//controladores
builder.Services.AddControllers();

//cors navegador
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<ReservasService>();
builder.Services.AddScoped<EmpleadosService>();
builder.Services.AddScoped<HorariosService>();
builder.Services.AddScoped<ServiciosService>();
builder.Services.AddScoped<VentasService>();
builder.Services.AddScoped<VentaDetallesService>();
builder.Services.AddScoped<AuthService>();

//dbcontext
builder.Services.AddDbContext<PeluqueriaDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVue");
app.UseAuthorization();
app.MapControllers();
app.Run();
