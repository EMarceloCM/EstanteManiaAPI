using AutoMapper;
using EstanteMania.API.DTO_s;
using EstanteMania.API.Identity_Entities;
using EstanteMania.API.Identity_Entities.DTO_s;
using EstanteMania.Models.Models;

namespace EstanteMania.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.CategorysBooks!.Select(b => b.Book))).ReverseMap();
            CreateMap<Book, BookDTO>().ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.CategorysBooks!.Select(c => c.Category))).ReverseMap();
            CreateMap<Author, AuthorDTO>().ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books)).ReverseMap();
            CreateMap<CategoryWithBook, CategoryWithBookDTO>().ReverseMap();
            CreateMap<AuthorWithBooks, AuthorWithBooksDTO>().ReverseMap();
            CreateMap<BookWithCategories, BookWithCategoriesDTO>().ReverseMap();
            CreateMap<BookWithAuthor, BookWithAuthorDTO>().ReverseMap();
            CreateMap<User, UserDTO>();
        }
    }
}