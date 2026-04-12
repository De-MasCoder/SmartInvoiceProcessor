namespace InvoiceProcessor.Api.Services
{
    public interface IFileValidator
    {
        void Validate(IFormFile file);
    }
}
