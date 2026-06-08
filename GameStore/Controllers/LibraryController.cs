using System.Security.Claims;
using GameStore.Data;
using GameStore.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibraryController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public IActionResult Index()
        {
            var games = _context.LibraryEntries
                .Where(l => l.UserId == CurrentUserId)
                .Include(l => l.Game).ThenInclude(g => g!.Genre)
                .Include(l => l.Game).ThenInclude(g => g!.Publisher)
                .OrderByDescending(l => l.AddedAt)
                .Select(l => l.Game!)
                .ToList();
            return View(games);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int gameId)
        {
            var exists = _context.LibraryEntries.Any(l => l.GameId == gameId && l.UserId == CurrentUserId);
            if (!exists && _context.Games.Any(g => g.GameId == gameId))
            {
                _context.LibraryEntries.Add(new LibraryEntry
                {
                    GameId = gameId,
                    UserId = CurrentUserId,
                    AddedAt = DateTime.Now
                });
                _context.SaveChanges();
                TempData["Message"] = "Dodano grę do Twojej biblioteki.";
            }
            return RedirectToAction("Details", "Games", new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int gameId)
        {
            var entry = _context.LibraryEntries
                .FirstOrDefault(l => l.GameId == gameId && l.UserId == CurrentUserId);
            if (entry != null)
            {
                _context.LibraryEntries.Remove(entry);
                _context.SaveChanges();
                TempData["Message"] = "Usunięto grę z biblioteki.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
