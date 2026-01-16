using DevQuestions.Web;
using DevQuestions.Web.Middlewares;
using DevQuestions.Web.Seeders;
using Framework;
using Tags;

var builder = WebApplication.CreateBuilder(args: args);

builder.Services.AddProgramDependencies();

builder.Services.AddEndpoints(TagsAssembly.Assembly);

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DevQuestions"));
}

app.MapControllers();
await app.UseSeedersAsync();

app.MapEndpoints();

app.Run();