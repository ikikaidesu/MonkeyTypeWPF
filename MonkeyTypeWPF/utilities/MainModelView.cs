using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyTypeWPF.utilities
{
    // метод для всех ViewModel классов, нужно для оповещения об обновлении данных
    public class MainModelView : INotifyPropertyChanged
    {
        // ивент обновления
        public event PropertyChangedEventHandler PropertyChanged;
        // метод обновления
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
