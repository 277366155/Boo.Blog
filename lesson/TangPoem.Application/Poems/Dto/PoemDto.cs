﻿using Volo.Abp.Application.Dtos;

namespace TangPoem.Application.Poems
{
    public class PoemDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 点评
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 诗人id
        /// </summary>
        public long PoetId { get; set; }
    }
}
