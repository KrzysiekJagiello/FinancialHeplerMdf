using System;
using System.Collections.Generic;

namespace financialHelper1._0.DB;

public partial class Balance
{
    public int Id { get; set; }

    public string AccountName { get; set; } = null!;

    public double AccountBalance { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
}
