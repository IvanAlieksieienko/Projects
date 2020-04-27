using SERP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Core.IServices
{
    public interface ISerpService
    {
        Task<int> GetPosition(TaskModel model);
        Task<List<LocationModel>> GetLocations(string searchEngine);
    }
}
