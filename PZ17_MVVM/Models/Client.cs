using System;

namespace PZ17_MVVM.Models;

public class Client
{
    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public DateTime Dob { get; set; }
}
