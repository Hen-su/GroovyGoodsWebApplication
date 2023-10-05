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


    [Required(ErrorMessage = "This field is required.")]
    [StringLength(31)]
    [RegularExpression(@"^.{0,30}$", ErrorMessage = "Input should not exceed 30 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [StringLength (256)]
    [RegularExpression(@"^.{0,255}$", ErrorMessage = "Input should not exceed 255 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Range(0.01, 1000000, ErrorMessage = "List Price must be between 0.01 and 1000000.")]
    [DisplayName("List Price ($)")]
    public decimal ListPrice { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Range(0, 99999, ErrorMessage = "Stock must be between 0 and 99999.")]
    public int Stock { get; set; }

    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}
