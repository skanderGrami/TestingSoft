using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class TypeValue
{
    public int Id { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<Scenario> Scenarios { get; set; } = new List<Scenario>();
}
