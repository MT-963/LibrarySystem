using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibrarySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;

        public HomeController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalBooks = await _context.Books.CountAsync();
            ViewBag.TotalMembers = await _context.Members.CountAsync();

            // Get overdue books using stored procedure
            var overdueBooks = await _context.GetOverdueBooksAsync();
            ViewBag.OverdueBooks = overdueBooks.Count;
            ViewBag.ActiveBorrowings = await _context.BorrowingRecords
                .Where(b => b.ReturnDate == null)
                .CountAsync();

            // Get recent activities
            var recentBorrowings = await _context.BorrowingRecords
                .OrderByDescending(b => b.BorrowDate)
                .Take(5)
                .Include(b => b.Book)
                .Include(b => b.Member)
                .ToListAsync();

            ViewBag.RecentActivities = recentBorrowings
                .Select(b => $"{b.Member.Name} borrowed {b.Book.Title} on {b.BorrowDate.ToShortDateString()}")
                .ToList();

            return View();
        }
    }
}
