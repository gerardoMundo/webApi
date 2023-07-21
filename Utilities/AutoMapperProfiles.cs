using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthorDTO, Author>();
            CreateMap<Author, AuthorWithID>();

            CreateMap<BookDTO, Books>()
                .ForMember(book => book.Authors, options => options.MapFrom(MapAuthors));// para solicitudes post
            CreateMap<Books, BookWithID>();// para método get

            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentWithID>();
        }

        private List<AuthorsBooks> MapAuthors(BookDTO bookDTO, Books books ) 
        {
            var result = new List<AuthorsBooks>();

            if( bookDTO.AuthorsIds != null )
            {
                return result;
            }

            foreach( var authorId in bookDTO.AuthorsIds) 
            {
                 result.Add(new AuthorsBooks { AuthorId = authorId });
            }

            return result;
        }
    }
}
