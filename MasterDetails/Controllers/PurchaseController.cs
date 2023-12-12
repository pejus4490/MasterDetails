using MasterDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace MasterDetails.Controllers;

public class PurchaseController : Controller
{
    private readonly MasterDetailsDbContext _context;

    public PurchaseController(MasterDetailsDbContext context)
    {
        _context = context;
    }

    // GET: Purchase
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Purchases.Include(p => p.Supplier);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Purchase/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Purchases == null)
        {
            return NotFound();
        }

        var purchase = await _context.Purchases
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (purchase == null)
        {
            return NotFound();
        }

        return View(purchase);
    }

    // GET: Purchase/Create
    public IActionResult Create()
    {
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
        ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName");
        Purchase purchase = new Purchase();
        purchase.PurchaseProducts.Add(new PurchaseProduct { Id = 1 });
        return View(purchase);
    }
   
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Purchase purchase, IFormFile pictureFile)
    {
        if (!ModelState.IsValid && purchase.SupplierId == 0)
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
            ViewData["ProductId"] = new SelectList(_context.Suppliers, "Id", "ProductName");
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName");
            return View(purchase);
        }
        if (pictureFile != null && pictureFile.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photo/ManualRequisitionAttach", pictureFile.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                pictureFile.CopyTo(stream);
            }
            purchase.Photo = $"{pictureFile.FileName}";
        }

        purchase.PurchaseProducts.RemoveAll(x => x.Quantity == 0);

        _context.Add(purchase);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Purchase/Edit/5
    public IActionResult Edit(int? id)
    {
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
        ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName");




        Purchase purchase = _context.Purchases.Where(x => x.Id == id)
            .Include(i => i.PurchaseProducts)
            .ThenInclude(i => i.Product)
            .FirstOrDefault();
        purchase.PurchaseProducts.ForEach(x => x.Amount = x.Quantity * x.PurchasePrice);



        return View(purchase);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Purchase purchase)
    {
        purchase.PurchaseProducts.RemoveAll(x => x.Quantity == 0);

        try
        {

            List<PurchaseProduct> purchaseItems = _context.PurchaseProducts.Where(x => x.PurchaseId == purchase.Id).ToList();
            _context.PurchaseProducts.RemoveRange(purchaseItems);
            await _context.SaveChangesAsync();

            _context.Attach(purchase);
            _context.Entry(purchase).State = EntityState.Modified;
            _context.PurchaseProducts.AddRange(purchase.PurchaseProducts);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }

    }

    // GET: Purchase/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        Purchase purchase = _context.Purchases.Where(x => x.Id == id)
            .Include(i => i.PurchaseProducts)
            .ThenInclude(i => i.Product)
            .FirstOrDefault();
        purchase.PurchaseProducts.ForEach(x => x.Amount = x.Quantity * x.PurchasePrice);

        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
        ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName");

        return View(purchase);
    }

    // POST: Purchase/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Purchase purchase)
    {
        purchase.PurchaseProducts.RemoveAll(a => a.Quantity == 0);

        try
        {
            List<PurchaseProduct> purchaseItems = _context.PurchaseProducts.Where(x => x.PurchaseId == purchase.Id).ToList();
            _context.PurchaseProducts.RemoveRange(purchaseItems);
            await _context.SaveChangesAsync();

            _context.Attach(purchase);
            _context.Entry(purchase).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    private bool PurchaseExists(int id)
    {
        return _context.Purchases.Any(e => e.Id == id);
    }
}
