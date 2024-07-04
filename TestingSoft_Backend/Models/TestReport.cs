using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class TestReport
{
    public int TestReportId { get; set; }

    public int? TestSuiteId { get; set; }

    public int? TestCaseId { get; set; }

    public string? TestResult { get; set; }

    public string? FilePathVideo { get; set; }

    public string? FilePathJson { get; set; }

    public string? FilePathC { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual TestCase? TestCase { get; set; }
}
