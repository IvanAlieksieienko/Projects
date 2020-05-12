using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Finder.Core.Interfaces.IServices
{
    public interface ISearchService<T> where T : class
    {
        List<string> StartTask(T task);
        List<string> SearchFileByName(T task, string path = null, List<string> findedFilesPaths = null);
        List<string> SearchFileByContent(T task, string path = null, List<string> findedFilesPaths = null);
        List<string> SearchFileByRegEx(T task, string path = null, List<string> findedFilesPaths = null);
    }
}
