using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class ItemConf {

        public string Name { get; set; }
        public string Item { get; set; }
        public int Price { get; set; }

        public ItemConf() {}
        
        public ItemConf(string Name, string Item, int Price) {
            this.Name = Name;
            this.Item = Item;
            this.Price = Price;
        }
    }
}
