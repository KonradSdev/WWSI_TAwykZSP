using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Mappings.PropertiesMappings
{
    public class SieveConfigurationForProduct : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Product>(p => p.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Product>(p => p.Description)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Product>(p => p.Price)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Product>(p => p.Sku)
                 .CanSort()
                 .CanFilter();
        }
    }
}
