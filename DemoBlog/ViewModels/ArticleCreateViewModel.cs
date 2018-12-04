using System.ComponentModel.DataAnnotations;

namespace DemoBlog.ViewModels
{
    public class ArticleCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Body must be between 2 and 200 characters")]
        public string Body { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        [RegularExpression(
            @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}",
            ErrorMessage = "CategoryId must be a valid guid")]
        public string CategoryId { get; set; }

        public string[] TagIds { get; set; }
    }
}