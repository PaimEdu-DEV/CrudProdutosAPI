using Microsoft.EntityFrameworkCore;
using APIExercicio.Data;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);
string SortVersion = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(SortVersion));


    // ✅ CORS adicionado — permite que o front-end acesse a API
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(); // ✅ Ativa o middleware CORS
app.UseAuthorization();
app.MapControllers();
app.Run();