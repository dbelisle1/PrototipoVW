using PrototipoVWApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<UsuariosRepository>();
builder.Services.AddScoped<PropuestasRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();