using MedApp.Models;
using MedApp.ViewModels;
using System.Windows;

namespace MedApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Employee Employee = new Employee();

        public MainWindow(MainViewModel vm)
        {
            this.DataContext = vm;
            InitializeComponent();

            Employee = vm.currentemployee;

            if (Employee.Post.Name == "Врач")
            {
                diagnosis.Visibility = Visibility.Collapsed;
                employees.Visibility = Visibility.Collapsed;
                EditItem.Visibility = Visibility.Collapsed;
                medicament.Visibility = Visibility.Collapsed;

                medrecords.IsSelected = true;

                MedRecordViewModel medvm = new MedRecordViewModel(Employee);
                UCMedRecord.DataContext = medvm;
            }
            else
            if (Employee.Post.Name == "Сотрудник регистратуры")
            {
                diagnosis.Visibility = Visibility.Collapsed;
                employees.Visibility = Visibility.Collapsed;
                EditItem.Visibility = Visibility.Collapsed;
                medicament.Visibility = Visibility.Collapsed;

                medrecords.IsSelected = true;

                MedRecordViewModel medvm = new MedRecordViewModel(Employee);
                UCMedRecord.DataContext = medvm;
            }
            if (Employee.Post.Name == "Администратор")
            {
                EditItem.Visibility = Visibility.Collapsed;
                MedRecordViewModel medvm = new MedRecordViewModel(Employee);
                UCMedRecord.DataContext = medvm;
                DiagnosisViewModel diavm = new DiagnosisViewModel();
                UCDiagnosis.DataContext = diavm;
                MedicamentViewModel medivm = new MedicamentViewModel();
                UCMedicament.DataContext = medivm;
                EmployeeViewModel empvm = new EmployeeViewModel();
                UCEmployee.DataContext = empvm;
            }         
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
