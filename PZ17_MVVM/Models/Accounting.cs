using System;

namespace PZ17_MVVM.Models;

public class Accounting
{
    public int AccountingId { get; set; }

    public int TrainerId { get; set; }

    public int ClientId { get; set; }

    public DateTime StartDate { get; set; }

    public int CountClasses { get; set; }
}
