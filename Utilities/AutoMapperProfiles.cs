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
            CreateMap<Author, AuthorDTOWithBooks>()
                .ForMember(author => author.Books, options => options.MapFrom(MapAuthorsWithBooks));

            CreateMap<BookDTO, Books>()// para solicitudes post
                .ForMember(book => book.AuthorsBooks, options => options.MapFrom(MapAuthors));
            // para método get
            CreateMap<Books, BookWithID>();
            CreateMap<Books, BookDTOWithAuthors>()
                .ForMember(bookWithID => bookWithID.Authors, options => options.MapFrom(MapBookWithID));
            CreateMap<BookPatchDTO, Books>().ReverseMap();

            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentWithID>();
        }

        private List<BookWithID> MapAuthorsWithBooks(Author author, AuthorWithID authorWithID)
        {
            var result = new List<BookWithID>();

            if(author.AuthorsBooks == null) { return result; }

            foreach (var authorBook in author.AuthorsBooks)
            {
                result.Add(new BookWithID()
                {
                    Id = authorBook.BooksId,
                    Title = authorBook.Books.Title,
                });
            }

            return result;
        }
        private List<AuthorWithID> MapBookWithID(Books books, BookWithID bookWithID)
        {
            var result = new List<AuthorWithID>();

            if(books.AuthorsBooks == null) { return result; }

            foreach(var authorBooks in books.AuthorsBooks)
            {
                result.Add(new AuthorWithID()
                {
                    Id = authorBooks.AuthorId,
                    Name = authorBooks.Authors.Name
                });
            }

            return result;
        }

        private List<AuthorsBooks> MapAuthors(BookDTO bookDTO, Books books ) 
        {
            var result = new List<AuthorsBooks>();

            if( bookDTO.AuthorsIds == null )
            {
                return result;
            }

            foreach( var authorId in bookDTO.AuthorsIds) 
            {
                 result.Add(new AuthorsBooks() { AuthorId = authorId });
            }

            return result;
        }
    }
}
