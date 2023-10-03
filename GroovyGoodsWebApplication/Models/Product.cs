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
    [StringLength(31)]
    [RegularExpression(@"^.{0,30}$", ErrorMessage = "Input should not exceed 30 characters.")]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength (256)]
    [RegularExpression(@"^.{0,255}$", ErrorMessage = "Input should not exceed 255 characters.")]
    public string? Description { get; set; }

    [Required]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be valid")]
    [DisplayName("List Price ($)")]
    public decimal ListPrice { get; set; }


    [Required]
    [RegularExpression(@"^\d+$", ErrorMessage = "Value must be valid")]
    public int Stock { get; set; }

    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}
