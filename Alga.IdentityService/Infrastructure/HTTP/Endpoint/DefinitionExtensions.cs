using System.Reflection;

namespace Alga.IdentityService.Infrastructure.HTTP.Endpoint;

public static class DefinitionExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                typeof(IDefinition).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = (IDefinition)Activator.CreateInstance(endpoint)!;
            instance.MapEndpoints(app);
        }
    }
}
