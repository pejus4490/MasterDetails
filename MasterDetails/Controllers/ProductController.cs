using MasterDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MasterDetails.Controllers;

public class ProductController : Controller
{
    private readonly MasterDetailsDbContext _context;

    public ProductController(MasterDetailsDbContext context)
    {
        _context = context;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ProductName,ProductCode,Discription")] Product product)
    {
          _context.Add(product);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ProductCode,Discription")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }
    
        try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(product.Id))
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

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'AppDbContext.Products'  is null.");
        }
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
