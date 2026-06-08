using System.Security.Claims;
using GameStore.Data;
using GameStore.Models.DbModels;
using GameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string? searchTerm, int? genreId, string? sortOrder)
        {
            var query = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Publisher)
                .Include(g => g.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(g => g.Title.Contains(searchTerm));

            if (genreId.HasValue && genreId.Value > 0)
                query = query.Where(g => g.GenreId == genreId.Value);

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(g => g.Price),
                "price_desc" => query.OrderByDescending(g => g.Price),
                "title" => query.OrderBy(g => g.Title),
                _ => query.OrderByDescending(g => g.ReleaseDate)
            };

            var vm = new StoreIndexViewModel
            {
                Games = query.ToList(),
                Genres = _context.Genres.OrderBy(g => g.Name).ToList(),
                SearchTerm = searchTerm,
                SelectedGenreId = genreId,
                SortOrder = sortOrder
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var game = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Publisher)
                .Include(g => g.Reviews)
                .FirstOrDefault(g => g.GameId == id);

            if (game == null) return NotFound();

            var vm = new GameDetailsViewModel
            {
                Game = game,
                Reviews = game.Reviews.OrderByDescending(r => r.CreatedAt).ToList(),
                NewReview = new Review { GameId = game.GameId }
            };

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                vm.IsInLibrary = _context.LibraryEntries.Any(l => l.GameId == id && l.UserId == userId);
                vm.HasReviewed = _context.Reviews.Any(r => r.GameId == id && r.UserId == userId);
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(Review review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            if (!_context.LibraryEntries.Any(l => l.GameId == review.GameId && l.UserId == userId))
            {
                TempData["Message"] = "Aby dodać recenzję, musisz najpierw mieć tę grę w swojej bibliotece.";
                return RedirectToAction(nameof(Details), new { id = review.GameId });
            }

            if (_context.Reviews.Any(r => r.GameId == review.GameId && r.UserId == userId))
            {
                TempData["Message"] = "Masz już recenzję tej gry.";
                return RedirectToAction(nameof(Details), new { id = review.GameId });
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Uzupełnij ocenę (1–5) oraz treść recenzji.";
                return RedirectToAction(nameof(Details), new { id = review.GameId });
            }

            review.UserId = userId;
            review.UserName = User.Identity?.Name ?? "Użytkownik";
            review.CreatedAt = DateTime.Now;

            _context.Reviews.Add(review);
            _context.SaveChanges();

            TempData["Message"] = "Dziękujemy za recenzję!";
            return RedirectToAction(nameof(Details), new { id = review.GameId });
        }
    }
}
