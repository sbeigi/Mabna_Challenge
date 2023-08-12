using System;
using System.Collections.Generic;

namespace FastMockDataGenerator.Database;

public partial class Instrument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
