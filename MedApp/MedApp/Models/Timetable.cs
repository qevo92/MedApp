//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedApp.Models
{
    using System;
    using System.Collections.Generic;
    using Prism.Mvvm;
    using System.ComponentModel;
    public partial class Timetable : BindableBase
    {
        private int  _Id;
        public int Id 
        { 
           get
           {
              return _Id;
           } 
           set
           {
              if(_Id != value)
              {
                _Id = value;
                RaisePropertyChanged("Id");
              }
           }
        }
     
        private int  _IdDay;
        public int IdDay 
        { 
           get
           {
              return _IdDay;
           } 
           set
           {
              if(_IdDay != value)
              {
                _IdDay = value;
                RaisePropertyChanged("IdDay");
              }
           }
        }
     
        private int  _IdEmployee;
        public int IdEmployee 
        { 
           get
           {
              return _IdEmployee;
           } 
           set
           {
              if(_IdEmployee != value)
              {
                _IdEmployee = value;
                RaisePropertyChanged("IdEmployee");
              }
           }
        }
     
        private int  _IdShift;
        public int IdShift 
        { 
           get
           {
              return _IdShift;
           } 
           set
           {
              if(_IdShift != value)
              {
                _IdShift = value;
                RaisePropertyChanged("IdShift");
              }
           }
        }
     
    
        private Day _Day;
        public virtual Day Day 
        { 
          get
          {
              return _Day;
          } 
          set
          {
             if(_Day != value)
             {
                _Day = value;
                RaisePropertyChanged("Day");
             }
          }
        }
     
        private Employee _Employee;
        public virtual Employee Employee 
        { 
          get
          {
              return _Employee;
          } 
          set
          {
             if(_Employee != value)
             {
                _Employee = value;
                RaisePropertyChanged("Employee");
             }
          }
        }
     
        private Shift _Shift;
        public virtual Shift Shift 
        { 
          get
          {
              return _Shift;
          } 
          set
          {
             if(_Shift != value)
             {
                _Shift = value;
                RaisePropertyChanged("Shift");
             }
          }
        }
     
    }
}