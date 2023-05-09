﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class Size
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SizeId { get; set; }
        [Required(ErrorMessage = "Vänligen tilldela storleken ett namn")]
        [Display(Name = "Storleksnamn")]
        public string Name { get; set; }
    }
}
