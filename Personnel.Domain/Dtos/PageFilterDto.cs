using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos
{
    public class PageFilterDto
    {
        public int Page { get; set; }

        public int PageSize
        {
            get => Length;
            set => Length = value;
        }

        public int Take { get; set; }
        public int Skip { get; set; }

        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public int PageIndex
        {
            get
            {
                return Start > 0 ? Start / Length + 1 : Page;
            }
        }

        public PageFilterDto()
        {
            this.Page = Draw > 0 ? Draw : 1;
            Sort = new List<DataSortDto>();
        }

        public IList<DataSortDto> Sort { get; set; }
    }

    public class DataSortDto
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
}
