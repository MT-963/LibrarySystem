namespace LibrarySystem.Models
{
    public class BorrowingDetails
    {
        public int BorrowingId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
        public int DaysOverdue { get; set; }
    }
}
