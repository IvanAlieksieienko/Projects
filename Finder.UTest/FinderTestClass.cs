using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using Finder.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finder.UTest
{
    [TestClass]
    public class FinderTestClass
    {
        [TestMethod]
        public void SearchService_SearchFileByName()
        {
            SearchService service = new SearchService();
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    var path = folder.SelectedPath;
                    var searchResult = service.SearchFileByName(path, @"Sexy-Jitsu.jpg");
                }
            }
        }

        [TestMethod]
        public void SearchService_SearchFileByContent()
        {
            SearchService service = new SearchService();
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    var path = folder.SelectedPath;
                    var searchResult = service.SearchFileByContent(path, @"одна из самых продолжительных эмоций. ");
                }
            }
        }

        [TestMethod]
        public void SearchService_SearchFileByRegEx()
        {
            SearchService service = new SearchService();
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    var path = folder.SelectedPath;
                    var regex = new Regex(@"одна из самых продолжительных эмоций. ");
                    var searchResult = service.SearchFileByRegEx(path, regex);
                }
            }
        }
    }
}
