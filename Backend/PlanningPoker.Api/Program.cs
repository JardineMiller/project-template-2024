using PlanningPoker.Api;
using PlanningPoker.Application;
using PlanningPoker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation()
    .AddCors(
        options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy
                        .WithOrigins("http://127.0.0.1:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            );
        }
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
