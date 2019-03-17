using AutoMapper;
using MedApp.Models;
using MedApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedApp.ViewModels
{
    public class MedRecordViewModel : BindableBase
    {
        MedAppDBEntities context;

        public MedRecord selectedMedRecord { get; set; }
        public BindingList<MedRecord> MedRecords { get; set; }

        public DelegateCommand EditMedRecord { get; }
        public DelegateCommand AddMedRecord { get; }
        public DelegateCommand RemoveMedRecord { get; }
        public DelegateCommand SaveMedRecord { get; }
        public DelegateCommand CanselEdit { get; }

        bool editoradd;
        MedRecord backap;


        public MedRecord SelectedMedRecord
        {
            get { return selectedMedRecord; }
            set
            {
                selectedMedRecord = value;
                RaisePropertyChanged("SelectedMedRecord");
                EditMedRecord.RaiseCanExecuteChanged();
                RemoveMedRecord.RaiseCanExecuteChanged();
                
            }
        }

        public int MyItem_PropertyChanged { get; }

        public BindingList<MedRecord> MedrecordsTake()
        {
            MedRecords = new BindingList<MedRecord>();
            context = new MedAppDBEntities();       
            List<MedRecord> temp = context.MedRecord.ToList();
            foreach (var item in temp)
            {
                MedRecords.Add(item);
            }
            return MedRecords;
        }

        public void DownloadMedRecords()
        {
            MedRecords = new BindingList<MedRecord>();
            context = new MedAppDBEntities();
            List<MedRecord> temp = context.MedRecord.ToList();
            foreach (var item in temp)
            {
                MedRecords.Add(item);
            }          
        }

        public MedRecordViewModel(bool a)
        {
            if (a)
            { DownloadMedRecords(); }

            Mapper.Initialize(cfg => cfg.CreateMap<MedRecord, MedRecord>());

            EditMedRecord = new DelegateCommand(() =>
            {
                
                backap = Mapper.Map<MedRecord, MedRecord>(SelectedMedRecord);
                editoradd = false;

                var w = new MedRecordEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedMedRecord != null);
            
         
            AddMedRecord = new DelegateCommand(() =>
            {                               
               editoradd = true;

               MedRecord NewMedRecord = new MedRecord();
               NewMedRecord.Name = "Новый пациент";
               SelectedMedRecord = NewMedRecord;

               var w = new MedRecordEdit();
               w.DataContext = this;
               w.Show();
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
                    context.SaveChanges();
                    MedRecords.Remove(selectedMedRecord);
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
                    MedRecords.
                    SelectedMedRecord = Mapper.Map<MedRecord, MedRecord>(backap);  
                }
                else
                {
                    return;
                }
            });

            //void MyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            //{
            //    if (e.NewItems != null)
            //    {
            //        foreach (object item in e.NewItems)
            //        {
            //            if (item is MyItem)
            //                ((MyItem)item).PropertyChanged += MyItem_PropertyChanged;
            //        }
            //    }

            //    if (e.OldItems != null)
            //    {
            //        foreach (object item in e.OldItems)
            //        {
            //            if (item is MyItem)
            //                ((MyItem)item).PropertyChanged -= MyItem_PropertyChanged;
            //        }
            //    }
            //}
        }
    }
}
