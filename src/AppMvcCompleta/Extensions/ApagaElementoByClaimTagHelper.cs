using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace DevIO.App.Extensions
{
    // Aqui vamos ter que apontar para o HtmlTargetElement. O primeiro parametro poderia ser qualquer um elemento do HTML (div, p, hfef, etc..)
    // No nosso caso, ele está atendendo para todo mundo, por isso usamos o *. E quais são os atributos dele? Os mesmos que nós já colocamos no arquivo Index

    [HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
    public class ApagaElementoTagHelper : TagHelper // Primeira coisa, vamos herdar de TagHelper porque ai podemos depois incluir as funcionalidades
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        // Vamos precisar criar dois tipos de propriedades, pq elas serão analisadas. São elas a IdentityClaimName e IdentityClaimValue abaixo
        // Vamos deixar o nome delas assim para ficar claro que estão fazendo tratamento através do Identity 
        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        // E tudo acontece dentro do Process, onde eu vou processar essa tagHelper  
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Duas que podemos fazer são validar aqui se o contexto e o output são nulos e retornam uma exception
            // O contexto é o que estamos recebendo naquela TagHelper, o contexto da view por exemplo, e tudo mais.
            // O output é a saida, é o que essa taghelper vai produzir de conteudo
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (temAcesso) return;

            output.SuppressOutput();
        }
    }
}