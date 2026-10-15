using FluentValidation;
using WebGenQRCode.Models.QrCode;

namespace WebGenQRCode.Validators.QrCode;

public class CreateQrCodeRequestValidator : AbstractValidator<CreateQrCodeRequest>
{
    public CreateQrCodeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва QR-коду обов'язкова")
            .MaximumLength(100).WithMessage("Назва QR-коду занадто довга");

        RuleFor(x => x.TargetUrl)
            .NotEmpty().WithMessage("Посилання обов'язкове")
            .MaximumLength(2048).WithMessage("Посилання занадто довге")
            .Must(BeAValidHttpUrl).WithMessage("Некоректний формат посилання (має починатися з http:// або https://)");
    }

    private static bool BeAValidHttpUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
