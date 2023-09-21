using System;
using System.Collections.Generic;

namespace GroovyGoodsWebApplication.Models;

public partial class Product
{
    public int Pid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal ListPrice { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}
