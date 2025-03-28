using Microsoft.EntityFrameworkCore;
using ProjectBackend.Data;
using ProjectBackend.Models;
using ProjectBackend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtenha a chave de configuração JWT
var jwtKey = builder.Configuration["Jwt:Key"];

// Registre o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionando o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registre o serviço ITaskItemService e sua implementação TaskItemService
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

// Adicionando os controladores
builder.Services.AddControllers();

// Verifique se a chave é nula ou vazia antes de usá-la
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("A chave JWT não foi configurada corretamente.");
}

// Adicionando JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Registre os serviços IUserService, JwtService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
// Registrando o serviço de registro
builder.Services.AddScoped<UserRegistrationService>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Habilita o Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

// Configurações do pipeline de requisições
app.UseAuthentication(); // Necessário para autenticação JWT
app.UseHttpsRedirection(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
