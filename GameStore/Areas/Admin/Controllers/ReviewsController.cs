using GameStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReviewsController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var reviews = _context.Reviews
                .Include(r => r.Game)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string? returnUrl)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.ReviewId == id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
                TempData["Message"] = "Usunięto recenzję.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }
    }
}
