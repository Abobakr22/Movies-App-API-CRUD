using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MoviesApp.Models;
using MoviesApp.Services;
using static System.Net.Mime.MediaTypeNames;

namespace MoviesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //configuring connection string for accessing database
            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));


            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();                     //enable cors
            //builder.Services.AddSwaggerGen();

            //you can provide an authorization token with jwt
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(  "Bearer",  new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your jwt key"
                });
            });

            builder.Services.AddTransient<IGenresService, GenresService>();    //add service for genres interface
            builder.Services.AddTransient<IMoviesService , MoviesService>();    //add service for movies interface


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}










// you can provide a configuaration for your api on swagger

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc(name: "v1", info: new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Version = "v1",
//        Title = "TestApi",
//        Description = "this is my first Api",

//        Contact = new Microsoft.OpenApi.Models.OpenApiContact
//        {Name = "Microsoft", Email = "Microsoft"},

//        License = new Microsoft.OpenApi.Models.OpenApiLicense
//        { Name = "my license" }
//    });





//Tools > Options > Text Editor > C# > Advanced > Inline Hints