using Microsoft.EntityFrameworkCore;
using Serilog;
using Villa_API;
using Villa_API.Data;
using Villa_API.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day ).CreateLogger();
// builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});// connect to database
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers(
    //  option => option.ReturnHttpNotAcceptable = true
    ).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
