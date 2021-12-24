using AppMvcBasica.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new() //esse new quer dizer que podemos dar um new() nessa Entity
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            //Aqui o metodo esta indo ate o banco de dados para a entidade especifica onde a expressao que for passada (predicate), retorna uma lista de forma assincrona
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync(); //oque é o Tracking? toda vez que colocamos algo na memoria, o EF começa a fazer o Tracking ou seja, a rastrear esse objeto, para perceber mudanças de estado, etc... só que se nós fazemos a leitura de objeto sem a intensao de devolve-lo para a base de dados, e sim apenas ler, ele fica no Tracking, entao essa consulta acaba ficando mais pesada, usando mais memoria. Entao o AsNoTracking tira essa funcao.
        }        

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            //DbSet.Remove(await DbSet.FindAsync(id)); //para remover atraves de um id, precisamos ir ate o banco, buscar o objeto, pra depois remover, pq ele nao remove pelo id, ele remove por receber a entidade em si
            DbSet.Remove(new TEntity { Id = id }); //dessa forma nao precisamos ir ate o banco buscar o objeto para remove-lo. como isso é possivel? pq todos estao herdando de TEntity, e o TEntity possui o ID. Entao montamos de forma generica sem precisar fazer a consulta no banco
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync(); //concentramos o saveChanges dentro de um metodo para termos mais reusabilidade de codigo, pois se precisarmos fazer um tratamento de try catch, iremos fazer apenas aqui, e nao em todos os metodos que chamam o saveChanges. Isso é codificacao limpa, pensada em reusabilidade.
        }

        public void Dispose()
        {
            Db?.Dispose(); //a interrogacao serve para: se ele existir faca, senao existir nao faca 
        }
    }
}
