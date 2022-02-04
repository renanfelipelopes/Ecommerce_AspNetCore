using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
    public class ApagaElementoTagHelper : TagHelper 
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (temAcesso) return;

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("*", Attributes = "supress-by-action")]
    public class ApagaElementoByActionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("supress-by-action")]
        public string ActionName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            // Aqui no processamento ele vai fazer _contextAccessor.HttpContext.GetRouteData(). O GetRouteData vai pegar uma coleção de dados da rota através da chave "action", ou seja, eu quero saber o nome da "action".
            // Então é assim que pegamos o nome de uma action dentro de um request
            var action = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            // Se a ActionName possui (contém) o valor da action da qual vc está chamando, ok, mata a operação aqui, caso não, faço o SuppressOutput, ou seja, ele não vai gerar o elemento
            if (ActionName.Contains(action)) return;

            output.SuppressOutput();
        }
    }
}