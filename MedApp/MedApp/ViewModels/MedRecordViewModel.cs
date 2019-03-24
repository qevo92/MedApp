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
    public class MedRecordViewModel : BindableBase
    {
        MedAppDBEntities context;

        public MedRecord selectedMedRecord { get; set; }
        public ObservableCollection<MedRecord> MedRecords { get; set; }

        public Employee currentemployee = new Employee();

        public DelegateCommand EditMedRecord { get; }
        public DelegateCommand AddMedRecord { get; }
        public DelegateCommand RemoveMedRecord { get; }
        public DelegateCommand SaveMedRecord { get; }
        public DelegateCommand CanselEdit { get; }
        public DelegateCommand MedRecordOpen { get; }

        bool editoradd;

        public string _searchKey { get; set; }

        public ICollectionView FilterCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(MedRecords); }
        }

        public Employee Сurrentemployee
        {
            get { return currentemployee; }
            set
            {
                currentemployee = value;
                RaisePropertyChanged("Сurrentemployee");
            }
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

        public MedRecord SelectedMedRecord
        {
            get { return selectedMedRecord; }
            set
            {
                selectedMedRecord?.EndEdit();

                selectedMedRecord = value;
                RaisePropertyChanged("SelectedMedRecord");
                EditMedRecord.RaiseCanExecuteChanged();
                RemoveMedRecord.RaiseCanExecuteChanged();              
            }
        }
 
        public ObservableCollection<MedRecord> MedrecordsTake()
        {
            return MedRecords;
        }

        public void DownloadMedRecords()
        {
            MedRecords = new ObservableCollection<MedRecord>();
            context = new MedAppDBEntities();
            List<MedRecord> temp = context.MedRecord.ToList();
            foreach (var item in temp)
            {
                MedRecords.Add(item);
            }          
        }

        public MedRecordViewModel(Employee employee)
        {           
            DownloadMedRecords();
            currentemployee = employee;

            FilterCollectionView.Filter = i =>
            {
                if (_searchKey == "" || _searchKey == null) return true;
                if (string.IsNullOrEmpty(FilterString)) return true;
                if (_searchKey == "Фамилия")
                {
                    MedRecord m = i as MedRecord;
                    return m.Surname.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Имя")
                {
                    MedRecord m = i as MedRecord;
                    return m.Name.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Отчество")
                {
                    MedRecord m = i as MedRecord;
                    return m.Patronymic.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Город")
                {
                    MedRecord m = i as MedRecord;
                    return m.City.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Улица")
                {
                    MedRecord m = i as MedRecord;
                    return m.Street.ToString().StartsWith(FilterString);
                }
                if (_searchKey == "Дом")
                {
                    MedRecord m = i as MedRecord;
                    return m.Home.ToString().StartsWith(FilterString);
                }
                else return true;
            };

            EditMedRecord = new DelegateCommand(() =>
            {          
                editoradd = false;
                selectedMedRecord?.BeginEdit();

                var w = new MedRecordEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedMedRecord != null);
            
         
            AddMedRecord = new DelegateCommand(() =>
            {                               
               editoradd = true;

               MedRecord NewMedRecord = new MedRecord();
               SelectedMedRecord = NewMedRecord;

               var w = new MedRecordEdit();
               w.DataContext = this;
               w.ShowDialog();
            });                          

            SaveMedRecord = new DelegateCommand(() =>
            {            
               if (editoradd == true)
               {
                  context.MedRecord.Add(selectedMedRecord);

                  context.SaveChanges();

                  MedRecords.Add(SelectedMedRecord);                      
               }
               else
               {
                  context.Entry(selectedMedRecord).State = EntityState.Modified;
                  context.SaveChanges();                       
               }            
            });

            RemoveMedRecord = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    context.Entry(selectedMedRecord).State = EntityState.Deleted;
                    MedRecords.Remove(selectedMedRecord);
                    context.SaveChanges();
                }
                else
                {
                    return;
                }
            }, () => selectedMedRecord != null);

            CanselEdit = new DelegateCommand(() =>
            {
                if (editoradd == false)
                {
                    if (selectedMedRecord == null) return;
                    SelectedMedRecord.CancelEdit();
                }
                else
                {
                    return;
                }
            });

            MedRecordOpen = new DelegateCommand(() =>
            {
                MedRecordOpenViewModel viewmodel = new MedRecordOpenViewModel(SelectedMedRecord, currentemployee);
                var w = new OpenMedRecord();
                w.DataContext = viewmodel;
                w.ShowDialog();
            }, () => currentemployee.Post.Name == "Врач" || currentemployee.Post.Name == "Администратор");
        }
    }
}
