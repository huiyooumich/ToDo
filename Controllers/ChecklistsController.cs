using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class ChecklistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checklists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Checklist.ToListAsync());
        }

        // GET: Checklists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .FirstOrDefaultAsync(m => m.ChecklistId == id);

            var items = _context.Items.Where(m => m.ChecklistId == id).ToList();

            return View(checklist);
        }

        // GET: Checklists/Create
        public IActionResult Create()
        {
            return View();
        }
        // GET: Items/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: Items/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase, String SearchType)
        {
            Console.WriteLine(SearchType);
            Console.WriteLine("Hello");
            if (SearchType == "Checklist")
            {
                return View("Index", await _context.Checklist.Where(i => i.Description.Contains(SearchPhrase)
                        || i.ChecklistName.Contains(SearchPhrase)).ToListAsync());
            }
            else
            {
                return View("ItemIndex", await _context.Items.Where(i => i.Title.Contains(SearchPhrase)).ToListAsync());
            }

        }

        // POST: Checklists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChecklistId,ChecklistName,Description")] Checklist checklist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checklist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checklist);
        }

        // GET: Checklists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist.FindAsync(id);
            if (checklist == null)
            {
                return NotFound();
            }
            return View(checklist);
        }

        // POST: Checklists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChecklistId,ChecklistName,Description")] Checklist checklist)
        {
            if (id != checklist.ChecklistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checklist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistExists(checklist.ChecklistId))
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
            return View(checklist);
        }

        // GET: Checklists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .FirstOrDefaultAsync(m => m.ChecklistId == id);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // POST: Checklists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checklist = await _context.Checklist.FindAsync(id);
            _context.Checklist.Remove(checklist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChecklistExists(int id)
        {
            return _context.Checklist.Any(e => e.ChecklistId == id);
        }


        // GET: Items
        public async Task<IActionResult> ItemIndex()
        {
            var applicationDbContext = _context.Items.Include(i => i.Checklist);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> ItemDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Checklist)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult ItemCreate()
        {
            ViewData["ChecklistId"] = new SelectList(_context.Checklist, "ChecklistId", "ChecklistId");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemCreate([Bind("ItemId,Title,Done,ChecklistId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChecklistId"] = new SelectList(_context.Checklist, "ChecklistId", "ChecklistId", item.ChecklistId);

            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> ItemEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ChecklistId"] = new SelectList(_context.Checklist, "ChecklistId", "ChecklistId", item.ChecklistId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemEdit(int id, [Bind("ItemId,Title,Done,ChecklistId")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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
            ViewData["ChecklistId"] = new SelectList(_context.Checklist, "ChecklistId", "ChecklistId", item.ChecklistId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> ItemDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Checklist)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("ItemDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemDeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
