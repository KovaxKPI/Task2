namespace Task2.Models
{
    public class BookDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public decimal Rating { get; set; }
        public List<Review> reviews { get; set; }
    }
}
