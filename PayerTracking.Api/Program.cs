using PayerTracking.Library;
using PayerTracking.Library.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var apiDocs = $@"{AppDomain.CurrentDomain.BaseDirectory}\PayerTracking.Api.xml";
    if (File.Exists(apiDocs))
    {
        c.IncludeXmlComments(apiDocs);
    }
});
builder.Services.AddSingleton<IDataAccess, MemoryDataAccess>();
builder.Services.AddTransient<Orchestrator, Orchestrator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
