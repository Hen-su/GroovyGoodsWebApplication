using System;
using System.Collections.Generic;

namespace GroovyGoodsWebApplication.Models;

public partial class SupplierProduct
{
    public int Spid { get; set; }
    
    public int Sid { get; set; }

    public int Pid { get; set; }

    public decimal Cost { get; set; }

    public virtual Product PidNavigation { get; set; } = null!;

    public virtual Supplier SidNavigation { get; set; } = null!;

}

