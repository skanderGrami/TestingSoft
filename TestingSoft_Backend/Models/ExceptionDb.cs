using System;
using System.Collections.Generic;

namespace TestingSoft_Backend.Models;

public partial class ExceptionDb
{
    public int Id { get; set; }

    public string? Error { get; set; }

    public string? Repository { get; set; }

    public string? Fonction { get; set; }

    public DateTime? CreateDate { get; set; }
}
