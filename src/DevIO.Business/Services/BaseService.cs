using DevIO.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
    }

    public class FornecedorService : BaseService, IFornecedorService
    {
    }

    public class ProdutoService : BaseService, IProdutoService
    {
    }
}
