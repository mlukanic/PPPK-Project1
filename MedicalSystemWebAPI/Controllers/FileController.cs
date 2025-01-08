using MedicalSystemClassLibrary.Services.Interfaces;
using MedicalSystemWebAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MedicalSystemMvc.Controllers
{
    [ApiController]
    [Route("/api/files")]
    public class FileController : ControllerBase
    {
        private readonly IMinioService _minioService;

        public FileController(IMinioService minioService)
        {
            _minioService = minioService;
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // procitati file stream

            var fileStream = file.OpenReadStream();
            var fileName = file.FileName;
            var fileContentType = file.ContentType;
            var fileSize = file.Length;

            // koristeci minio, uploadati file stream u bucket

            var objectId = await _minioService.PutObject(fileStream, fileName, fileContentType, fileSize);

            // here, data could be saved into relational database

            return Created( // 201 Created (returns object dto and URI to newly created resource)
                "$http://localhost:5100/api/files/{objectId}",
                new FileResponseDto()
                {
                    ObjectId = objectId,
                    ContentType = fileContentType,
                    Path = $"http://localhost:5100/api/files/{objectId}",
                    Size = fileSize,
                    Name = fileName
                });

        }


        [HttpGet("{objectId}")]
        public async Task<IActionResult> GetFile(string objectId)
        {

            var response = await _minioService.GetObject(objectId);

            return File(response.Data, response.ContentType);
        }


    }


}
