using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LocalizationFromDB.Data;

namespace LocalizationFromDB.Localization
{
    public class DbStringLocalizer : IStringLocalizer
    {
        private readonly MvcprojectsContext _context;
        private readonly string _culture;

        public DbStringLocalizer(MvcprojectsContext context, string culture)
        {
            _context = context;
            _culture = culture;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = _context.LocalizationResources
                    .FirstOrDefault(r => r.ResourceKey == name && r.Culture == _culture)?.Value;

                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = this[name].Value;
                var value = string.Format(format, arguments);
                return new LocalizedString(name, value, format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _context.LocalizationResources
                .Where(r => r.Culture == _culture)
                .Select(r => new LocalizedString(r.ResourceKey, r.Value, false))
                .ToList();
        }
    }
}