namespace TechNest.API.Model
{
    public class FileUploadSettings
    {
        public int MaxFileUploadLimit { get; set; }
        public long MaxFileSize { get; set; } // in bytes
        public string[] AllowedFileTypes { get; set; }
    }
}
