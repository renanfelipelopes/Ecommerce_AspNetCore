using AppMvcBasica.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    interface IProdutoService
    {
        Task Adicionar(Produto produto);
        
        Task Atualizar(Produto produto);
        
        Task Remover(Guid id);
    }
}
