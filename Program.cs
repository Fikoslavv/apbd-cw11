using Microsoft.EntityFrameworkCore;

namespace apbd_cw11;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<apbd_cw11.Database.ClinicDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("mssql.pjwstk.edu.pl")));

        builder.Services.AddScoped<DBService>();

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
