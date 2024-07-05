using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IBlobService
    {
        public Task<List<string>> GetAllBobs(string containerName);
        public Task<bool> UploadBlob(string name, IFormFile file, string conatinerName);
    }
}
