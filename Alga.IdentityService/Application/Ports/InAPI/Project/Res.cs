namespace Alga.IdentityService.Application.Ports.InAPI.Project;

public record class Res(
    Guid userId,
    IReadOnlyList<Guid>? projectIds
);