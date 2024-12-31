namespace LibrarySystem.Models.ViewModels
{
    public class MemberDetailsViewModel
    {
        public Member Member { get; set; } = null!;
        public IEnumerable<BorrowingRecord> ActiveBorrowings { get; set; } = null!;
        public IEnumerable<BorrowingRecord> BorrowingHistory { get; set; } = null!;
        public bool CanBorrowMore { get; set; }
        public Dictionary<int, decimal> Fines { get; set; } = null!;
    }
}
