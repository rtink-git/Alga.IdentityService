using System.Reflection;

namespace Alga.IdentityService.Infrastructure;

public static class EndpointDefinitionExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                typeof(IEndpointDefinition).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = (IEndpointDefinition)Activator.CreateInstance(endpoint)!;
            instance.MapEndpoints(app);
        }
    }
}
