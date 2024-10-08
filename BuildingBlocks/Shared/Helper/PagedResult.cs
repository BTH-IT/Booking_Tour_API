using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helper
{
	public class PagedResult<T>
	{
		public List<T> Items { get; set; }
		public int TotalItems { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

		public PagedResult(List<T> items, int totalItems, int pageNumber, int pageSize)
		{
			Items = items;
			TotalItems = totalItems;
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
	}

}
