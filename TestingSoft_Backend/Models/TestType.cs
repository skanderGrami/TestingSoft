using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class TestType
{
    public int TestId { get; set; }

    public string? TestName { get; set; }

    public string? TestDescription { get; set; }

    public virtual ICollection<TestSuite> TestSuites { get; set; } = new List<TestSuite>();
}
