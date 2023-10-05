using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GroovyGoodsWebApplication.Models;

public partial class SupplierProduct
{
    public int Spid { get; set; }
    
    public int Sid { get; set; }

    public int Pid { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Range(0.01, 1000000, ErrorMessage = "Cost must be between 0.01 and 1000000.")]
    [DisplayName("Cost ($)")]
    public decimal Cost { get; set; }

    public virtual Product PidNavigation { get; set; } = null!;

    public virtual Supplier SidNavigation { get; set; } = null!;

}

