
using InvoiceProcessor.Api.Services;
using InvoiceProcessor.Application.Common;
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
        private readonly IFileValidator _fileValidator;

        public DocumentsController(
            IMediator mediator,
            IFileValidator fileValidator
            )
        {
            _mediator = mediator;
            _fileValidator = fileValidator;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
        {
            _fileValidator.Validate(file);

            using var stream = file.OpenReadStream();

            var command = new UploadDocumentCommand
            {
                FileStream = stream,
                FileName = file.FileName
            };

            var documentId = await _mediator.Send(command, cancellationToken);
            var correlationId = HttpContext.TraceIdentifier;

            return Ok(ApiResponse<Guid>.Ok(documentId, correlationId));
        }
    }
}
