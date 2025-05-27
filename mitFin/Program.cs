using MitFin_Api.Inventory;
using MitFin_Api.Inventory.Interface;
using MitFin_Api.Inventory.Reposatory;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<MitFin_Api.DBAccess.DBConnection>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection for repositories
builder.Services.AddScoped<MaterialInterface, MaterialRepository>();
builder.Services.AddScoped<RegionMaterialInterface, RegionWiseMaterialRepository>();
builder.Services.AddScoped<MaterialInfoInterface, MaterialInfoRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
