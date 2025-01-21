using Microsoft.EntityFrameworkCore;
using SmsApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicione o serviço de autorização
builder.Services.AddAuthorization();

// Configuração de CORS
builder.Services.AddCors(options => {

    options.AddPolicy("DevPolicy", policy =>{

        policy.WithOrigins("http://127.0.0.1:5500")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });

});

// Configuração do banco de dados MYSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))));

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment()){
app.UseSwagger();
app.UseSwaggerUI();
}

// Remova ou comente esta linha
//app.UseHttpsRedirection();

app.UseCors("DevPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();
