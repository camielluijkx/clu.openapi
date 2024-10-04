using AutoMapper;

namespace clu.openapi.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, Models.Author>();
            
            CreateMap<Models.AuthorForUpdate, Entities.Author>();
        }
    }
}