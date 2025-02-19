using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using BookStoreAPI.Model.Data;
namespace BookStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string conStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<BookDbContext>(options => {
                options.UseSqlServer(conStr);

            });

            builder.Services.AddControllers().AddXmlSerializerFormatters().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOData();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.EnableDependencyInjection();
                endpoints.Select().OrderBy().Filter().MaxTop(100).SkipToken().Count();

            });


            app.MapControllers();

            app.Run();
        }
    }
}
