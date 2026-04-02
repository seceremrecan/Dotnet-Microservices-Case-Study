using Log.Application.Features.Logs.Commands.CreateLog;
using Log.Application.Features.Logs.Queries.GetLogs;
using Log.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogInfrastructure(builder.Configuration);

builder.Services.AddScoped<CreateLogCommandHandler>();
builder.Services.AddScoped<GetLogsQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();