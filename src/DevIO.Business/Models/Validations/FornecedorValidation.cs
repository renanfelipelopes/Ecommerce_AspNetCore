using AppMvcBasica.Models;
using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    class FornecedorValidation : AbstractValidator<Fornecedor>
    {
        public FornecedorValidation()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("o campo {PropertyName} precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, action: () =>
            {
                //RuleFor(expression: f => f.Documento.Length)
            });

            When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, action: () => { });
        }
    }
}
