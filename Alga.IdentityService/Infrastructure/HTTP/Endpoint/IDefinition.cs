namespace Alga.IdentityService.Infrastructure.HTTP.Endpoint;

public interface IDefinition { ValueTask MapEndpoints(IEndpointRouteBuilder app); }