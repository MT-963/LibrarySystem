namespace LibrarySystem.Models.ViewModels
{
    public class ReturnBookViewModel
    {
        public BorrowingRecord BorrowingRecord { get; set; } = null!;
        public decimal Fine { get; set; }
    }
}
