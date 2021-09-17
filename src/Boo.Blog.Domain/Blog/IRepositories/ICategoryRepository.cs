using Volo.Abp.Domain.Repositories;

namespace Boo.Blog.Domain.Blog.IRepositories
{
    public interface ICategoryRepository:IRepository<Category,long>
    {
    }
}
