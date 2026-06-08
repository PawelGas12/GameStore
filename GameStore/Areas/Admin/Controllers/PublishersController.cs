using GameStore.Data;
using GameStore.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublishersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PublishersController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Publishers.Include(g => g.Games).OrderBy(g => g.Name).ToList());

        public IActionResult Create() => View(new Publisher());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
                TempData["Message"] = "Dodano wydawcę.";
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public IActionResult Edit(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(g => g.PublisherId == id);
            if (publisher == null) return NotFound();
            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Publisher publisher)
        {
            if (!ModelState.IsValid) return View(publisher);
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
            TempData["Message"] = "Zapisano zmiany.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(g => g.PublisherId == id);
            if (publisher == null) return NotFound();
            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(g => g.PublisherId == id);
            if (publisher != null)
            {
                if (_context.Games.Any(g => g.PublisherId == id))
                {
                    TempData["Message"] = "Nie można usunąć wydawcy, do którego przypisano gry.";
                    return RedirectToAction(nameof(Index));
                }
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
                TempData["Message"] = "Usunięto wydawcę.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
