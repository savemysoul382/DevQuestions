using DevQuestions.Web;
using DevQuestions.Web.Middlewares;
using DevQuestions.Web.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DevQuestions"));
}

app.MapControllers();
await app.UseSeedersAsync();
app.Run();