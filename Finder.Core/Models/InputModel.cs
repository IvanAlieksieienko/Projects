using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace Finder.Core.Models
{
    public class InputModel : INotifyPropertyChanged
    {
        public TaskModel Task { get; set; }
        public ObservableCollection<SizeConditionEnum> SizeConditionEnums { get; set; } = new ObservableCollection<SizeConditionEnum>() 
        { SizeConditionEnum.BiggerThan, SizeConditionEnum.SmallerThan, SizeConditionEnum.Equal };

        private SizeConditionEnum _selectedCondition = SizeConditionEnum.BiggerThan;
        public SizeConditionEnum SelectedCondition 
        {
            get => _selectedCondition;
            set => _selectedCondition = value; // reaction to action here!!!
        }
        public ObservableCollection<UnitOfMeasure> UnitsOfMeasure { get; set; } = new ObservableCollection<UnitOfMeasure>()
        { UnitOfMeasure.Bit, UnitOfMeasure.Byte, UnitOfMeasure.KiloByte, UnitOfMeasure.MegaByte, UnitOfMeasure.GigaByte, UnitOfMeasure.TeraByte };

        private UnitOfMeasure _selectedMeasure = UnitOfMeasure.KiloByte;
        public UnitOfMeasure SelectedMeasure
        {
            get => _selectedMeasure;
            set => _selectedMeasure = value; // !!!
        }
        public ObservableCollection<DateCondition> DateConditions { get; set; } = new ObservableCollection<DateCondition>()
        { DateCondition.LaterThan, DateCondition.EarlierThan, DateCondition.InThatDay};

        private DateCondition _selectedDateCondition = DateCondition.EarlierThan;
        public DateCondition SelectedDateCondition
        {
            get => _selectedDateCondition;
            set => _selectedDateCondition = value;
        }
        private bool _isPathSelected = false;
        public bool IsPathSelected
        {
            get => _isPathSelected;
            set
            {
                _isPathSelected = value;
                OnPropertyChanged(nameof(IsPathSelected));
            }
        }
        private bool _isStarted = false;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;
                OnPropertyChanged(nameof(IsStarted));
            }
        }
        private bool _isFinished = false;
        public bool IsFinished
        {
            get => _isFinished;
            set
            {
                _isFinished = value;
                OnPropertyChanged(nameof(IsFinished));
            }
        }
        private bool _isShowResults = false;
        public bool IsShowResults
        {
            get => _isShowResults;
            set
            {
                _isShowResults = value;
                OnPropertyChanged(nameof(IsShowResults));
            }
        }
        private bool _isSuccess = false;
        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                _isSuccess = value;
                OnPropertyChanged(nameof(IsSuccess));
            }
        }
        private string _currentTime = "0";
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        public Timer Timer = new Timer();
        public Stopwatch Stopwatch = new Stopwatch();

        public InputModel()
        {
            Timer.Elapsed += TimerTick;
            Timer.Interval = TimeSpan.FromMilliseconds(1).TotalMilliseconds;
        }

        private ObservableCollection<string> _searchResults;
        public ObservableCollection<string> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                if (_searchResults.Count > 0) IsSuccess = true;
                else IsSuccess = false;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        public void TimerTick(object sender, EventArgs e)
        {
            if (Stopwatch.IsRunning)
            {
                TimeSpan ts = Stopwatch.Elapsed;
                CurrentTime = String.Format("{0:00}:{1:00}:{2:00} ", ts.Minutes, ts.Seconds, ts.Milliseconds);
            }
        }

        public async void StartStopwatch()
        {
            Timer.Start();
            Stopwatch.Start();
        }

        public async void StopStopwatch()
        {
            if (Stopwatch.IsRunning) Stopwatch.Stop();
            if (Timer.Enabled) Timer.Stop();
            Task.TaskInProcessTime = CurrentTime;
        }

        public async void ResetStopwatch()
        {
            Stopwatch.Reset();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
