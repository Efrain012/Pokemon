namespace MiniSocialMediaApp.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public required string AuthorUsername { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public required DateTimeOffset CreatedAt { get; set; }

        public required DateTimeOffset? EditedAt { get; set; }
    }
}
