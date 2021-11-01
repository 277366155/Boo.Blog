using Boo.Blog.ToolKits.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangPoem.Application.Poems;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;


namespace ConsolePro
{
    public class Service : ApplicationService
    {
        //IRepository<Poet> _poetRepository;
        //IUnitOfWorkManager _uowManager;
        //public Service(IRepository<Poet> poetRepository, IUnitOfWorkManager uowManager)
        //{
        //    _poetRepository = poetRepository;
        //    _uowManager = uowManager;
        //}

        IPoemApplicationService _poemService;
        public Service(IPoemApplicationService poemService)
        {
            _poemService = poemService;
        }

        public void HelloWorld()
        {
            Console.WriteLine("Hello world.");
        }

        public IEnumerable<PoetDto> PagedData()
        {
            var data = _poemService.GetPagedPoets(new PagedResultRequestDto
            {
                 SkipCount=0,
                  MaxResultCount=10
            });
            return data.Items;
        }

        public void GetPoemOfCategory(long id)
        {
            var data =_poemService.GetPoemOfCategory(id);
            foreach (var d in data)
            {
                Console.WriteLine(d.ToJson());
            }
        }
        //public void GetPoetList()
        //{
        //    using var uow = _uowManager.Begin(new AbpUnitOfWorkOptions());

        //    var poets = _poetRepository.AsQueryable().ToList();
        //    Console.WriteLine(poets.ToJson());

        //}
        //public Poet GetOne()
        //{
        //    using var uow = _uowManager.Begin(new AbpUnitOfWorkOptions());
        //    var data = _poetRepository.Include(a => a.Poems).FirstOrDefault();
        //    return data;
        //}
        //public async Task<Poet> AddNewDataAsync()
        //{
        //    var newData = new Poet { Name = "托尔斯泰", Description = Guid.NewGuid().ToString("N") };
        //    var data = await _poetRepository.InsertAsync(newData);
        //    return data;
        //}
    }
}
