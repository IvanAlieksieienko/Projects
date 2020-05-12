using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Core.Models
{
    public class TaskModel : INotifyPropertyChanged
    {
        private SearchMethod _searchMethod;
        public int SearchMethod
        {
            get => (int)_searchMethod;
            set => _searchMethod = (SearchMethod)value;
        }
        public string SearchValue { get; set; }
        public string BasicPath { get; set; }
        public string FileMask { get; set; }
        private SizeConditionEnum _sizeCondition;
        public int SizeCondition
        {
            get => (int)_sizeCondition;
            set => _sizeCondition = (SizeConditionEnum)value;
        }
        private UnitOfMeasure _unitOfMeasure;
        public int UnitOfMeasure
        {
            get => (int)_unitOfMeasure;
            set => _unitOfMeasure = (UnitOfMeasure)value;
        }
        public long SizeValue { get; set; } = 0;
        private DateCondition _dateCondition;
        public int DateCondition
        {
            get => (int)_dateCondition;
            set => _dateCondition = (DateCondition)value;
        }
        public DateTime DateValue { get; set; } = DateTime.Today;
        private TaskState _taskState;
        public int TaskState
        {
            get => (int)_taskState;
            set => _taskState = (TaskState)value;
        }
        private string _taskInProgressTime = "0";
        public string TaskInProcessTime
        {
            get => _taskInProgressTime;
            set
            {
                _taskInProgressTime = value;
                OnPropertyChanged(nameof(TaskInProcessTime)); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
