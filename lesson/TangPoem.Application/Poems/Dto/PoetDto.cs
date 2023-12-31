﻿using Volo.Abp.Application.Dtos;

namespace TangPoem.Application.Poems
{
    public class PoetDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
