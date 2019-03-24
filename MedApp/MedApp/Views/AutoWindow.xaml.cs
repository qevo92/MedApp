using MedApp.ViewModels;
using System.Windows;

namespace MedApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AutoWindow.xaml
    /// </summary>
    public partial class AutoWindow : Window
    {
        public AutoWindow()
        {
            AutoViewModel vm = new AutoViewModel();

            this.DataContext = vm;

            vm.OnRequestClose += (s, e) => this.Close();

            InitializeComponent();
        }
    }
}
