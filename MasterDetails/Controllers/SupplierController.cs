using MasterDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MasterDetails.Controllers;

public class SupplierController : Controller
{
    private readonly MasterDetailsDbContext _context;

    public SupplierController(MasterDetailsDbContext context)
    {
        _context = context;
    }

    // GET: Supplier
    public async Task<IActionResult> Index()
    {
        return View(await _context.Suppliers.ToListAsync());
    }

    // GET: Supplier/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Suppliers == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // GET: Supplier/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,SupplierName,PhoneNumber,EmailAddress")] Supplier supplier)
    {
       
       
            _context.Add(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
       
    }

    // GET: Supplier/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Suppliers == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierName,PhoneNumber,EmailAddress")] Supplier supplier)
    {
        if (id != supplier.Id)
        {
            return NotFound();
        }

        
        
            try
            {
                _context.Update(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.Id))
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

    // GET: Supplier/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Suppliers == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // POST: Supplier/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Suppliers == null)
        {
            return Problem("Entity set 'AppDbContext.Suppliers'  is null.");
        }
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SupplierExists(int id)
    {
        return _context.Suppliers.Any(e => e.Id == id);
    }
}
