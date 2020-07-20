using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BindPlanet.Data;
using BindPlanet.Models;
using Microsoft.AspNetCore.Identity;
using BindPlanet.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BindPlanet.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public static string order = "";
        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
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

        // products/order?type=name&direction=1
        public async Task<IActionResult> Index(ProductType type = ProductType.Name, OrderDirection direction = OrderDirection.Ascending, string searchterm = "")
        {
            var list = await _context.Products.Include(p => p.Category).Include(p => p.Supplier).ToListAsync();
            if (!string.IsNullOrEmpty(searchterm))
            {
                list = list.Where(x => x.Name.ToLower().Contains(searchterm.ToLower())).ToList();
            }
            ViewBag.type = type;
            ViewBag.direction = direction;
            switch (type)
            {
                case ProductType.Name:
                    if (direction == OrderDirection.Descending)
                    {
                        list = list.OrderByDescending(x => x.Name).ToList();
                    }
                    else
                    {
                        list = list.OrderBy(x => x.Name).ToList();
                    }
                    break;
                case ProductType.Price:
                    if (direction == OrderDirection.Descending)
                    {
                        list = list.OrderByDescending(x => x.Price).ToList(); ;
                    }
                    else
                    {
                        list = list.OrderBy(x => x.Price).ToList(); ;
                    }
                    break;
                case ProductType.Category:
                    if (direction == OrderDirection.Descending)
                    {
                        list = list.OrderByDescending(x => x.Category.Name).ToList();
                    }
                    else
                    {
                        list = list.OrderBy(x => x.Category.Name).ToList();
                     
                    }
                    break;
                case ProductType.Quantity:
                    if (direction == OrderDirection.Descending)
                    {
                        list = list.OrderByDescending(x => x.Quantity).ToList();
                    }
                    else
                    {
                        list = list.OrderBy(x => x.Quantity).ToList();
                    }
                    break;
                default:
                    return View("Index", list);
            }

            return View("Index", list);
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
