namespace MedicalSystemWebAPI.Dtos
{
    public class FileResponseDto
    {
        public string ObjectId { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }

    }
}
