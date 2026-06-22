using BenchPulse.Application;
using BenchPulse.Api.Filters;
using BenchPulse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<CompanyDomainFilter>();
builder.Services.AddControllers(options =>
    options.Filters.Add<CompanyDomainFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BenchPulse API", Version = "v1" });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Supabase:Url"] + "/auth/v1";
        options.Audience = "authenticated";
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/problem+json";

        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error is not null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                title = "An unexpected error occurred.",
                status = 500,
                detail = app.Environment.IsDevelopment() ? error.Error.Message : "An internal server error has occurred."
            });
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");
app.UseAuthorization();
app.MapControllers();
app.Run();