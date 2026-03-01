using Microsoft.EntityFrameworkCore;
using back.bbdd;

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
builder.Services.AddSwaggerGen();

//dbcontext
// builder.Services.AddDbContext<PeluqueriaDbContext>(options =>
//     options.UseMySql(
//         builder.Configuration.GetConnectionString("DefaultConnection"),
//         ServerVersion.AutoDetect(
//             builder.Configuration.GetConnectionString("DefaultConnection")
//         )
//     )
// );

builder.Services.AddDbContext<PeluqueriaDbContext>(options =>
    options.UseMySql(
        "server=localhost;database=peluqueria;user=peluqueria;password=password;",
        ServerVersion.AutoDetect(
            "server=localhost;database=peluqueria;user=peluqueria;password=password;"
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
