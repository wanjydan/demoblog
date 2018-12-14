using System;

namespace DemoBlog.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public AuthorViewModel Author { get; set; }
    }
}