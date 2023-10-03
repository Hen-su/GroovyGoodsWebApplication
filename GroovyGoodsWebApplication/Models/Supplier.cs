using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroovyGoodsWebApplication.Models
{
    public partial class Supplier
    {
        public int Sid { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [StringLength(31)]
        [RegularExpression(@"^.{0,30}$", ErrorMessage = "Input should not exceed 30 characters.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Contact name is required.")]
        [StringLength(31)]
        [RegularExpression(@"^.{0,30}$", ErrorMessage = "Input should not exceed 30 characters.")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(51)]
        [RegularExpression(@"^(?=.{1,50}$)([A-Za-z0-9+_.-]+@(.+))$", ErrorMessage = "Please enter a valid email (up to 50 characters).")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(16)]
        [RegularExpression(@"^.{0,15}$", ErrorMessage = "Input should not exceed 15 characters.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(51)]
        [RegularExpression(@"^.{0,50}$", ErrorMessage = "Input should not exceed 50 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(21)]
        [RegularExpression(@"^.{0,20}$", ErrorMessage = "Input should not exceed 20 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postcode is required.")]
        [Range(0, 99999, ErrorMessage = "Postcode must be between 0 and 99999.")]
        public int Postcode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(21)]
        [RegularExpression(@"^.{0,20}$", ErrorMessage = "Input should not exceed 20 characters.")]
        public string Country { get; set; }

        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    }
}