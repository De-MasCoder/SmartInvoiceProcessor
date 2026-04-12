namespace InvoiceProcessor.Api.Services
{
    public class FileValidator : IFileValidator
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        private static readonly string[] AllowedContentTypes =
        {
            "application/pdf",
            "image/png",
            "image/jpeg"
        };

        private static readonly string[] AllowedExtensions =
        {
            ".pdf",
            ".png",
            ".jpg",
            ".jpeg"
        };

        public void Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File size exceeds 5MB limit");

            if (!AllowedContentTypes.Contains(file.ContentType))
                throw new ArgumentException("Invalid file type");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file extension");
        }
    }
}
