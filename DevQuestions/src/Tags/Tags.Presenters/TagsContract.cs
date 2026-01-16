// Tags.Presenters

using Tags.Contracts;
using Tags.Contracts.Dtos;

namespace Tags.Presenters;

public class TagsContract : ITagsContract
{
    public async Task<IReadOnlyList<TagDto>> GetByIds(GetByIdsDto dto)
    {
        await Task.Delay(500);
        return dto.Ids.Select(id => new TagDto(Id: id, Name: $"Tag {id}")).ToList();
    }
}