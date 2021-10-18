using System.Collections.Generic;
using System.Threading.Tasks;
using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories;

namespace TangPoem.Core.IRepositories
{
    public interface IPoemRepository:IRepository<Poem,long>
    {
        Task<(List<Poem>,int)> GetPagedPoemsAsync(int maxResult,int skip,string author,string keyword,string[] categories);
    }
}
