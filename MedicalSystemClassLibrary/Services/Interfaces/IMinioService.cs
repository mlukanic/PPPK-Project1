using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystemClassLibrary.Services.Interfaces
{
    public interface IMinioService
    {
        public Task<string> PutObject(Stream data, string fileName, string contentType, long size);
        public Task<MinioObjectResponse> GetObject(string objectId);
        Task DeleteObject(string objectId);
    }

}
