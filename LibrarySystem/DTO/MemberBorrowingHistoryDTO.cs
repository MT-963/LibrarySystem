namespace LibrarySystem.DTO
{

    public class MemberBorrowingHistoryDTO
    {
        public string Title { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
    }
}
