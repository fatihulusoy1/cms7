 using System;

namespace CMS.Application.DTOs
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }
}
