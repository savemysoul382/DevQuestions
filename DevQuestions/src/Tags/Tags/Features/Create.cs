// Tags

using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tags.Contracts.Dtos;
using Tags.Database;
using Tags.Domain;

namespace Tags.Features;

public sealed class Create
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("tags", handler: Handle);
        }
    }

    public static async Task<IResult> Handle(CreateTagDto createTagDto, TagsDbContext tagsDbContext)
    {
        Tag tag = new Tag { Id = Guid.NewGuid(), Name = createTagDto.Name, };
        await tagsDbContext.AddAsync(entity: tag);

        return Results.Ok(value: tag.Id);
    }

    /*
   public sealed class CreateTagHandler : ICommandHandler<>
   {
       private readonly TagsDbContext _tagsDbContext;

       public CreateTagHandler(TagsDbContext tagsDbContext)
       {
           _tagsDbContext = tagsDbContext;
       }

       private async Task<IResult> Handle(CreateTagDto createTagDto, CancellationToken cancellationToken)
       {
           Tag tag = new Tag { Id = Guid.NewGuid(), Name = createTagDto.Name, };
           await tagsDbContext.AddAsync(entity: tag);

           return Results.Ok(value: tag.Id);
       }
   } */
}