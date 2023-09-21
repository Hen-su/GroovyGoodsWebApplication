using System;
using System.Collections.Generic;

namespace GroovyGoodsWebApplication.Models;

public partial class Administrator
{
    public int Aid { get; set; }

    public string Username { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Hash { get; set; } = null!;
}
