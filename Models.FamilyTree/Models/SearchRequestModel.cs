using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class SearchRequestModel
    {
        public string Query { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string FilterField { get; set; }
        public string FilterValue { get; set; }
    }
}
