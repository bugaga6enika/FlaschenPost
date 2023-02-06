using System;
using System.Globalization;

namespace FlaschenPost.Infrastructure.Drinks.Tests
{
    public class DeCultureBaseClass
    {
        public DeCultureBaseClass()
        {
            var culture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}

