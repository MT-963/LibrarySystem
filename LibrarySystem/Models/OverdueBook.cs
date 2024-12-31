namespace LibrarySystem.Models
{
    public class OverdueBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string MemberName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
    }
}
