using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "getBookByID")]
        public async Task<ActionResult<BookDTOWithAuthors>> Get(int id)
        {
            var book = await context.Books
                .Include(bookBD => bookBD.AuthorsBooks)
                .ThenInclude(authorsBooksBD => authorsBooksBD.Authors)
                .FirstOrDefaultAsync(bookBD => bookBD.Id == id);

            if(book == null) { return NotFound(); }

            book.AuthorsBooks = book.AuthorsBooks.OrderBy(x => x.Sort).ToList();
            
            return mapper.Map<BookDTOWithAuthors>(book);
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

            SortAuthorsBooksProperty(book);

            context.Add(book);
            await context.SaveChangesAsync();
            var bookWithId = mapper.Map<BookWithID>(book);
            return CreatedAtRoute("getBookByID", new {book.Id}, bookWithId);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, BookDTO bookDTO)
        {
            var bookDB = await context.Books.Include(x => x.AuthorsBooks)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bookDB == null) { return NotFound(); }          

            bookDB = mapper.Map(bookDTO, bookDB );
            SortAuthorsBooksProperty(bookDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
        {
            if(patchDocument == null) { return BadRequest(); }

            var bookDB = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (bookDB == null) { return NotFound(); }

            var bookDTO = mapper.Map<BookPatchDTO>(bookDB);

            patchDocument.ApplyTo(bookDTO, ModelState);

            var isValid = TryValidateModel(bookDTO);

            if(!isValid) { return BadRequest(ModelState); }

            mapper.Map(bookDTO, bookDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = context.Books.AnyAsync(book => book.Id == id);

            if (!await exists)
            {
                return NotFound();
            }

            context.Remove(new Books { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        private static void SortAuthorsBooksProperty(Books book)
        {
            if (book.AuthorsBooks != null)
            {
                for (int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Sort = i;
                }
            }
        }

    }
}