using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class Scenario
{
    public int ScenarioId { get; set; }

    public string? Commande { get; set; }

    public string? Value { get; set; }

    public string? Path { get; set; }

    public int? TestCaseId { get; set; }

    public string? TagValue { get; set; }

    public int? IdTypeValue { get; set; }

    public string? Url { get; set; }

    public virtual TypeValue? IdTypeValueNavigation { get; set; }

    public virtual TestCase? TestCase { get; set; }
}
