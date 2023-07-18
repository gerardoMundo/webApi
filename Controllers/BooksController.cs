﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DBContext;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookWithID>> Get(int id)
        {
            var book = await context.Books.Include(bookBD => bookBD.Comments)
                .FirstOrDefaultAsync(author => author.Id == id);
            return mapper.Map<BookWithID>(book);
        }

        [HttpGet]
        public async Task<ActionResult<List<BookWithID>>> Get()
        {
            var books = await context.Books.ToListAsync();
            return mapper.Map<List<BookWithID>>(books);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookDTO bookDTO)
        {
            var book = mapper.Map<Books>(bookDTO);

            context.Add(book);
            await context.SaveChangesAsync();
            return Ok(book);
        }
    }
}
