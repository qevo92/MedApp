using MedApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для UCMedRecord.xaml
    /// </summary>
    public partial class UCMedRecord : UserControl
    {
        public UCMedRecord()
        {
            MedRecordViewModel vm = new MedRecordViewModel(true);
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
