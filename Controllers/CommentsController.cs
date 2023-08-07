using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<IdentityUser> userManager;

        public CommentsController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post( int bookId, CommentDTO commentDTO ) 
        {
        var emailClaims = HttpContext.User.Claims.Where(claim => claim.Type == "Email").FirstOrDefault();//Permite obtener valores del JWT
            var email = emailClaims.Value;
        var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;
        var bookExist =  await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);

            if(!bookExist)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentDTO);
            comment.UserId = userId;
            comment.BooksId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var commentWithId = mapper.Map<CommentWithID>(comment);

            return CreatedAtRoute("getCommentByID", new { bookId, comment.Id }, commentWithId);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int bookId, int id, CommentDTO commentDTO) 
        {
            var bookExist = await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);

            if (!bookExist)
            {
                return NotFound();
            }

            var commentExiste = await context.Comments.AnyAsync(commentDB => commentDB.Id == id);

            if (!commentExiste) { return NotFound(); }

            var comment = mapper.Map<Comment>(commentDTO);
            comment.BooksId = bookId;
            comment.Id = id;
            context.Update(comment);
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}
