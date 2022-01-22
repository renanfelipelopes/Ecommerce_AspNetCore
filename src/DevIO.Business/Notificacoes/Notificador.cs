using System.Collections.Generic;
using System.Linq;
using DevIO.Business.Intefaces;

namespace DevIO.Business.Notificacoes
{
    public class Notificador : INotificador
    {
        // Essa lista vai ser uma lista global que vai durar todo o request, então como igual uma pilha,
        // eu vou colocando notificações aqui dentro durante todo o processo de validação
        // A noticacao vai ser lançada e manipulada pelo Handle
        // Essa propriedade não pode ser readonly senão eu não vou poder adicionar as notificações 
        private List<Notificacao> _notificacoes;

        // Uma vez que eu der um new no Notificador(), eu vou criar essa lista de notificações e pronto
        // já posso começar a trabalhar com essa classe
        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        // O Handle vai pegar a notificação e adicionar ela na lista
        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        // Aqui só vamos retornar essa lista
        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        // Aqui só vamos dizer se tem ou não. Se eu tiver uma notificação significa existe um problema já notificado, 
        // Então é o momento de tomar uma atitude
        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
