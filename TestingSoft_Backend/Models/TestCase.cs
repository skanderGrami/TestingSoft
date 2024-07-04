using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class TestCase
{
    public int TestCaseId { get; set; }

    public string? TestCaseName { get; set; }

    public string? TestCaseDescription { get; set; }

    public DateTime? TestCaseCreatedDate { get; set; }

    public DateTime? TestCaseUpdatedDate { get; set; }

    public string? Navigator { get; set; }

    public string? Url { get; set; }

    public int? UserId { get; set; }

    public int? TestSuiteId { get; set; }

    public byte? VersionTest { get; set; }

    public string? JsonContent { get; set; }

    public string? CsharpContent { get; set; }

    public virtual ICollection<Build> Builds { get; set; } = new List<Build>();

    public virtual ICollection<Scenario> Scenarios { get; set; } = new List<Scenario>();

    public virtual ICollection<TestReport> TestReports { get; set; } = new List<TestReport>();

    public virtual TestSuite? TestSuite { get; set; }

    public virtual User? User { get; set; }
}
