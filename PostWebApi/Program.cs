using MongoDB.Driver;
using PostWebApiCommon;
using PostWebApiCommon.Helpers;
using PostWebApiService.Services;
using Serilog;
using Serilog.Events;

namespace PostWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            Log.Logger = new LoggerConfiguration()
                            .ReadFrom
                            .Configuration(builder.Configuration).WriteTo.Logger(lc => lc
                            .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information || evt.Level == LogEventLevel.Error || evt.Level == LogEventLevel.Fatal)
                            .WriteTo.File(builder.Configuration.GetSection("Serilog:WriteTo:0:Args:path").Value))
                            .WriteTo.Logger(lc => lc
                            .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error || evt.Level == LogEventLevel.Fatal)
                            .WriteTo.File(builder.Configuration.GetSection("Serilog:WriteTo:1:Args:path").Value))
                            .CreateLogger();
            builder.Host.UseSerilog();

            try
            {
                Log.Information("Starting the Post API");

                var mongoConnectionString = builder.Configuration.GetConnectionString("PostDefaultConnection");
                var identityAPIBaseUrl = builder.Configuration[Constants.IdentityAPIBaseUrl];

                builder.Services.AddHttpClient<HttpClientHelper>(client =>
                {
                    client.BaseAddress = new Uri(identityAPIBaseUrl);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
                builder.Services.AddScoped(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();
                    return client.GetDatabase("SocialMediaDb");
                });
                builder.Services.AddScoped<IPostService, PostService>();
                builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

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
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}