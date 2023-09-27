using System;
using System.Collections.Generic;

namespace PZ17_MVVM.Models;

public partial class Accounting
{
    public int AccountingId { get; set; }

    public int TrainerId { get; set; }
    public string TrainerName { get; set; }

    public int ClientId { get; set; }
    public string ClientName { get; set; }

    public DateOnly StartDate { get; set; }

    public int CountClasses { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Trainer Trainer { get; set; } = null!;
}
