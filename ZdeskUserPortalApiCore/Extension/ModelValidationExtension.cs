using Microsoft.AspNetCore.Mvc;
using ZdeskUserPortalApiCore.Filter;

namespace ZdeskUserPortalApiCore.Extension
{
    public static class ModelValidationExtension
    {
        public static IServiceCollection UseModelValidationHandler(this IServiceCollection services)
        {

            #region Model Validation

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelHelperAttribute));
            });

            #endregion

            return services;
        }
    }
}
