using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DBContext;
using WebApi.DTOs;
using WebApi.Entities;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<AuthorWithID>>> Get()
        {
            var authors = await context.Authors.ToListAsync();
            return mapper.Map<List<AuthorWithID>>(authors);
        }

        [HttpGet("{id:int}", Name = "getAuthorById")] // ("{id:int/param2?/param3=consola}") varibles de ruta: opcionales y por default
        public async Task<ActionResult<AuthorDTOWithBooks>> Get(int id)
        {
            var author = await context.Authors.
                Include(authorsDB => authorsDB.AuthorsBooks).
                ThenInclude(authorsBooksDB => authorsBooksDB.Books).
                FirstOrDefaultAsync(author => author.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return mapper.Map<AuthorDTOWithBooks>(author);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<AuthorWithID>>> Get(string name)
        {
            var authors = await context.Authors.Where(author => author.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AuthorWithID>>(authors);
        }

        // Rutas:
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AuthorDTO authorDTO)
        {
            var isDuplicatedAuthor = await context.Authors.AnyAsync(x => x.Name == authorDTO.Name);

            if(isDuplicatedAuthor)
            {
                return BadRequest($"El autor con el nombre {authorDTO.Name} ya existe.");
            }

            var author = mapper.Map<Author>(authorDTO);//DTO generado con automapper

            context.Add(author);
            await context.SaveChangesAsync();

            var authorWithID = mapper.Map<AuthorWithID>(author);

            return CreatedAtRoute("getAuthorById", new {author.Id}, authorWithID);
        }


        
        [HttpGet("first")]
        public async Task<Author> FirstAuthor()
        {
            return await context.Authors.FirstOrDefaultAsync();// Buscar un autor, el primero
        }

        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AuthorDTO authorDTO, int id)
        {
            var exist = await context.Authors.AnyAsync(author => author.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            var author = mapper.Map<Author>(authorDTO);
            author.Id = id;
            context.Update(author);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete( int id)
        {
            var exists = context.Authors.AnyAsync(author =>  author.Id == id);

            if (!await exists) 
            {
            return NotFound();
            }

            context.Remove(new Author { Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
