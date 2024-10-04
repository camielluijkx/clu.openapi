using AutoMapper;

namespace clu.openapi.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            // domain model to dto
            CreateMap<Entities.Book, Models.Book>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => $"{src.Author.FirstName}"))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => $"{src.Author.LastName}"));

            CreateMap<Entities.Book, Models.BookWithConcatenatedAuthorName>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<Entities.Book, Models.BookWithAmountOfPages>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => $"test{src.Author.FirstName}"))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => $"test{src.Author.LastName}"))
                .ForMember(dest => dest.AmountOfPages, opt => opt.MapFrom(src => $"{src.AmountOfPages.Value}"));

            // dto to domain model
            CreateMap<Models.BookForCreation, Entities.Book>();

            CreateMap<Models.BookForCreationWithAmountOfPages, Entities.Book>();
        }
    }
}