using System;
using System.Collections.Generic;

namespace financialHelper1._0.DB;

public partial class Expense
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Category { get; set; } = null!;

    public double Amount { get; set; }

    public int AccountId { get; set; }

    public double HowMuchReturn { get; set; }

    public bool IsSettled { get; set; }

    public string? Description { get; set; }

    public virtual Balance Account { get; set; } = null!;
}
