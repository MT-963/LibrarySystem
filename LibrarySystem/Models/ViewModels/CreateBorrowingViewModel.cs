using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels
{
    public class CreateBorrowingViewModel
    {
        [Required(ErrorMessage = "Please select a book")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please select a member")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please set a due date")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public IEnumerable<Book> AvailableBooks { get; set; } = new List<Book>();
        public IEnumerable<Member> Members { get; set; } = new List<Member>();
    }
}
