using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class Build
{
    public int BuildId { get; set; }

    public string? BuildNum { get; set; }

    public string? BuildStatus { get; set; }

    public DateTime? BuildUrl { get; set; }

    public int? TestCaseId { get; set; }

    public virtual TestCase? TestCase { get; set; }
}
