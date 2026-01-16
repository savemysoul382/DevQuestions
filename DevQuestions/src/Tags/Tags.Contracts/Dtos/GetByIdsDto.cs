// Tags

namespace Tags.Contracts.Dtos;

public record GetByIdsDto(Guid[] Ids);

public record TagDto(Guid Id, string Name);