using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_GYM_APP.Views.Classes
{
    public class EventModel : INotifyPropertyChanged
    {
        private string _Status;

        public string Name { get; set; }

        public string Status
        {
            get => _Status;
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(IsCompleted));
                    OnPropertyChanged(nameof(StatusDisplay));
                }
            }
        }
        public bool IsCompleted => Status == "Complete";
        public string StatusDisplay => IsCompleted ? "Complete ✅" : "Incomplete ❌";
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
