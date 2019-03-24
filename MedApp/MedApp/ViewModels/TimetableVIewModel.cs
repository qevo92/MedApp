using MedApp.Models;
using MedApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MedApp.ViewModels
{
    class TimetableVIewModel : BindableBase
    {
        MedAppDBEntities context;

        public Timetable selectedTimetable { get; set; }
        public ObservableCollection<Timetable> Timetables { get; set; }

        public DelegateCommand EditTimetable { get; }
        public DelegateCommand AddTimetable { get; }
        public DelegateCommand RemoveTimetable { get; }
        public DelegateCommand SaveTimetable { get; }
        public DelegateCommand CanselEdit { get; }

        bool editoradd;

        public string _searchKey { get; set; }

        public ICollectionView FilterCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Timetables); }
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

        //public Timetable SelectedTimetable
        //{
        //    get { return selectedTimetable; }
        //    set
        //    {
        //        selectedTimetable?.EndEdit();

        //        selectedTimetable = value;
        //        RaisePropertyChanged("SelectedTimetable");
        //        EditTimetable.RaiseCanExecuteChanged();
        //        RemoveTimetable.RaiseCanExecuteChanged();

        //        selectedTimetable?.BeginEdit();
        //    }
        //}

        public ObservableCollection<Timetable> TimetableTake()
        {
            Timetables = new ObservableCollection<Timetable>();
            context = new MedAppDBEntities();
            List<Timetable> temp = context.Timetable.ToList();
            foreach (var item in temp)
            {
                Timetables.Add(item);
            }
            return Timetables;
        }

        public void DownloadTimetables()
        {
            Timetables = new ObservableCollection<Timetable>();
            context = new MedAppDBEntities();
            List<Timetable> temp = context.Timetable.ToList();
            foreach (var item in temp)
            {
                Timetables.Add(item);
            }
        }

        public TimetableVIewModel(bool a)
        {
            if (a)
            { DownloadTimetables(); }

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

            EditTimetable = new DelegateCommand(() =>
            {
                editoradd = false;

                var w = new MedRecordEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedTimetable != null);


            //AddTimetable = new DelegateCommand(() =>
            //{
            //    editoradd = true;

            //    Timetable timetable = new Timetable();
            //    SelectedTimetable = timetable;

            //    var w = new MedRecordEdit();
            //    w.DataContext = this;
            //    w.Show();
            //});

            //SaveTimetable = new DelegateCommand(() =>
            //{
            //    if (editoradd == true)
            //    {
            //        context.Timetable.Add(SelectedTimetable);

            //        context.SaveChanges();

            //        Timetables.Add(SelectedTimetable);
            //    }
            //    else
            //    {
            //        context.Entry(selectedTimetable).State = EntityState.Modified;
            //        context.SaveChanges();
            //    }
            //});

            //RemoveTimetable = new DelegateCommand(() =>
            //{
            //    if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //    {
            //        context.Entry(SelectedTimetable).State = EntityState.Deleted;
            //        Timetables.Remove(SelectedTimetable);
            //        context.SaveChanges();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}, () => selectedTimetable != null);

            //CanselEdit = new DelegateCommand(() =>
            //{
            //    if (editoradd == false)
            //    {
            //        if (selectedTimetable == null) return;
            //        SelectedTimetable.CancelEdit();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //});

        }
    }
}
