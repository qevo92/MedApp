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
    public partial class Ticket : BindableBase
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
     
        private Nullable<int>  _IdMedRecord;
        public Nullable<int> IdMedRecord 
        { 
           get
           {
              return _IdMedRecord;
           } 
           set
           {
              if(_IdMedRecord != value)
              {
                _IdMedRecord = value;
                RaisePropertyChanged("IdMedRecord");
              }
           }
        }
     
        private System.DateTime  _Date;
        public System.DateTime Date 
        { 
           get
           {
              return _Date;
           } 
           set
           {
              if(_Date != value)
              {
                _Date = value;
                RaisePropertyChanged("Date");
              }
           }
        }
     
        private System.TimeSpan  _Time;
        public System.TimeSpan Time 
        { 
           get
           {
              return _Time;
           } 
           set
           {
              if(_Time != value)
              {
                _Time = value;
                RaisePropertyChanged("Time");
              }
           }
        }
     
        private int  _QueueNum;
        public int QueueNum 
        { 
           get
           {
              return _QueueNum;
           } 
           set
           {
              if(_QueueNum != value)
              {
                _QueueNum = value;
                RaisePropertyChanged("QueueNum");
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
     
        private MedRecord _MedRecord;
        public virtual MedRecord MedRecord 
        { 
          get
          {
              return _MedRecord;
          } 
          set
          {
             if(_MedRecord != value)
             {
                _MedRecord = value;
                RaisePropertyChanged("MedRecord");
             }
          }
        }
     
    }
}
