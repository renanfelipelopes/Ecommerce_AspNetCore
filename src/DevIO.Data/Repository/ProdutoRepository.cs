using AppMvcBasica.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(MeuDbContext context) : base(context) { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await Db.Produtos.AsNoTracking()
                .Include(f => f.Fornecedor).FirstOrDefaultAsync(p => p.Id == id); //nessa query fazemos um innerjoin entre produto e fornecedor
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await Db.Produtos.AsNoTracking()
                .OrderBy(p => p.Nome).ToListAsync(); //obter todos os produtos com os dados dos fornecedores, retornando uma lista organizada por nome
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId); //o metodo Buscar está na classe mae Repository, e o parametro dela (predicate) permite que facamos essa query de buscar dados personalizada 
        }
    }
}
