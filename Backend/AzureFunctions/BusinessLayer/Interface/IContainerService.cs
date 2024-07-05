using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IContainerService
    {
        public Task<Dictionary<string, List<string>>> GetAllContainersAndBlobs();
        public Task<List<string>> GetAllContainers();

    }
}
