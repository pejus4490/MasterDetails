using MasterDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.Controllers;

public class UnitController : Controller
{
    private readonly MasterDetailsDbContext _context;

    public UnitController(MasterDetailsDbContext context)
    {
        _context = context;
    }

    // GET: Unit
    public async Task<IActionResult> Index()
    {
        return View(await _context.Units.ToListAsync());
    }

    // GET: Unit/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Units == null)
        {
            return NotFound();
        }

        var unit = await _context.Units
            .FirstOrDefaultAsync(m => m.Id == id);
        if (unit == null)
        {
            return NotFound();
        }

        return View(unit);
    }

    // GET: Unit/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,UnitName")] Unit unit)
    {
        if (ModelState.IsValid)
        {
            _context.Add(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(unit);
    }

    // GET: unit/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Units == null)
        {
            return NotFound();
        }

        var unit = await _context.Units.FindAsync(id);
        if (unit == null)
        {
            return NotFound();
        }
        return View(unit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,UnitName")] Unit unit)
    {
        if (id != unit.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(unit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(unit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(unit);
    }

    // GET: Unit/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Units == null)
        {
            return NotFound();
        }

        var unit = await _context.Units
            .FirstOrDefaultAsync(m => m.Id == id);
        if (unit == null)
        {
            return NotFound();
        }

        return View(unit);
    }

    // POST: Unit/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Units == null)
        {
            return Problem("Entity set 'AppDbContext.Units'  is null.");
        }
        var unit = await _context.Units.FindAsync(id);
        if (unit != null)
        {
            _context.Units.Remove(unit);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UnitExists(int id)
    {
        return _context.Units.Any(e => e.Id == id);
    }
}
