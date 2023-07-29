﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApi.DBContext;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<CommentWithID>>> Get(int bookId)
        {
            var bookExist = await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);
            var comments = await context.Comments.Where(commentDB => commentDB.BooksId == bookId).ToListAsync();

            if (!bookExist)
            {
                return NotFound();
            }

            return mapper.Map<List<CommentWithID>>(comments);
        }

        [HttpGet("{id:int}", Name = "getCommentByID")]
        public async Task<ActionResult<CommentWithID>> GetByID(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(commentDB => commentDB.Id == id);

            if(comment == null) { return NotFound(); }

            return mapper.Map<CommentWithID>(comment);
        }

        [HttpPost]
        public async Task<ActionResult> Post( int bookId, CommentDTO commentDTO ) 
        {
        var bookExist =  await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);

            if(!bookExist)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentDTO);
            comment.BooksId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var commentWithId = mapper.Map<CommentWithID>(comment);

            return CreatedAtRoute("getCommentByID", new { bookId, comment.Id }, commentWithId);
        }
    }
}
