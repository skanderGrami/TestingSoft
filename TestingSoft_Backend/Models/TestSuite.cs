using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class TestSuite
{
    public int TestSuiteId { get; set; }

    public string? TestSuiteName { get; set; }

    public string? TestSuiteDescription { get; set; }

    public string? TestSuiteCreator { get; set; }

    public DateTime? TestSuiteCreatedDate { get; set; }

    public DateTime? TestSuiteUpdatedDate { get; set; }

    public int? UserId { get; set; }

    public int? TestId { get; set; }

    public virtual TestType? Test { get; set; }

    public virtual ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();

    public virtual User? User { get; set; }
}
