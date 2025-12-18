using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MyAppContext _context;
        public ItemsController(MyAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, int? itemId = null)
        {
            // Get total number of items
            var totalItems = await _context.Items.CountAsync();

            // Get the paginated list of items
            var items = await _context.Items
                .Include(s => s.SerialNumber)
                .Include(c => c.Category)
                .Include(ic => ic.ItemClients)
                    .ThenInclude(c => c.Client)
                    .OrderByDescending(i => i.ID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Create a PaginatedList
            var model = new PaginatedList<Item>(items, totalItems, pageNumber, pageSize);

            // If an ID is passed, we highlight the last inserted item

            
            if (itemId.HasValue)
            {
                ViewData["LastInsertedItemId"] = itemId.Value;
            }

            return View(model);
            /*var item = await _context.Items.Include(S =>S.SerialNumber).Include(c => c.Category).Include(ic => ic.ItemClients).ThenInclude(c=>c.Client).ToListAsync();
            return View(item);*/
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID, Name, Price, CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                // Get the last inserted item's ID (assuming ID is auto-incremented)
                var lastInsertedItemId = item.ID;

                return RedirectToAction("index", new {itemId = lastInsertedItemId});
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            var item = await _context.Items.FirstOrDefaultAsync(x => x.ID == id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Name, Price, CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.ID == id);
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }


        /*public IActionResult Overview()
        {
            var item = new Item() { Name = "mouse" };
            return View(item);
        }

        public IActionResult Edit(int itemId)
        {
            return Content("id=" + itemId);
        }*/
    }
}
