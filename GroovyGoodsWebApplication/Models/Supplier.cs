using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroovyGoodsWebApplication.Models;

public partial class Supplier
{
    public int Sid { get; set; }

    [Required]
    [StringLength(50)]
    public string Company { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string ContactName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[A-Za-z0-9+_.-]+@(.+)$", ErrorMessage = "Please enter a valid email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public int Postcode { get; set; }

    [Required]
    [StringLength(50)]
    public string Country { get; set; } = null!;

    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}
