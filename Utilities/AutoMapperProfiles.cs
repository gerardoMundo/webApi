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

            CreateMap<BookDTO, Books>(); // para solicitudes post
            CreateMap<Books, BookWithID>();// para método get

            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentWithID>();
        }
    }
}
