namespace LibrarySystem.Models
{
    // Path: Models/BorrowingRecord.cs
    public class BorrowingRecord
    {
        public int Id { get; set; }  // Remove required as it's auto-generated
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }  // Remove required as it's set in constructor
        public DateTime? ReturnDate { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;

        public BorrowingRecord()
        {
            // Initialize any required properties here
            BorrowDate = DateTime.UtcNow;
        }
    }
}
