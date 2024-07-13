using MonkeyTypeWPF.modelviews;
using MonkeyTypeWPF.utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MonkeyTypeWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        // код при запуске программы
        // экземпляр основной ViewModel
        navigationVM mainVM = new navigationVM();
        protected override void OnStartup(StartupEventArgs e)
        {
            // при запуске
            base.OnStartup(e);
            new MainWindow() { DataContext = mainVM }.Show();
            // вызываем метод создания json
            JsonData.create_json();
            // вызываем метод создания CSV
            CSVData.create_csv();
        }
    }
}
