using MedApp.Models;
using MedApp.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace MedApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public Employee currentemployee = new Employee();

        public Employee Сurrentemployee
        {
            get { return currentemployee; }
            set
            {
                currentemployee = value;
                RaisePropertyChanged("Сurrentemployee");
            }
        }

        public MainViewModel(Employee employee)
        {
            currentemployee = employee;
        }
    }
}
