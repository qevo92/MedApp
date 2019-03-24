using MedApp.Models;
using MedApp.Views;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.Office.Interop.Excel;
using MedApp.Behaviors;
using System;

namespace MedApp.ViewModels
{
    public class MedRecordOpenViewModel : BindableBase
    {
        MedAppDBEntities context;

        DiagnosisViewModel DiagnosisViewModel = new DiagnosisViewModel();
        MedicamentViewModel MedicamentViewModel = new MedicamentViewModel();

        public MedRecord currentMedRecord { get; set; }
        public Employee currentEmployee { get; set; }
        public Medicament currentMedicament { get; set; }
        public Diagnosis currentDiagnosis{ get; set; }

        public Xrays selectedXrays { get; set; }
        public Visiting selectedVisiting { get; set; }

        public ObservableCollection<Xrays> Photos { get; set; }
        public ObservableCollection<Visiting> Visitings { get; set; }
        public ObservableCollection<Diagnosis> Diagnosings { get; set; }
        public ObservableCollection<Medicament> Medicaments { get; set; }

        public DelegateCommand EditXrays { get; }
        public DelegateCommand AddXrays { get; }
        public DelegateCommand RemoveXrays { get; }
        public DelegateCommand SaveXrays { get; }
        public DelegateCommand CanselXrays { get; }
        public DelegateCommand AddImage { get; }

        public DelegateCommand EditVisiting { get; }
        public DelegateCommand AddVisiting { get; }
        public DelegateCommand RemoveVisiting { get; }
        public DelegateCommand SaveVisiting { get; }
        public DelegateCommand CanselVisiting { get; }        
        public DelegateCommand ExportVisiting { get;  set; }

        bool editoradd;

        public ICollectionView PhotosCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Photos); }
        }
        public ICollectionView VisitingsCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Visitings); }
        }

        public Xrays SelectedXrays
        {
            get { return selectedXrays; }
            set
            {
                selectedXrays?.EndEdit();

                selectedXrays = value;
                RaisePropertyChanged("SelectedXrays");
                EditXrays.RaiseCanExecuteChanged();
                RemoveXrays.RaiseCanExecuteChanged();              
            }
        }

        public Visiting SelectedVisiting
        {
            get { return selectedVisiting; }
            set
            {
                selectedVisiting?.EndEdit();

                selectedVisiting = value;
                RaisePropertyChanged("SelectedVisiting");
                EditVisiting.RaiseCanExecuteChanged();
                RemoveVisiting.RaiseCanExecuteChanged();
            }
        }      

        public void DownloadXrays(MedRecord medRecord)
        {
            Photos = new ObservableCollection<Xrays>();
            context = new MedAppDBEntities();
            List<Xrays> temp = context.Xrays.Where(i => i.IdMedRecord == medRecord.Id).ToList();
            foreach (var item in temp)
            {
                Photos.Add(item);
            }
        }

        public void DownloadVisiting(MedRecord medRecord)
        {
            Visitings = new ObservableCollection<Visiting>();
            context = new MedAppDBEntities();
            List<Visiting> temp = context.Visiting.Where(i => i.IDMedRecord == medRecord.Id).ToList();
            foreach (var item in temp)
            {
                Visitings.Add(item);
            }
        }

        private byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitMap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitMap.RawFormat;
            var imageToConvert = Image.FromFile(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, bmpFormat);
                return ms.ToArray();
            }
        }
        public BitmapImage ConvertByteArrayToImage(byte[] array)
        {
            if (array != null)
            {
                using (var ms = new MemoryStream(array, 0, array.Length))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            return null;
        }
        private void Export(object gridExcel)
        {
            if (gridExcel != null)
            {
                DataGridExport.ExportDataGrid(gridExcel);
            }
        }

        public MedRecordOpenViewModel(MedRecord medRecord, Employee employee)
        {
            currentMedRecord = medRecord;
            currentEmployee = employee;

            DownloadXrays(medRecord);
            DownloadVisiting(medRecord);
            Diagnosings = DiagnosisViewModel.DiagnosisTake();
            Medicaments = MedicamentViewModel.MedicamentsTake();

            EditXrays = new DelegateCommand(() =>
            {
                editoradd = false;

                selectedXrays?.BeginEdit();
                var w = new XraysEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedXrays != null);


            AddXrays = new DelegateCommand(() =>
            {
                editoradd = true;

                Xrays xrays = new Xrays();
                SelectedXrays = xrays;

                var w = new XraysEdit();
                w.DataContext = this;
                w.Show();
            });

            SaveXrays = new DelegateCommand(() =>
            {
                if (editoradd == true)
                {
                    selectedXrays.IdMedRecord = currentMedRecord.Id;

                    context.Xrays.Add(selectedXrays);

                    context.SaveChanges();

                    Photos.Add(SelectedXrays);
                }
                else
                {
                    context.Entry(selectedXrays).State = EntityState.Modified;
                    context.SaveChanges();
                }
            },() => selectedXrays.Photo != null);

            RemoveXrays = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    context.Entry(selectedXrays).State = EntityState.Deleted;
                    Photos.Remove(selectedXrays);
                    context.SaveChanges();
                }
                else
                {
                    return;
                }
            }, () => selectedXrays != null);

            CanselXrays = new DelegateCommand(() =>
            {
                if (editoradd == false)
                {
                    if (selectedXrays == null) return;
                    SelectedXrays.CancelEdit();
                }
                else
                {
                    return;
                }
            });
    
            AddImage = new DelegateCommand(() =>
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = "";
                dlg.Filter = "Image files (*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp|All Files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    string selectedFileName = dlg.FileName;
                    selectedXrays.Photo = ConvertImageToByteArray(selectedFileName);
                    SaveXrays.RaiseCanExecuteChanged();
                }               
            });

            #region Visiting

            EditVisiting = new DelegateCommand(() =>
            {
                editoradd = false;

                var w = new VisitingEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedVisiting != null);


            AddVisiting = new DelegateCommand(() =>
            {
                editoradd = true;

                Visiting visiting = new Visiting();
                SelectedVisiting = visiting;

                var w = new VisitingEdit();
                w.DataContext = this;
                w.Show();
            });

            SaveVisiting = new DelegateCommand(() =>
            {
                if (editoradd == true)
                {
                    selectedVisiting.IDEmployee = currentEmployee.Id;
                    selectedVisiting.IDMedRecord = currentMedRecord.Id;

                    currentMedRecord = context.MedRecord.Find(currentMedRecord.Id);
                    currentEmployee = context.Employee.Find(currentEmployee.Id);
                    currentMedicament = context.Medicament.Find(selectedVisiting.IDMedicament);
                    currentDiagnosis = context.Diagnosis.Find(selectedVisiting.IDDiagnosis);
                    //Diagnosings = DiagnosisViewModel.DiagnosisTake();
                    //Medicaments = MedicamentViewModel.MedicamentsTake();

                    context.Visiting.Add(selectedVisiting);                 

                    context.SaveChanges();

                    Visitings.Add(SelectedVisiting);
                }
                else
                {
                    currentMedRecord = context.MedRecord.Find(currentMedRecord.Id);
                    currentEmployee = context.Employee.Find(currentEmployee.Id);
                    currentMedicament = context.Medicament.Find(selectedVisiting.IDMedicament);
                    currentDiagnosis = context.Diagnosis.Find(selectedVisiting.IDDiagnosis);

                    context.Entry(selectedVisiting).State = EntityState.Modified;
                    context.SaveChanges();
                }
            });

           
            RemoveVisiting  = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    context.Entry(selectedVisiting).State = EntityState.Deleted;
                    Visitings.Remove(selectedVisiting);
                    context.SaveChanges();
                }
                else
                {
                    return;
                }
            }, () => selectedVisiting != null);

            CanselVisiting = new DelegateCommand(() =>
            {
                if (editoradd == false)
                {
                    if (selectedVisiting == null) return;
                    SelectedVisiting.CancelEdit();
                }
                else
                {
                    return;
                }
            });        

            ExportVisiting = new DelegateCommand(() =>
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();              
                    
                    Workbook workbook = exApp.Workbooks.Add(Type.Missing);
                    
                    Worksheet workSheet = (Worksheet)exApp.ActiveSheet;

                    workSheet.Cells[1, 1] = "ФИО пациента";
                    workSheet.Cells[1, 2] = "ФИО врача";
                    workSheet.Cells[1, 3] = "Дата";
                    workSheet.Cells[1, 4] = "Жалобы";
                    workSheet.Cells[1, 5] = "Начало болезни";
                    workSheet.Cells[1, 6] = "Текущее состояние";
                    workSheet.Cells[1, 7] = "Дополнительно";
                    workSheet.Cells[1, 8] = "Медикамент";
                    workSheet.Cells[1, 9] = "Диагноз";

                    int rowExcel = 2;

                    foreach (var item in Visitings)
                    {
                        workSheet.Cells[rowExcel, "A"] = item.MedRecord.Surname + item.MedRecord.Name + item.MedRecord.Patronymic;
                        workSheet.Cells[rowExcel, "B"] = item.Employee.Surname + item.Employee.Name + item.Employee.Patronymic;
                        workSheet.Cells[rowExcel, "C"] = item.Date;
                        workSheet.Cells[rowExcel, "D"] = item.Complaints;
                        workSheet.Cells[rowExcel, "E"] = item.StartDisease;
                        workSheet.Cells[rowExcel, "F"] = item.StatePraesens;
                        workSheet.Cells[rowExcel, "G"] = item.Additionally;
                        workSheet.Cells[rowExcel, "H"] = item.Medicament.Name;
                        workSheet.Cells[rowExcel, "I"] = item.Diagnosis.Name;

                        ++rowExcel;
                    }

                    //workbook.Close();
                    exApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(exApp);                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка записи в Excel");
                }
            });
            #endregion
        }
    }
}
