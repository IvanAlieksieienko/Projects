using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SERP.Core.Models
{
    public class TaskModel : INotifyPropertyChanged
    {
        public Guid ID { get; set; }
        public string SearchEngine { get; set; }
        public int LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Domain { get; set; }
        public string Keywords { get; set; }

        private int _position = 0;

        private string _status = "";
        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
