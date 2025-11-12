namespace MiniSocialMediaApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        public required string AuthorId { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public required DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? EditedAt { get; set; }
    }
}
