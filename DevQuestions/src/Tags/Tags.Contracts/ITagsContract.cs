// Tags.Contracts

using Tags.Contracts.Dtos;

namespace Tags.Contracts;

public interface ITagsContract
{
    Task<IReadOnlyList<TagDto>> GetByIds(GetByIdsDto dto);
}