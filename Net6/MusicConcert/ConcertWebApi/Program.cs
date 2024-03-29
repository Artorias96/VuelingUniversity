using Business.ServiceContracts;
using Business.ServiceRepository;
using InfrastructureData.RepositoryImplementations;
using RepositoryContracts;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

 
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddScoped<ICrowdSelectedService, CrowdSelectedService>();
builder.Services.AddScoped<IUnavaiableDaysRepository, UnavaiableDaysRepository>();
builder.Services.AddScoped<IMusiciansRepository, MusiciansRepository>();
builder.Services.AddScoped<IPeopleNeedForMeetingRepository, PeopleNeedForMeetingRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
