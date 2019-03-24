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
using System.Windows;
using System.Windows.Data;

namespace MedApp.ViewModels
{
    public class DiagnosisViewModel : BindableBase
    {
        MedAppDBEntities context;

        public Diagnosis selectedDiagnos { get; set; }
        public ObservableCollection<Diagnosis> Diagnosings { get; set; }

        public DelegateCommand EditDiagnos { get; }
        public DelegateCommand AddDiagnos { get; }
        public DelegateCommand RemoveDiagnos { get; }
        public DelegateCommand SaveDiagnos { get; }
        public DelegateCommand CanselEdit { get; }

        bool editoradd;

        public ICollectionView FilterCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Diagnosings); }
        }

        public Diagnosis SelectedDiagnos
        {
            get { return selectedDiagnos; }
            set
            {
                selectedDiagnos?.EndEdit();

                selectedDiagnos = value;
                RaisePropertyChanged("SelectedDiagnos");
                EditDiagnos.RaiseCanExecuteChanged();
                RemoveDiagnos.RaiseCanExecuteChanged();
            }
        }

        public void DownloadDiagnosings()
        {
            Diagnosings = new ObservableCollection<Diagnosis>();
            context = new MedAppDBEntities();          
            List<Diagnosis> temp = context.Diagnosis.ToList();
            foreach (var item in temp)
            {
                Diagnosings.Add(item);
            }          
        }

        public ObservableCollection<Diagnosis> DiagnosisTake()
        {
           return Diagnosings;
        }

        public DiagnosisViewModel()
        {                     
           DownloadDiagnosings();

            EditDiagnos = new DelegateCommand(() =>
            {
                editoradd = false;
                selectedDiagnos?.BeginEdit();

                var w = new DiagnosisEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedDiagnos != null);


            AddDiagnos = new DelegateCommand(() =>
            {
                editoradd = true;

                Diagnosis diagnosis = new Diagnosis();
                SelectedDiagnos = diagnosis;

                var w = new DiagnosisEdit();
                w.DataContext = this;
                w.ShowDialog();
            });

            SaveDiagnos = new DelegateCommand(() =>
            {
                if (editoradd == true)
                {
                    context.Diagnosis.Add(selectedDiagnos);

                    context.SaveChanges();

                    Diagnosings.Add(SelectedDiagnos);
                }
                else
                {
                    context.Entry(SelectedDiagnos).State = EntityState.Modified;
                    context.SaveChanges();
                }
            });

            RemoveDiagnos = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    context.Entry(selectedDiagnos).State = EntityState.Deleted;
                    context.SaveChanges();
                    Diagnosings.Remove(selectedDiagnos);
                }
                else
                {
                    return;
                }
            }, () => selectedDiagnos != null);

            CanselEdit = new DelegateCommand(() =>
            {
                if (editoradd == false)
                {
                    if (selectedDiagnos == null) return;
                    SelectedDiagnos.CancelEdit();
                }
                else
                {
                    return;
                }
            });
        }
    }
}
