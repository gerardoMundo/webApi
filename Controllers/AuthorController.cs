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

        // Rutas:
        [HttpPost]
        public async Task<ActionResult> Post(AuthorDTO authorDTO)
        {
            var isDuplicatedAuthor = await context.Authors.AnyAsync(x => x.Name == authorDTO.Name);

            if(isDuplicatedAuthor)
            {
                return BadRequest($"El autor con el nombre {authorDTO.Name} ya existe.");
            }

            var author = mapper.Map<Author>(authorDTO);//DTO generado con automapper

            context.Add(author);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<AuthorWithID>>> Get()
        {
            var authors = await context.Authors.ToListAsync();
            return mapper.Map<List<AuthorWithID>>(authors);
        }

        [HttpGet("first")]
        public async Task<Author> FirstAuthor()
        {
            return await context.Authors.FirstOrDefaultAsync();// Buscar un autor, el primero
        }

        [HttpGet("{id:int}")] // ("{id:int/param2?/param3=consola}") varibles de ruta: opcionales y por default
        public async Task<ActionResult<AuthorWithID>> Get(int id)
        {
            var author = await context.Authors.FirstOrDefaultAsync(author => author.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return mapper.Map<AuthorWithID>(author);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<AuthorWithID>>> Get(string name)
        {
            var authors = await context.Authors.Where(author => author.Name.Contains(name)).ToListAsync();     

            return mapper.Map<List<AuthorWithID>>(authors);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Author author, int id)
        {
            var exists = context.Authors.AnyAsync(author => author.Id == id);

            if (!await exists)
            {
                return NotFound();
            }

            if (author.Id != id)
            {
                return BadRequest("El autor no coincide con el Id proporcionado");
            }

            context.Update(author);
            await context.SaveChangesAsync();
            return Ok();
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
