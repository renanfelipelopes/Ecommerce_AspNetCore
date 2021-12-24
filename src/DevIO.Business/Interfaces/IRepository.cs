using AppMvcBasica.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        //descricao do metodo: existe um metodo Adicionar onde qualquer entidade que for recebida aqui desde que seja filha de entity sera aceita. Isso vale para os outros metodos
        //quando metodo só possui a task, ele é do tipo void
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(Guid Id); //obterPorId recebe um id guid, que vai retornar uma Task do tipo TEntity.
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        //o metodo buscar onde passamos uma expressao lambda que vai trabalhar com uma function que vai comparar a nossa entidade (TEntity) com alguma que retorne boolean, que chamamos de predicado. Ou seja, estou possibilitando que seja passada uma expressao lambda dentro do metodo buscar qualquer entidade por qualquer parametro
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges(); //o int é o numero de linhas afetadas 
    }
}
