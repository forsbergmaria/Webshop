using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class TransactionWithSizes : ItemTransaction
	{
		public int SizeId { get; set; }
		[ForeignKey(nameof(SizeId))] 
		public virtual Size Sizes { get; set; }
	}
}
