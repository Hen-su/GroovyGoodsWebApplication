using System;
using System.Collections.Generic;

namespace GroovyGoodsWebApplication.Models;

public partial class Supplier
{
    public int Sid { get; set; }

    public string Company { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public int Postcode { get; set; }

    public string Country { get; set; } = null!;

    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}
