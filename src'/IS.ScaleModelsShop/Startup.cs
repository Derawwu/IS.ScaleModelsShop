using IS.ScaleModelsShop.API.Constants;
using IS.ScaleModelsShop.API.HealthChecks;
using IS.ScaleModelsShop.API.SwaggerConfiguration;
using IS.ScaleModelsShop.Application.Extensions;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using IS.ScaleModelsShop.API.SlugifyParameters;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using IS.ScaleModelsShop.API.Middleware;
using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Providers;
using IS.ScaleModelsShop.API.Responses;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IS.ScaleModelsShop.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/> instance, containing application configurations.</param>
        /// <param name="environment"><see cref="IWebHostEnvironment"/> instance.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Configures application services.
        /// </summary>
        /// <param name="services">A collection of application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ServiceCollectionDIExtension).Assembly));

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder =>
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddSwaggerConfiguration();

            services.AddMvc().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors
                            .Select(
                                e =>
                                {
                                    var propertyName = x.Key;
                                    return new ValidationFailure(propertyName, e.ErrorMessage)
                                    {
                                        ErrorCode = StatusCodes.Status400BadRequest.ToString()
                                    };
                                }));

                    throw new ValidationException(errors);
                };
            });

            services.AddControllers(options =>
            {
                options.RequireHttpsPermanent = true;
                options.Conventions.Add(new RouteTokenTransformerConvention(new ParameterTransformer()));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddEndpointsApiExplorer();

            services.AddApplicationLayer();
            services.AddInfrastructureLayer(configuration);

            services.AddSingleton<IJsonResponseProvider<ExceptionResponse>, JsonResponseProvider<ExceptionResponse>>();
            services.AddSingleton<IResponseHandler, ResponseHandler>();

            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();
        }

        /// <summary>
        /// Configures API.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>instance.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorsHandlingMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IS.ScaleModelsShop.API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Open");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks(RequestConstants.HealthCheckRequestUri, new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        }
    }
}