using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace GroovyGoodsWebApplication.Models;

public partial class Product
{
    public int Pid { get; set; }
    [Required]
    [StringLength (50)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength (255)]
    public string? Description { get; set; }
    [Required]
    [RegularExpression(@"^\d{0,6}[.]?\d{1,2}$", ErrorMessage = "Price is invalid")]
    [DisplayName("List Price ($)")]
    [StringLength(9)]
    public decimal ListPrice { get; set; }
    [Required]
    [RegularExpression(@"/^\d+$/", ErrorMessage = "Value must be positive")]
    public int Stock { get; set; }

    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}
