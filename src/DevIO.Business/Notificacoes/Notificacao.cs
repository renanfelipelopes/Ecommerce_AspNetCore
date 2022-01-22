using System;
using System.Text;

namespace DevIO.Business.Notificacoes
{
    public class Notificacao
    {
        // O construtor vai receber a mensagem em si, o que quer dizer que eu sou obrigado a passar a mensagem via construtor
        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;
        }

        // Já eu só passo a mensagem via construtor, eu posso apenas passar uma entidade readonly sem o set, uma vez que eu
        // já criei a instancia eu não preciso mais mexer nela
        public string Mensagem { get; set; }
    }
}
