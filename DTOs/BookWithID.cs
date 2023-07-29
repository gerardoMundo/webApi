using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

namespace WebApi.DTOs
{
    public class BookWithID
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public List<CommentWithID> Comments { get; set; }
    }
}
