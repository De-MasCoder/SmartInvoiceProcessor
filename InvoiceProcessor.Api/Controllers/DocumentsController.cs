
using InvoiceProcessor.Application.Documents.Commands.UploadDocument;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessor.Api.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            using var stream = file.OpenReadStream();

            var command = new UploadDocumentCommand
            {
                FileStream = stream,
                FileName = file.FileName
            };

            var documentId = await _mediator.Send(command);

            return Ok(new { DocumentId = documentId });
        }
    }
}
