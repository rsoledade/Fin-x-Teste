namespace Finx.Api.Configuration
{
    public static class CacheConfiguration
    {
        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = configuration["Redis:Configuration"];
            
            if (!string.IsNullOrWhiteSpace(redisConfig))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConfig;
                });
            }

            return services;
        }
    }
}
