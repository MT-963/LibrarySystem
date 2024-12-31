namespace LibrarySystem.Models
{
    public class ActiveBorrowing
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
