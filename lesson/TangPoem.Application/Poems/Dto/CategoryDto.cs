﻿using Volo.Abp.Application.Dtos;

namespace TangPoem.Application.Poems
{
    public class CategoryDto : EntityDto<long>
    {
        public string Name { get; set; } 
    }
}
