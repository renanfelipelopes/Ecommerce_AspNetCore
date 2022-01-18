using AppMvcBasica.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        //eu preciso colocar algum metodo aqui? nao, por que todos os metodos que eu defini já estao em Fornecedor, entao essa interface só extende especializando para a classe fornecedor
        //porem, algumas coisas serao necessarias
        Task<Fornecedor> ObterFornecedorEndereco(Guid id); //retornar um fornecedor, e tambem no msm metodo, obter o endereco junto
        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id); //sera retornado tanto fornecedor, quanto a lista de produtos e o endereco do fornecedor
    }
}
