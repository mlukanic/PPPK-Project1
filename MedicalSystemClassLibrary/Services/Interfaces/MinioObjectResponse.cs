namespace MedicalSystemClassLibrary.Services.Interfaces
{
    public class MinioObjectResponse
    {
        public string ContentType { get; set; }
        public Stream Data { get; set; }
    }
}