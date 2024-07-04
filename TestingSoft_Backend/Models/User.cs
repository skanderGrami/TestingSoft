using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();

    public virtual ICollection<TestSuite> TestSuites { get; set; } = new List<TestSuite>();
}
