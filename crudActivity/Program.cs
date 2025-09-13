using crudActivity.Dtos;
using crudActivity.Services;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("Mongo"));
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<ActivityRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FE", p =>
        p.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000" })
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("FE");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// --- Seed (กดครั้งแรกมีข้อมูล mock) ---
await crudActivity.Seed.SeedData.RunAsync(app.Services);

app.Run();
