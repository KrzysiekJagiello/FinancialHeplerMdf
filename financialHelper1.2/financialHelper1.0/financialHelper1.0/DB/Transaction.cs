using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialHelper1._0.DB
{
    public partial class Transaction
    {
        public DateOnly Date { get; set; }

        public string Category { get; set; } = null!;

        public double Amount { get; set; }

        public int AccountId { get; set; }

        public string? Description { get; set; }

        public int TransactionSource { get; set; } // 1 -> income 2-> expense

    }
}
