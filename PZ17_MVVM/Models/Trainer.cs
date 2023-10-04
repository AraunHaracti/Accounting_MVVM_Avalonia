using System.Collections.Generic;

namespace PZ17_MVVM.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Accounting> Accountings { get; set; } = new List<Accounting>();
}
