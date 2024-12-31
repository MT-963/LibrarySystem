using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly LibraryContext _context;

        public BorrowingController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Borrowing
        public async Task<IActionResult> Index()
        {
            var borrowings = await _context.BorrowingRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return View(borrowings);
        }

        // GET: Borrowing/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateBorrowingViewModel
            {
                AvailableBooks = await _context.Books
                    .Where(b => b.IsAvailable)
                    .ToListAsync(),
                Members = await _context.Members.ToListAsync(),
                DueDate = DateTime.Now.AddDays(14) // Default due date: 14 days from now
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBorrowingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.BorrowBookAsync(
                    viewModel.BookId,
                    viewModel.MemberId,
                    viewModel.DueDate);

                if (result == "Success")
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", result);
            }

            // If we got this far, something failed, redisplay form
            viewModel.AvailableBooks = await _context.Books
                .Where(b => b.IsAvailable)
                .ToListAsync();
            viewModel.Members = await _context.Members.ToListAsync();
            return View(viewModel);
        }

        // GET: Borrowing/Return/5
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.BorrowingRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (borrowing == null)
            {
                return NotFound();
            }

            var fine = _context.CalculateFine(borrowing.Id);
            var viewModel = new ReturnBookViewModel
            {
                BorrowingRecord = borrowing,
                Fine = fine
            };

            return View(viewModel);
        }

        // POST: Borrowing/ReturnConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(int id)
        {
            var result = await _context.ReturnBookAsync(id);

            if (result == "Success")
            {
                return RedirectToAction(nameof(Index));
            }

            // If there was an error, redisplay the return form with the error
            ModelState.AddModelError("", "Error returning book: " + result);

            var borrowing = await _context.BorrowingRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (borrowing == null)
            {
                return NotFound();
            }

            var fine = _context.CalculateFine(borrowing.Id);
            var viewModel = new ReturnBookViewModel
            {
                BorrowingRecord = borrowing,
                Fine = fine
            };

            return View("Return", viewModel);
        }
        // GET: Borrowing/Overdue
        public async Task<IActionResult> Overdue()
        {
            var overdueBooks = await _context.BorrowingRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .Where(b => b.ReturnDate == null && b.DueDate < DateTime.Now)
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            return View(overdueBooks);
        }
    }
}
