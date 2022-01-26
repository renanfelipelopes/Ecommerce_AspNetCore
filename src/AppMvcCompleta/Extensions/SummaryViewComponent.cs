using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Extensions
{
    public class SummaryViewComponent : ViewComponent // Irá herdar de ViewComponent. Assim eu posso chamar o método InvokeAsync que sempre retorna uma View
    {
        private readonly INotificador _notificador;

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        // Nesse método InvokeAsync, eu vou pegar as notificações e transformar isso numa lista. Para isso eu injetei a dependência do INotificador
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Aqui eu preciso receber uma Task FromResult pq como esse método ObterNotificacoes não é assincrono, e eu estou dentro de contexto assincrono aqui, eu vou recebe-lo dessa forma para possibilitar a compatibilidade
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());
           
            // Aqui eu faço um foreach de notificacoes colocando cada uma das AddModelError na ModelState como se fosse um erro de model, ou seja, ele vai tratar como formulário, como se fosse um erro de preenchimento de campo, só que sem um campo específico. 
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));

            // Essa view vai substituir o item que mostra as notificações 
            return View();
        }
    }
}