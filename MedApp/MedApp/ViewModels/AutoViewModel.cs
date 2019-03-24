using MedApp.Models;
using MedApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Windows;


namespace MedApp.ViewModels
{
    public class AutoViewModel : BindableBase
    {
        private string _Login;
        public string Login
        {
            get
            {
                return _Login;
            }
            set
            {
                if (_Login != value)
                {
                    _Login = value;
                    RaisePropertyChanged("Login");
                }
                LoginPasswordCheck.RaiseCanExecuteChanged();
            }
        }

        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    RaisePropertyChanged("Password");
                }
                LoginPasswordCheck.RaiseCanExecuteChanged();
            }
        }

        MedAppDBEntities context;

        public event EventHandler OnRequestClose;

        public MainViewModel mainViewModel;
        public DelegateCommand LoginPasswordCheck { get; }

        public AutoViewModel()
        {
            LoginPasswordCheck = new DelegateCommand(() =>
            {
                using (context = new MedAppDBEntities())
                {

                    Employee temp = context.Employee.FirstOrDefault(i => i.Login == Login && i.Password == Password);
                    if (temp == null)
                    { MessageBox.Show("Неправильный логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK); }
                    else
                    {
                        mainViewModel = new MainViewModel(temp);
                        var w = new MainWindow(mainViewModel);
                        w.Show();
                        OnRequestClose(this, new EventArgs());
                    };

                }


            }, () => Login != null && Password != null);
        }
    }
}