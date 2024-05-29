using LocalizationFromDB.Data;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace LocalizationFromDB.Localization
{
    public class DbStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DbStringLocalizerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return CreateLocalizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return CreateLocalizer();
        }

        private IStringLocalizer CreateLocalizer()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MvcprojectsContext>();
            var culture = CultureInfo.CurrentUICulture.Name;
            return new DbStringLocalizer(context, culture);
        }
    }
}