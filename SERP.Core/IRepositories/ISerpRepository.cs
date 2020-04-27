using SERP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Core.IRepositories
{
    public interface ISerpRepository
    {
        Task Add(TaskModel task);
        Task Update(TaskModel task);
    }
}
