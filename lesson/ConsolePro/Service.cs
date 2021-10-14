using System;
using System.Linq;
using System.Threading.Tasks;
using TangPoem.Core.Poems;
using TangPoem.EF.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ConsolePro
{
    public class Service : ApplicationService
    {
        IPoemRepository _poetRepository;
        public Service(IPoemRepository poetRepository)
        {
            _poetRepository = poetRepository;
        }

        public void HelloWorld()
        {
            Console.WriteLine("Hello world.");
        }

        public void GetFirstPoetName()
        {
            //Console.WriteLine(_poetRepository.FirstOrDefault()?.Name);
            var newData = new Poet { Name = "托尔斯泰", Description = Guid.NewGuid().ToString("N") };
            var result = _poetRepository.Insert(newData);
            Console.WriteLine($"\r\n{result.Id}\r\n");
        }

        public async Task<Poet> AddNewDataAsync()
        {
            var newData = new Poet { Name = "托尔斯泰", Description = Guid.NewGuid().ToString("N") };
            var data = await _poetRepository.InsertAsync(newData);
            return data;
        }
    }
}
