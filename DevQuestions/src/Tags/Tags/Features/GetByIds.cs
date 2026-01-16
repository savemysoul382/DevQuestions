// Tags

using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Shared.FullTextSearch;
using Tags.Contracts.Dtos;
using Tags.Database;

namespace Tags.Features;

public sealed class GetByIds
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // У Get метода нет Body, тк нет доступа к Body, а id может быть много и строка query будет огромной
            // app.MapGet("tags", handler: HandleAsync);
            // в документации пометить, что этот метод возвращает список id
            app.MapPost("tags/ids", handler: HandleAsync);
        }
    }

    // [FromBody] можем не писать, тк возмется из тела автоматически
    private static async Task<IResult> HandleAsync(
        Guid id,
        [FromBody] GetByIdsDto dto,
        TagsDbContext tagsDbContext,
        ISearchProvider searchProvider,
        CancellationToken ct)
    {
        var tags = await tagsDbContext.Tags.Where(t => dto.Ids.Contains(t.Id)).ToListAsync(cancellationToken: ct);
        return Results.Ok(value: tags.Select(t => new TagDto(Id: t.Id, Name: t.Name)));
    }
}