using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroovyGoodsWebApplication.Models
{
    public partial class Supplier
    {
        public int Sid { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(31)]
        [RegularExpression(@"^.{0,30}$", ErrorMessage = "Input should not exceed 30 characters.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(31)]
        [RegularExpression(@"^.{0,30}$", ErrorMessage = "Input should not exceed 30 characters.")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(51)]
        [RegularExpression(@"^(?=.{1,50}$)([A-Za-z0-9+_.-]+@(.+))$", ErrorMessage = "Please enter a valid email: [+, -, _, ., letter or digits]@valid domain email address (up to 50 characters).")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(17)]
        [RegularExpression(@"^[\d+()-]{1,16}$", ErrorMessage = "Please enter a valid phone number up to 16 characters (+, -, () symbols only).")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(51)]
        [RegularExpression(@"^.{0,50}$", ErrorMessage = "Input should not exceed 50 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(21)]
        [RegularExpression(@"^.{0,20}$", ErrorMessage = "Input should not exceed 20 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Range(0, 99999, ErrorMessage = "Postcode must be between 0 and 99999.")]
        public int Postcode { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(21)]
        [RegularExpression(@"^.{0,20}$", ErrorMessage = "Input should not exceed 20 characters.")]
        public string Country { get; set; }

        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    }
}