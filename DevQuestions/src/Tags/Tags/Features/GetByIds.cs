// Tags

using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Tags.Contracts.Dtos;
using Tags.Database;

namespace Tags.Features;

public sealed class GetByIds
{
    public record GetByIdsQuery(GetByIdsDto Dto) : IQuery;

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // У Get метода нет Body, тк нет доступа к Body, а id может быть много и строка query будет огромной
            // app.MapGet("tags", handler: HandleAsync);
            // в документации пометить, что этот метод возвращает список id
            app.MapPost(
                "tags/ids",
                handler: async (
                    GetByIdsDto dto,
                    IQueryHandler<IReadOnlyList<TagDto>, GetByIdsQuery> handler,
                    CancellationToken ct) =>
                {
                    var result = await handler.HandleAsync(new GetByIdsQuery(Dto: dto), cancellationToken: ct);

                    return Results.Ok(value: result);
                });
        }
    }

    public sealed class Handler : IQueryHandler<IReadOnlyList<TagDto>, GetByIdsQuery>
    {
        private readonly TagsDbContext _tagsDbContext;

        public Handler(TagsDbContext tagsDbContext)
        {
            _tagsDbContext = tagsDbContext;
        }

        // [FromBody] можем не писать, тк возмется из тела автоматически
        public async Task<IReadOnlyList<TagDto>> HandleAsync(
            GetByIdsQuery query,
            CancellationToken ct)
        {
            var tags = await _tagsDbContext.Tags.Where(t => query.Dto.Ids.Contains(t.Id)).ToListAsync(cancellationToken: ct);
            return tags.Select(t => new TagDto(Id: t.Id, Name: t.Name)).ToList();
        }
    }
}