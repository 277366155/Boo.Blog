﻿
using Volo.Abp.Application.Dtos;
namespace TangPoem.Application.Poems
{
    public class CategoryPoemDto : EntityDto<long>
    {
        public long CategoryId { get; set; }
        public long PoemId { get; set; }
    }
}
