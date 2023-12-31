﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class BookDTO
    {
        [Required]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public List<int> AuthorsIds { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
