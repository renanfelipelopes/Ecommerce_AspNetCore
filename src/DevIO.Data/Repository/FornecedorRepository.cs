using AppMvcBasica.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(MeuDbContext context) : base(context) { }

        public Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
