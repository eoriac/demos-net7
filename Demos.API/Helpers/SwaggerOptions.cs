using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Demo.API.Helpers
{
    public class SwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider apiVersionProvider;

        public SwaggerOptions(
            IApiVersionDescriptionProvider provider)
        {
            this.apiVersionProvider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var versionDescription in apiVersionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(versionDescription.GroupName, CreateVersionInfo(versionDescription));
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "OpenAPI configuration sample",
                Description = "Extending configuration on OpenAPI UI with ASP.NET Core",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                },
                Version = desc.ApiVersion.ToString()
            };

            if (desc.IsDeprecated)
            {
                info.Description += "API version deprecated.";
            }

            return info;
        }
    }
}
