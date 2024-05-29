using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LocalizationFromDB.Data;
using Microsoft.Extensions.Localization;
using LocalizationFromDB.ViewModels;

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
            var resources = await _context.LocalizationResources.GroupBy(a => a.ResourceKey).ToListAsync();
            var cultures = await _context.LocalizationCultures.Select(c => c.CultureCode).ToListAsync();
            var viewModel = new List<ViewModels.LocalizationResourceViewModel>();
            foreach(var resource in resources)
            {
                var resourceViewModel = new ViewModels.LocalizationResourceViewModel
                {
                    ResourceKey = resource.Key
                };
                foreach(var culture in cultures)
                {
                    var localizedValue = resource.FirstOrDefault(r => r.Culture == culture);
                    if(localizedValue != null)
                    {
                        resourceViewModel.LocalizedValues.Add(culture, localizedValue.Value);
                    }
                    else
                    {
                        resourceViewModel.LocalizedValues.Add(culture, "NOT TRANSLATED");
                    }
                }
                viewModel.Add(resourceViewModel);
            }
            ViewBag.Cultures = cultures;
            return View(viewModel);
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
        public async Task<IActionResult> CreateAsync()
        {
            var cultures = await _context.LocalizationCultures
             .Select(c => c.CultureCode)
             .ToListAsync();

            var viewModel = new LocalizationResourceViewModel();
            foreach (var culture in cultures)
            {
                viewModel.LocalizedValues[culture] = string.Empty;
            }

            ViewBag.Cultures = cultures;
            return View(viewModel);
        }

        // POST: LocalizationResources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocalizationResourceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var localizedValue in viewModel.LocalizedValues)
                {
                    var resource = new LocalizationResource
                    {
                        ResourceKey = viewModel.ResourceKey,
                        Culture = localizedValue.Key,
                        Value = localizedValue.Value
                    };
                    _context.LocalizationResources.Add(resource);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cultures = await _context.LocalizationCultures
                .Select(c => c.CultureCode)
                .ToListAsync();

            return View(viewModel);
        }

        // GET: LocalizationResources/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resources = await _context.LocalizationResources
                .Where(r => r.ResourceKey == id)
                .ToListAsync();

            if (!resources.Any())
            {
                return NotFound();
            }

            var cultures = await _context.LocalizationCultures
                .Select(c => c.CultureCode)
                .ToListAsync();

            var viewModel = new LocalizationResourceViewModel
            {
                ResourceKey = id
            };

            foreach (var culture in cultures)
            {
                var localizedValue = resources.FirstOrDefault(r => r.Culture == culture)?.Value;
                viewModel.LocalizedValues[culture] = localizedValue ?? string.Empty;
            }

            ViewBag.Cultures = cultures;
            return View(viewModel);
        }

        // POST: LocalizationResources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, LocalizationResourceViewModel viewModel)
        {
            if (id != viewModel.ResourceKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingResources = await _context.LocalizationResources
                    .Where(r => r.ResourceKey == id)
                    .ToListAsync();

                // Delete existing resources
                _context.LocalizationResources.RemoveRange(existingResources);

                // Add updated resources
                foreach (var localizedValue in viewModel.LocalizedValues)
                {   if (localizedValue.Value != null && localizedValue.Value != string.Empty)
                    {
                    var resource = new LocalizationResource
                    {
                        ResourceKey = viewModel.ResourceKey,
                        Culture = localizedValue.Key,
                        Value = localizedValue.Value
                    };
                    _context.LocalizationResources.Add(resource);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cultures = await _context.LocalizationCultures
                .Select(c => c.CultureCode)
                .ToListAsync();

            return View(viewModel);
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
