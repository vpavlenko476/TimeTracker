using Microsoft.Extensions.DependencyInjection;

namespace TimeTracker.Api.MapperProfiles
{
    /// <summary>
    /// Регистрация профилей в ServiceCollection
    /// </summary>
    public static class RegistrationExtension
    {
        /// <summary>
        /// Зарегистрировать профили маппера в ServiceCollection
        /// </summary>
        public static IServiceCollection AddMapperProfiles(this IServiceCollection services)
        {
            return services.AddAutoMapper(expression =>
            {
                expression.AllowNullCollections = true;
            }, typeof(RegistrationExtension).Assembly);
        }
    }
}