using PrototipoVWApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<UsuariosRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();