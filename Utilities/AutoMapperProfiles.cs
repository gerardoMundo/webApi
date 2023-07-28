﻿using AutoMapper;
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

            CreateMap<BookDTO, Books>()// para solicitudes post
                .ForMember(book => book.AuthorsBooks, options => options.MapFrom(MapAuthors));
            // para método get
            CreateMap<Books, BookWithID>()
                .ForMember(bookWithID => bookWithID.Authors, options => options.MapFrom(MapBookWithID));            

            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentWithID>();
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