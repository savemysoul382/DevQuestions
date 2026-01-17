// Tags.Presenters

using Shared.Abstractions;
using Tags.Contracts;
using Tags.Contracts.Dtos;
using Tags.Database;
using Tags.Features;
using static Tags.Features.GetByIds;

namespace Tags.Presenters;

public class TagsModuleContract : ITagsContract
{
    private readonly IQueryHandler<IReadOnlyList<TagDto>, GetByIdsQuery> _handler;
    private readonly TagsDbContext _dbContext;

    public TagsModuleContract(
        IQueryHandler<IReadOnlyList<TagDto>, GetByIdsQuery> handler,
        TagsDbContext dbContext)
    {
        _handler = handler;
        _dbContext = dbContext;
    }

    public async Task CreateTag(CreateTagDto dto)
    {
        await Create.Handle(dto, _dbContext);
    }

    public async Task<IReadOnlyList<TagDto>> GetByIds(GetByIdsDto dto)
    {
        return await _handler.Handle(new GetByIdsQuery(dto));
    }
}