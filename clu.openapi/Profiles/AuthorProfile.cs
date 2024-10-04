using AutoMapper;

namespace clu.openapi.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            // domain model to dto
            CreateMap<Entities.Author, Models.Author>();
            
            // dto to domain model
            CreateMap<Models.AuthorForUpdate, Entities.Author>();
        }
    }
}