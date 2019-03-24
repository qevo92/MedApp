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
    class MedicamentViewModel : BindableBase
    {
        MedAppDBEntities context;

        public Medicament selectedMedicament { get; set; }
        public ObservableCollection<Medicament> Medicaments { get; set; }

        public DelegateCommand EditMedicament { get; }
        public DelegateCommand AddMedicament { get; }
        public DelegateCommand RemoveMedicament { get; }
        public DelegateCommand SaveMedicament { get; }
        public DelegateCommand CanselEdit { get; }

        bool editoradd;

        public ICollectionView FilterCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Medicaments); }
        }

        public Medicament SelectedMedicament
        {
            get { return selectedMedicament; }
            set
            {
                selectedMedicament?.EndEdit();

                selectedMedicament = value;
                RaisePropertyChanged("SelectedMedicament");
                EditMedicament.RaiseCanExecuteChanged();
                RemoveMedicament.RaiseCanExecuteChanged();
            }
        }

        public void DownloadMedicaments()
        {
            Medicaments = new ObservableCollection<Medicament>();
            context = new MedAppDBEntities();
            List<Medicament> temp = context.Medicament.ToList();
            foreach (var item in temp)
            {
                Medicaments.Add(item);
            }
        }

        public ObservableCollection<Medicament> MedicamentsTake()
        {
            return Medicaments;
        }

        public MedicamentViewModel()
        {
            DownloadMedicaments();

            EditMedicament = new DelegateCommand(() =>
            {
                editoradd = false;
                selectedMedicament?.BeginEdit();

                var w = new MedicamentEdit();
                w.DataContext = this;
                w.ShowDialog();

            }, () => selectedMedicament != null);


            AddMedicament = new DelegateCommand(() =>
            {
                editoradd = true;

                Medicament medicament = new Medicament();
                SelectedMedicament = medicament;

                var w = new MedicamentEdit();
                w.DataContext = this;
                w.ShowDialog();
            });

            SaveMedicament = new DelegateCommand(() =>
            {
                if (editoradd == true)
                {
                    context.Medicament.Add(selectedMedicament);

                    context.SaveChanges();

                    Medicaments.Add(SelectedMedicament);
                }
                else
                {
                    context.Entry(SelectedMedicament).State = EntityState.Modified;
                    context.SaveChanges();
                }
            });

            RemoveMedicament = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Удалить?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    context.Entry(selectedMedicament).State = EntityState.Deleted;
                    context.SaveChanges();
                    Medicaments.Remove(selectedMedicament);
                }
                else
                {
                    return;
                }
            }, () => selectedMedicament != null);

            CanselEdit = new DelegateCommand(() =>
            {
                if (editoradd == false)
                {
                    if (selectedMedicament == null) return;
                    SelectedMedicament.CancelEdit();
                }
                else
                {
                    return;
                }
            });
        }
    }

}
