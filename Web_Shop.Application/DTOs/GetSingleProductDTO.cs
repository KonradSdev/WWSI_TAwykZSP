using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Shop.Application.DTOs
{
    public class GetSingleProductDTO
    {
        public ulong IdProduct { get; set; } 

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string Sku { get; set; } = null!;
    }
}
