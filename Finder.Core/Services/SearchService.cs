using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Finder.Core.Interfaces.IServices;
using Finder.Core.Models;
using Microsoft.Office.Interop.Word;

namespace Finder.Core.Services
{
    public class SearchService : ISearchService<TaskModel>
    {

        public SearchService()
        {

        }

        public List<string> StartTask(TaskModel task)
        {
            List<string> result = new List<string>();
            switch(task.SearchMethod)
            {
                case 0: 
                    result = SearchFileByName(task);
                    break;
                case 1:
                    result = SearchFileByContent(task);
                    break;
                case 2:
                    result = SearchFileByRegEx(task);
                    break;
                default: break;
            }
            return result;
        }

        public List<string> SearchFileByName(TaskModel task, string path = null, List<string> findedFilesPaths = null)
        {
            if (findedFilesPaths == null) findedFilesPaths = new List<string>();
            var validFolderPath = (path == null || path == string.Empty) ? Path.GetFullPath(task.BasicPath) : Path.GetFullPath(path);
            var validFileName = Path.GetFileName(task.SearchValue);
            try
            {
                var directoryInfo = new DirectoryInfo(validFolderPath);
                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    var result = true;
                    if (Path.GetFileName(fileInfo.FullName) != validFileName) result = false;
                    var byteSize = (long)(task.SizeValue / (double)Math.Pow(1024, task.UnitOfMeasure));
                    switch (task.SizeCondition)
                    {
                        case 0:
                            if (fileInfo.Length <= byteSize) result = false;
                            break;
                        case 1:
                            if (fileInfo.Length >= byteSize) result = false;
                            break;
                        case 2:
                            if (fileInfo.Length != byteSize) result = false;
                            break;
                        default:
                            break;
                    }
                    switch (task.DateCondition)
                    {
                        case 0:
                            if (fileInfo.CreationTime <= task.DateValue) result = false;
                            break;
                        case 1:
                            if (fileInfo.CreationTime >= task.DateValue) result = false;
                            break;
                        case 2:
                            if (fileInfo.CreationTime != task.DateValue) result = false;
                            break;
                        default:
                            break;
                    }
                    if (result) findedFilesPaths.Add(fileInfo.FullName);
                }
                foreach (var folderInfo in directoryInfo.GetDirectories())
                {
                    SearchFileByName(task, folderInfo.FullName, findedFilesPaths);
                }
            }
            catch(Exception ex) 
            {
                /*
                    Notify user about exceptions
                    In future make some global exceptions handler
                 */
            }
            return findedFilesPaths;
        }

        public List<string> SearchFileByContent(TaskModel task, string path = null, List<string> findedFilesPaths = null)
        {
            if (findedFilesPaths == null) findedFilesPaths = new List<string>();
            var validFolderPath = (path == null || path == string.Empty) ? Path.GetFullPath(task.BasicPath) : Path.GetFullPath(path);
            var fileName = string.Empty;
            var fileinfo = new FileInfo(task.FileMask);
            fileName = Path.GetFileNameWithoutExtension(fileinfo.Name);
            var fileExtension = fileinfo.Extension;
            try
            {
                var directoryInfo = new DirectoryInfo(validFolderPath);
                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    var result = true;
                    var file = fileInfo.FullName;
                    if (!string.IsNullOrEmpty(fileName) && fileName != Path.GetFileNameWithoutExtension(file))
                        continue;
                    if (fileExtension == ".txt")
                    {
                        using (StreamReader streamReader = new StreamReader(file))
                        {
                            var fileContent = streamReader.ReadToEnd();
                            if (!fileContent.Contains(task.SearchValue)) result = false;
                        }
                    }
                    else if (fileExtension == ".doc" || fileExtension == ".docx")
                    {
                        if (!file.Contains("~$"))
                        {
                            Application application = new Application();
                            Document document = application.Documents.Open(file, false, true, false);
                            var fileContent = document.Content.Text;
                            document.Close();
                            application.Quit();
                            if (!fileContent.Contains(task.SearchValue)) result = false;
                        }
                    }
                    var byteSize = (long)(task.SizeValue / (double)Math.Pow(1024, task.UnitOfMeasure));
                    switch (task.SizeCondition)
                    {
                        case 0:
                            if (fileInfo.Length <= byteSize) result = false;
                            break;
                        case 1:
                            if (fileInfo.Length >= byteSize) result = false;
                            break;
                        case 2:
                            if (fileInfo.Length != byteSize) result = false;
                            break;
                        default:
                            break;
                    }
                    switch (task.DateCondition)
                    {
                        case 0:
                            if (fileInfo.CreationTime <= task.DateValue) result = false;
                            break;
                        case 1:
                            if (fileInfo.CreationTime >= task.DateValue) result = false;
                            break;
                        case 2:
                            if (fileInfo.CreationTime != task.DateValue) result = false;
                            break;
                        default:
                            break;
                    }
                    if (result) findedFilesPaths.Add(file);
                }
                foreach (var folderInfo in directoryInfo.GetDirectories())
                {
                    SearchFileByContent(task, folderInfo.FullName, findedFilesPaths);
                }
            }
            catch (Exception ex)
            {
                /*
                    Notify user about exceptions
                    In future make some global exceptions handler
                 */
            }
            return findedFilesPaths;
        }

        public List<string> SearchFileByRegEx(TaskModel task, string path = null, List<string> findedFilesPaths = null)
        {
            if (findedFilesPaths == null) findedFilesPaths = new List<string>();
            var validFolderPath = (path == null || path == string.Empty) ? Path.GetFullPath(task.BasicPath) : Path.GetFullPath(path);
            var regex = new Regex(task.SearchValue);
            var fileName = string.Empty;
            var fileinfo = new FileInfo(task.FileMask);
            fileName = Path.GetFileNameWithoutExtension(fileinfo.Name);
            var fileExtension = fileinfo.Extension;
            try
            {
                var directoryInfo = new DirectoryInfo(validFolderPath);
                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    var result = true;
                    var file = fileInfo.FullName;
                    if (!string.IsNullOrEmpty(fileName) && fileName != Path.GetFileNameWithoutExtension(file))
                        continue;
                    if (fileExtension == ".txt")
                    {
                        using (StreamReader streamReader = new StreamReader(file))
                        {
                            var fileContent = streamReader.ReadToEnd();
                            if (regex.Matches(fileContent).Count <= 0) result = false;
                        }
                    }
                    else if (fileExtension == ".doc" || fileExtension == ".docx")
                    {
                        if (!file.Contains("~$"))
                        {
                            Application application = new Application();
                            Document document = application.Documents.Open(file, false, true, false);
                            var fileContent = document.Content.Text;
                            document.Close();
                            application.Quit();
                            if (regex.Matches(fileContent).Count <= 0) result = false;
                        }
                    }
                    var byteSize = (long)(task.SizeValue / (double)Math.Pow(1024, task.UnitOfMeasure));
                    switch (task.SizeCondition)
                    {
                        case 0:
                            if (fileInfo.Length <= byteSize) result = false;
                            break;
                        case 1:
                            if (fileInfo.Length >= byteSize) result = false;
                            break;
                        case 2:
                            if (fileInfo.Length != byteSize) result = false;
                            break;
                        default:
                            break;
                    }
                    switch (task.DateCondition)
                    {
                        case 0:
                            if (fileInfo.CreationTime <= task.DateValue) result = false;
                            break;
                        case 1:
                            if (fileInfo.CreationTime >= task.DateValue) result = false;
                            break;
                        case 2:
                            if (fileInfo.CreationTime != task.DateValue) result = false;
                            break;
                        default:
                            break;
                    }
                    if (result) findedFilesPaths.Add(file);
                }
                foreach (var folderInfo in directoryInfo.GetDirectories())
                {
                    SearchFileByRegEx(task, folderInfo.FullName, findedFilesPaths);
                }
            }
            catch (Exception ex)
            {
                /*
                    Notify user about exceptions
                    In future make some global exceptions handler
                 */
            }
            return findedFilesPaths;
        }
    }
}
