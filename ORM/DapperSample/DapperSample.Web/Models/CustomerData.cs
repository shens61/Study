using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperSample.Web.Models
{
    [Display(Name = "CSV Data")]
    public class CustomerData
    {
        public string Text { get; set; }
    }
}
