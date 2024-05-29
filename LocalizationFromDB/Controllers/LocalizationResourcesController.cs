using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LocalizationFromDB.Data;
using Microsoft.Extensions.Localization;

namespace LocalizationFromDB.Controllers
{
    public class LocalizationResourcesController : Controller
    {
        private readonly MvcprojectsContext _context;
        private readonly IStringLocalizer<LocalizationResourcesController> _localizer;

        public LocalizationResourcesController(MvcprojectsContext context, IStringLocalizer<LocalizationResourcesController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: LocalizationResources
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocalizationResources.ToListAsync());
        }

        // GET: LocalizationResources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localizationResource = await _context.LocalizationResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (localizationResource == null)
            {
                return NotFound();
            }

            return View(localizationResource);
        }

        // GET: LocalizationResources/Create
        public IActionResult Create()
        {
            ViewBag.CultureList = new SelectList(_context.LocalizationCultures.ToList(), "CultureCode", "DisplayName");
            return View();
        }

        // POST: LocalizationResources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceKey,Value,Culture")] LocalizationResource localizationResource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localizationResource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localizationResource);
        }

        // GET: LocalizationResources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localizationResource = await _context.LocalizationResources.FindAsync(id);
            if (localizationResource == null)
            {
                return NotFound();
            }

            ViewBag.CultureList = new SelectList(_context.LocalizationCultures.ToList(), "CultureCode", "DisplayName");
            return View(localizationResource);
        }

        // POST: LocalizationResources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceKey,Value,Culture")] LocalizationResource localizationResource)
        {
            if (id != localizationResource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localizationResource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalizationResourceExists(localizationResource.Id))
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
            return View(localizationResource);
        }

        // GET: LocalizationResources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localizationResource = await _context.LocalizationResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (localizationResource == null)
            {
                return NotFound();
            }

            return View(localizationResource);
        }

        // POST: LocalizationResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localizationResource = await _context.LocalizationResources.FindAsync(id);
            if (localizationResource != null)
            {
                _context.LocalizationResources.Remove(localizationResource);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalizationResourceExists(int id)
        {
            return _context.LocalizationResources.Any(e => e.Id == id);
        }
    }
}
