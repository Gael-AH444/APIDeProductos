using APIProductos.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*Configurar DB context*/
builder.Services.AddDbContext<DbapicrudContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));


/*Configurando el controlador para eviarar las referencias ciclicas en el JSON */
builder.Services.AddControllers().AddJsonOptions(opt =>
{
	opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//***AGREGANDO CORS***
var misReglasCors = "ReglasCors";
builder.Services.AddCors(opt =>
{
	opt.AddPolicy(name: misReglasCors, builder =>
	{
		builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
	
//}

app.UseSwagger();
app.UseSwaggerUI();

//***ACTIVANDO CORS***
app.UseCors(misReglasCors);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
