using System;
using System.Collections.Generic;
using System.Text;

namespace TangPoem.Application.Poems.Dto
{
   public class SearchPoemDto
    {
        public int MaxResultCount { get; set; }
        public int Skip { get; set;}
        public string Author { get; set; }
        public string Keyword { get; set; }
        public string[] Categories { get; set;}

    }
}
