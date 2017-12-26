using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Rhisis.Manager
{
    public partial class App : Application
    {
        private static App _instance;

        public static App Instance => _instance;

        public App()
        {
            _instance = this;
        }

        public void SwitchLanguage(string culture)
        {
            if (CultureInfo.CurrentCulture.Name.Equals(culture))
                return;

            var ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            this.LoadStringResource(culture);
        }

        private void LoadStringResource(string locale)
        {
            var resources = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Rhisis.Manager;component/Resources/Translations/AppTranslation.{locale}.xaml", UriKind.Absolute)
            };

            var current = Current.Resources.MergedDictionaries.FirstOrDefault(m => m.Source.OriginalString.EndsWith($"{locale}.xaml"));
            
            if (current != null)
                Current.Resources.MergedDictionaries.Remove(current);

            Current.Resources.MergedDictionaries.Add(resources);
        }
    }
}
