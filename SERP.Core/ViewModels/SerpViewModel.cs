using SERP.Core.Commands;
using SERP.Core.IRepositories;
using SERP.Core.IServices;
using SERP.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SERP.Core.ViewModels
{
    public class SerpViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();
        private List<LocationModel> _locationsGoogle;
        private List<LocationModel> _locationsBing;
        private List<LocationModel> _locations;
        private LocationModel _selectedLocation;
        private string[] _searchEngines = new string[3] { "Search Engine", "Google", "Bing" };
        private string _selectedEngine = "Search Engine";
        private ISerpService _serpService;
        private ISerpRepository _serpRepository;
        private bool _spinnerSpin;
        private string _spinnerVisibility;
        private string _domainName = "Domain name";
        private string _keywords = "Keywords";

        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged("Tasks");
            }
        }
        public List<LocationModel> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged("Locations");
            }
        }

        public LocationModel SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }

        public string[] SearchEngines
        {
            get { return _searchEngines; }
            set
            {
                _searchEngines = value;
                OnPropertyChanged("SearchEngines");
            }
        }

        public string SelectedEngine
        {
            get { return _selectedEngine; }
            set
            {
                _selectedEngine = value;
                OnPropertyChanged("SelectedEngine");
                var engine = _selectedEngine.ToLower();
                if (engine == "google" || engine == "bing")
                    SetLocations(engine);
            }
        }

        public bool SpinnerSpin 
        {
            get { return _spinnerSpin; }
            set
            {
                _spinnerSpin = value;
                OnPropertyChanged("SpinnerSpin");
            }
        }
        public string SpinnerVisibility
        {
            get { return _spinnerVisibility; }
            set
            {
                _spinnerVisibility = value;
                OnPropertyChanged("SpinnerVisibility");
            }
        }

        public string DomainName
        {
            get { return _domainName; }
            set
            {
                _domainName = value;
                OnPropertyChanged("DomainName");
            }
        }
        public string Keywords
        {
            get { return _keywords; }
            set
            {
                _keywords = value;
                OnPropertyChanged("Keywords");
            }
        }

        private RelayCommand _sendRequest;
        public RelayCommand SendRequest
        {
            get
            {
                return _sendRequest ??
                    (_sendRequest = new RelayCommand(async obj =>
                    {
                        if ((SelectedEngine == "Google" || SelectedEngine == "Bing") && SelectedLocation != null && (DomainName.Length > 0 && DomainName != "Domain name") && Keywords.Length > 0)
                        {
                            TaskModel newTask = new TaskModel();
                            newTask.ID = Guid.NewGuid();
                            newTask.SearchEngine = SelectedEngine.ToLower();
                            newTask.LocationCode = SelectedLocation.LocationCode;
                            newTask.LocationName = SelectedLocation.LocationName; 
                            newTask.Domain = DomainName;
                            var keywords = "";
                            var keywordsArray = Keywords.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                            foreach (var elementOfArray in keywordsArray)
                            {
                                keywords += String.Join(" ", elementOfArray.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                                keywords += " ";
                            }
                            keywords = keywords.Substring(0, keywords.Length - 1);
                            newTask.Keywords = keywords;
                            newTask.Status = "Processing";
                            Tasks.Add(newTask);
                            await _serpRepository.Add(newTask);
                            newTask.Position = await _serpService.GetPosition(newTask);
                            newTask.Status = "Tracked";
                            await _serpRepository.Update(newTask);
                        }
                    }));
            }
        }


        public SerpViewModel(ISerpService serpService, ISerpRepository serpRepository)
        {
            _serpService = serpService;
            _serpRepository = serpRepository;
            GetLocations();
        }

        private async Task GetLocations()
        {
            SpinnerVisibility = "Visible";
            SpinnerSpin = true;
            _locationsGoogle = await _serpService.GetLocations("google");
            _locationsBing = await _serpService.GetLocations("bing");
            SpinnerVisibility = "Hidden";
            SpinnerSpin = false;
        }

        private async Task SetLocations(string engine)
        {
            Locations = engine == "google" ? _locationsGoogle : engine == "bing" ? _locationsBing : null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
