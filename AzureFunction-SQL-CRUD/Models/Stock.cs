using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunction_SQL_CRUD.Models
{
	public class Stock
	{
        public int StockId { get; set; }
        public string StockName { get; set; }
		public decimal Price { get; set; }
	}
}
