using Finder.Core.Commands;
using Finder.Core.Interfaces.IServices;
using Finder.Core.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;

namespace Finder.Core.ViewModels
{
    public class ContentPageVM : INotifyPropertyChanged
    {
        private ISearchService<TaskModel> _searchService;
        private ObservableCollection<InputModel> _tasks;
        private string _modalMessage = string.Empty;
        public string ModalMessage
        {
            get => _modalMessage;
            set
            {
                _modalMessage = value;
                OnPropertyChanged(nameof(ModalMessage));
            }
        }
        private bool _isMenuOpened = false;
        public bool IsMenuOpened
        {
            get => _isMenuOpened;
            set
            {
                _isMenuOpened = value;
                OnPropertyChanged(nameof(IsMenuOpened));
            }
        }
        public ICommand SetPathCommand { get; set; }
        public ICommand ShowPathCommand { get; set; }
        public ICommand GoCommand { get; set; }
        public ICommand OpenMenuCommand { get; set; }
        public ICommand CloseMenuCommand { get; set; }
        public ICommand CreateFileTaskCommand { get; set; }
        public ICommand CreateContentTaskCommand { get; set; }
        public ICommand CreateRegExTaskCommand { get; set; }
        public ICommand ShowResultsCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }

        public ObservableCollection<InputModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public ContentPageVM(ISearchService<TaskModel> searchService)
        {
            Tasks = new ObservableCollection<InputModel>();
            SetPathCommand = new Command(SetPathMethod);
            ShowPathCommand = new Command(ShowPathMethod);
            GoCommand = new Command(GoMethod);
            OpenMenuCommand = new Command(OpenMenuMethod);
            CloseMenuCommand = new Command(CloseMenuMethod);
            CreateFileTaskCommand = new Command(CreateFileTaskMethod);
            CreateContentTaskCommand = new Command(CreateContentTaskMethod);
            CreateRegExTaskCommand = new Command(CreateRegExTaskMethod);
            ShowResultsCommand = new Command(ShowResultsMethod);
            OpenFileCommand = new Command(OpenFileMethod);
            OpenFolderCommand = new Command(OpenFolderMethod);
            _searchService = searchService;
        }

        public async void CreateNewTask(SearchMethod searchMethod)
        {
            TaskModel newTask = new TaskModel();
            newTask.SearchMethod = (int)searchMethod;
            newTask.SearchValue = searchMethod == SearchMethod.File ? "file_name" :
                searchMethod == SearchMethod.Content ? "File content\nYou can use multiple lines" :
                searchMethod == SearchMethod.RegEx ? @"^Regular Expression$" : "file_name";
            newTask.FileMask = searchMethod == SearchMethod.File ? "only_extension" : ".docx | .txt | .doc";
            InputModel newInput = new InputModel();
            newInput.Task = newTask;
            Tasks.Add(newInput);
        }

        private async void SetPathMethod(object sender)
        {
            var inputModel = Tasks.FirstOrDefault(b=> b == (InputModel)sender);
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    var path = folder.SelectedPath;
                    inputModel.Task.BasicPath = path;
                    inputModel.IsPathSelected = true;

                }
            }
        }

        private async void ShowPathMethod(object sender)
        {
            var inputModel = (InputModel)sender;
            ModalMessage = inputModel.Task.BasicPath;
            MessageBox.Show(ModalMessage, "Full path");
        }

        private async void GoMethod(object sender)
        {
            var inputModel = (InputModel)sender;
            var error = string.Empty;
            if (string.IsNullOrEmpty(inputModel.Task.SearchValue))
                error += "Search value (file name, file content, regular expression\n";
            if (!inputModel.IsPathSelected || string.IsNullOrEmpty(inputModel.Task.BasicPath))
                error += "Path is not selected!\n";
            if (string.IsNullOrEmpty(inputModel.Task.FileMask))
                error += "File mask is empty!\n";
            Regex regex;
            switch (inputModel.Task.SearchMethod)
            {   
                case 0:
                    regex = new Regex(@"^\w*$");
                    if (!regex.IsMatch(inputModel.Task.FileMask))
                        error += "File extension contains unsupported symbols\n";
                    break;
                case 1:
                    regex = new Regex(@"^[^<>:;,*?\u0022|/]*\.(txt|doc|docx)$");
                    if (!regex.IsMatch(inputModel.Task.FileMask))
                        error += "File mask contains unsupported symbols or extension. Must be (file name or empty).(txt or doc or docx)\n";
                    break;
                case 3:
                    regex = new Regex(@"^[^<>:;,*?\u0022|/]*\.(txt|doc|docx)$");
                    if (!regex.IsMatch(inputModel.Task.FileMask))
                        error += "File mask contains unsupported symbols or extension. Must be (file name or empty).(txt or doc or docx)\n";
                    break;
            }
            if (inputModel.Task.SizeValue == 0 && (inputModel.SelectedCondition == SizeConditionEnum.Equal || inputModel.SelectedCondition == SizeConditionEnum.SmallerThan))
                error += "Size cannot be 0 or smaller!\n";
            if (inputModel.Task.DateValue > DateTime.Now && inputModel.SelectedDateCondition == DateCondition.LaterThan)
                error += "Date cannot be in future!\n";
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Error");
                return;
            }
            TaskModel task = new TaskModel();
            task.SearchValue = inputModel.Task.SearchValue;
            task.BasicPath = inputModel.Task.BasicPath;
            task.FileMask = inputModel.Task.FileMask;
            task.SizeCondition = (int)inputModel.SelectedCondition;
            task.UnitOfMeasure = (int)inputModel.SelectedMeasure;
            task.SizeValue = inputModel.Task.SizeValue;
            task.DateCondition = (int)inputModel.SelectedDateCondition;
            task.DateValue = inputModel.Task.DateValue;
            task.SearchMethod = inputModel.Task.SearchMethod;
            inputModel.IsStarted = true; // hide additional search properties, go should be removed
            inputModel.StartStopwatch();
            var result = await Task.Run(() => _searchService.StartTask(task));
            inputModel.SearchResults = new ObservableCollection<string>(result);
            inputModel.StopStopwatch();
            inputModel.Task.TaskState = (int)TaskState.Ready;
            inputModel.IsFinished = true; // make button show results visible
        }

        private async void ShowResultsMethod(object sender)
        {
            var inputModel = (InputModel)sender;
            inputModel.IsShowResults = !inputModel.IsShowResults;
        }

        private async void OpenMenuMethod(object sender)
        {
            IsMenuOpened = true;
        }
        private async void CloseMenuMethod(object sender)
        {
            IsMenuOpened = false;
        }
        private async void CreateFileTaskMethod(object sender)
        {
            CreateNewTask(SearchMethod.File);
        }
        private async void CreateContentTaskMethod(object sender)
        {
            CreateNewTask(SearchMethod.Content);
        }
        private async void CreateRegExTaskMethod(object sender)
        {
            CreateNewTask(SearchMethod.RegEx);
        }

        private async void OpenFolderMethod(object sender)
        {
            var path = (string)sender;
            var fileInfo = new FileInfo(path);
            var directory = fileInfo.Directory;
            Process.Start(directory.FullName);
        }

        private async void OpenFileMethod(object sender)
        {
            var path = (string)sender;
            Process.Start(path);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
