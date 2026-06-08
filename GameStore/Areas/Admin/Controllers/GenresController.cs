using GameStore.Data;
using GameStore.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GenresController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Genres.Include(g => g.Games).OrderBy(g => g.Name).ToList());

        public IActionResult Create() => View(new Genre());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Genres.Add(genre);
                _context.SaveChanges();
                TempData["Message"] = "Dodano gatunek.";
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        public IActionResult Edit(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);
            if (genre == null) return NotFound();
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre genre)
        {
            if (!ModelState.IsValid) return View(genre);
            _context.Genres.Update(genre);
            _context.SaveChanges();
            TempData["Message"] = "Zapisano zmiany.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);
            if (genre == null) return NotFound();
            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);
            if (genre != null)
            {
                if (_context.Games.Any(g => g.GenreId == id))
                {
                    TempData["Message"] = "Nie można usunąć gatunku, do którego przypisano gry.";
                    return RedirectToAction(nameof(Index));
                }
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                TempData["Message"] = "Usunięto gatunek.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
