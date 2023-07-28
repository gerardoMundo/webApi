using AutoMapper;
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
            var book = await context.Books
                .Include(bookBD => bookBD.AuthorsBooks)
                .ThenInclude(authorsBooksBD => authorsBooksBD.Authors)
                .FirstOrDefaultAsync(bookBD => bookBD.Id == id);
            
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
            if (bookDTO.AuthorsIds == null) { return BadRequest("No se puede agregar un libro sin autor"); }

            var authorsIds = await context.Authors.Where(authorBD => bookDTO.AuthorsIds.Contains(authorBD.Id)
            ).Select(author => author.Id).ToListAsync();

            if(authorsIds.Count != bookDTO.AuthorsIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var book = mapper.Map<Books>(bookDTO);

            if(book.AuthorsBooks != null)
            {
                for (int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Sort = i;
                }
            }

            context.Add(book);
            await context.SaveChangesAsync();
            return Ok(book);
        }
    }
}
