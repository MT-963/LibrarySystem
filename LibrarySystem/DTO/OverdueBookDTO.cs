namespace LibrarySystem.DTO
{
    public class OverdueBookDTO
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
    }
}
