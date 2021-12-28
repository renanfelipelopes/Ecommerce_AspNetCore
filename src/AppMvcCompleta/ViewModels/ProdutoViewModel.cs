using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.ViewModels
{
    public class ProdutoViewModel
    {
        //Para controle e entendimento da View, preciso dizer que esse é um componente chave com a annotation [Key], para ele saber que o campo Id sempre vai ser uma chave e nunca um campo
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        //para trabalharmos com upload de arquivo, a entidade nao pode ser do tipo string, ele precisa trabalhar de outra forma, mas nao podemos perder a entidade Imagem pq ele vai continuar mapeando o banco de dados, entao vamos duplicar a entidade imagem com outro nome e do tipo IFormFile, e nesse tipo ele possui propriedades como nome do arquivo, extensao do arquivo, tamanho etc...
        //public IFormFile ImagemUpload { get; set; }
        public string Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        //O campo DataCadastro não queremos que ele seja criado como campo, entao usamos o recurso ScaffoldColumn(false), na hora de fazer o Scaffold nao considere esta coluna
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
    }
}
