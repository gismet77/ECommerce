using ECommerce.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Mehsulun adi bos ola bilmez.")
                .MaximumLength(150)
                .MinimumLength(5)
                .WithMessage("Mehsulun adi 5-150 simvol arasi olmalidir");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("Stock melumatlarini doldurun")
                .Must(s => s >= 0)
                .WithMessage("Stock melumatlari menfi eded ola bilmez!");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                .WithMessage("Qiymet melumatlarini doldurun")
                .Must(s => s >= 0)
                .WithMessage("Qiymet melumatlari menfi eded ola bilmez!");
        }
    }
}
