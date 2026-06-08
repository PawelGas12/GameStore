using GameStore.Data;
using GameStore.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public GamesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var games = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Publisher)
                .OrderBy(g => g.Title)
                .ToList();
            return View(games);
        }

        public IActionResult Create()
        {
            PopulateDropDowns();
            return View(new Game());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Game game, IFormFile? coverFile)
        {
            if (ModelState.IsValid)
            {
                game.CoverImagePath = SaveCover(coverFile);
                _context.Games.Add(game);
                _context.SaveChanges();
                TempData["Message"] = "Dodano nową grę.";
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns(game);
            return View(game);
        }

        public IActionResult Edit(int id)
        {
            var game = _context.Games.FirstOrDefault(g => g.GameId == id);
            if (game == null) return NotFound();
            PopulateDropDowns(game);
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Game game, IFormFile? coverFile)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDowns(game);
                return View(game);
            }

            var newCover = SaveCover(coverFile);
            if (newCover != null)
                game.CoverImagePath = newCover;

            _context.Games.Update(game);
            _context.SaveChanges();
            TempData["Message"] = "Zapisano zmiany.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var game = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.Publisher)
                .FirstOrDefault(g => g.GameId == id);
            if (game == null) return NotFound();
            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var game = _context.Games.FirstOrDefault(g => g.GameId == id);
            if (game != null)
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
                TempData["Message"] = "Usunięto grę.";
            }
            return RedirectToAction(nameof(Index));
        }

        private string? SaveCover(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return $"/uploads/{fileName}";
        }

        private void PopulateDropDowns(Game? game = null)
        {
            ViewBag.Genres = new SelectList(_context.Genres.OrderBy(g => g.Name),
                "GenreId", "Name", game?.GenreId);
            ViewBag.Publishers = new SelectList(_context.Publishers.OrderBy(p => p.Name),
                "PublisherId", "Name", game?.PublisherId);
        }
    }
}
