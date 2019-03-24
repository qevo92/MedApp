using MedApp.Models;
using MedApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MedApp.ViewModels
{
    public class EmployeeViewModel : BindableBase
    {
        MedAppDBEntities context;

        public Employee selectedEmployee { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<Post> Posts { get; set; }

        public Post currentPost { get; set; }

        public DelegateCommand EditEmployee { get; }
        public DelegateCommand AddEmployee { get; }
        public DelegateCommand RemoveEmployee { get; }
        public DelegateCommand SaveEmployee { get; }
        public DelegateCommand CanselEdit { get; }

        bool editoradd;

        public string _searchKey { get; set; }

        public ICollectionView FilterCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Employees); }
        }

        private string filterString;
        public string FilterString
        {
            get { return filterString; }
            set
            {
                if (Equals(value, filterString)) return;
                filterString = value;
                FilterCollectionView.Refresh(); // tirggers filtering logic
                RaisePropertyChanged("FilterString");
            }
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee?.EndEdit();

                selectedEmployee = value;
                RaisePropertyChanged("SelectedEmployee");
                EditEmployee.RaiseCanExecuteChanged();
                RemoveEmployee.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Employee> EmployeesTake()
        {
            return Employees;
        }

        public void DownloadEmployees()
        {
            Employees = new ObservableCollection<Employee>();
            context = new MedAppDBEntities();
            List<Employee> temp = context.Employee.ToList();
            foreach (var item in temp)
            {
                Employees.Add(item);
            }
            Posts = new ObservableCollection<Post>();
            List<Post> temp1 = context.Post.ToList();
            foreach (var item in temp1)
            {
                Posts.Add(item);
            }
        }

        public EmployeeViewModel()
        {
            DownloadEmployees();

            FilterCollectionView.Filter = i =>
            {
                if (_searchKey == "" || _searchKey == null) return true;
                if (string.IsNullOrEmpty(FilterString)) return true;
                if (_searchKey == "Фамилия")
                {
                    Employee m = i as Employee;
                    return m.Surname.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Имя")
                {
                    Employee m = i as Employee;
                    return m.Name.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Отчество")
                {
                    Employee m = i as Employee;
                    return m.Patronymic.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Город")
                {
                    Employee m = i as Employee;
                    return m.City.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Улица")
                {
                    Employee m = i as Employee;
                    return m.Street.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Дом")
                {
                    Employee m = i as Employee;
                    return m.Home.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Должность")
                {
                    Employee m = i as Employee;
                    return m.Post.Name.ToString().StartsWith(FilterString);
                }
                else return true;
            };

            EditEmployee = new DelegateCommand(() =>
            {
                editoradd = false;
                selectedEmployee?.BeginEdit();

                var w = new EmployeeEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedEmployee != null);


            AddEmployee = new DelegateCommand(() =>
            {
                editoradd = true;

                Employee employee = new Employee();
                SelectedEmployee = employee;

                var w = new EmployeeEdit();
                w.DataContext = this;
                w.ShowDialog();
            });

            SaveEmployee = new DelegateCommand(() =>
            {
                if (editoradd == true)
                {
                    currentPost = context.Post.Find(selectedEmployee.Post.Id);

                    context.Employee.Add(selectedEmployee);

                    context.SaveChanges();

                    Employees.Add(SelectedEmployee);
                }
                else
                {
                    currentPost = context.Post.Find(selectedEmployee.Post.Id);
                    context.Entry(selectedEmployee).State = EntityState.Modified;
                    context.SaveChanges();
                }
            });

            RemoveEmployee = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    context.Entry(selectedEmployee).State = EntityState.Deleted;
                    Employees.Remove(selectedEmployee);
                    context.SaveChanges();
                }
                else
                {
                    return;
                }
            }, () => selectedEmployee != null);

            CanselEdit = new DelegateCommand(() =>
            {
                if (editoradd == false)
                {
                    if (selectedEmployee == null) return;
                    SelectedEmployee.CancelEdit();
                }
                else
                {
                    return;
                }
            });
        }
    }
}
