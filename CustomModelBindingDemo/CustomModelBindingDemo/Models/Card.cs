using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomModelBindingDemo.Models
{
    public class Card
    {
        public string CardNo { get; set; }
        public string Code{ get; set; }
        public DateTime? Expiry { get; set; }
    }
}