using System;
using System.Collections.Generic;

namespace MinimalApi.Models;

public partial class TblPerson
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public DateTime? Birth { get; set; }
}
