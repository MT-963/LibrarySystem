namespace LibrarySystem.Models
{
    public class Book
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? ISBN { get; set; }
        public bool IsAvailable { get; set; } = true;

        public ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();
    }
}
