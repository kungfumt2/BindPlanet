using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BindPlanet.Data;
using BindPlanet.Models;

namespace BindPlanet.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public static string order= ""; 
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var bindPlanetContext = _context.Products.Include(p => p.Category).Include(p => p.Supplier);
           order = "";
            return View(await bindPlanetContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "SupplierId", "Name");
            return View();
        }

        public async Task<IActionResult> Order(string type)
        {
            var bindPlanetContext = _context.Products.Include(p => p.Category).Include(p => p.Supplier);
            await bindPlanetContext.ToListAsync();
            
            switch (type)
            {
                case "name":
                    if (order == "n1")
                    {
                        var byname = bindPlanetContext.OrderByDescending(x => x.Name);

                        order = "n2";
                        return View("Index", byname); 
                    }
                    else
                    {
                        var byname = bindPlanetContext.OrderBy(x => x.Name);

                        order = "n1";
                        return View("Index", byname);
                    }
             
                   
                case "price":
                    if (order == "p1")
                    {
                        var byprice = bindPlanetContext.OrderByDescending(x => x.Price);
                        order = "p2";
                        return View("Index", byprice);
                    }
                    else
                    {
                        var byprice = bindPlanetContext.OrderBy(x => x.Price);
                        order = "p1";
                        return View("Index", byprice);
                    }

                 
                
                case "category":
                    if (order == "c1")
                    {
                        var bycategory = bindPlanetContext.OrderByDescending(x => x.Category.Name);
                        order = "c2";
                        return View("Index", bycategory);
                    }
                    else
                    {
                        var bycategory = bindPlanetContext.OrderBy(x => x.Category.Name);
                        order = "c1";
                        return View("Index", bycategory);
                    }
                   

                case "quantity":
                    if (order == "q1")
                    {
                        var byquantity = bindPlanetContext.OrderByDescending(x => x.Quantity);
                        order = "q2";
                        return View("Index", byquantity);
                    }
                    else
                    {
                        var byquantity = bindPlanetContext.OrderBy(x => x.Quantity);
                        order = "q1";
                        return View("Index", byquantity);
                    }

                  

                
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return RedirectToAction(nameof(Index));






        }
        public async Task<IActionResult> Search(string word)
        {
            if (word != null)
            {
                var bindPlanetContext = _context.Products.Include(p => p.Category).Include(p => p.Supplier);
                await bindPlanetContext.ToListAsync();

                var searched = bindPlanetContext.Where(x => x.Name.Contains(word));

                return View("Index", searched);
            }
            return RedirectToAction(nameof(Index));

        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Quantity,Price,Description,SupplierId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "SupplierId", "Name");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Quantity,Price,Description,SupplierId,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
