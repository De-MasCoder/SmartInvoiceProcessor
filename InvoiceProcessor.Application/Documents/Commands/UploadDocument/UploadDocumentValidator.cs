using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Documents.Commands.UploadDocument
{
    public class UploadDocumentValidator : AbstractValidator<UploadDocumentCommand>
    {
        public UploadDocumentValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.FileStream)
                .NotNull();
        }
    }
}
