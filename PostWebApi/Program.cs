using MongoDB.Driver;
using PostWebApiService.Services;

namespace PostWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var mongoConnectionString = builder.Configuration.GetConnectionString("PostDefaultConnection");

            builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
            builder.Services.AddScoped(sp => {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("SocialMediaDb");
            });
            builder.Services.AddScoped<IPostService, PostService>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi(); 
                app.UseStaticFiles();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "PostWebApi v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}