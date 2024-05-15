using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials{
    public class ItemShopCategory {

        public string CategoryName { get; set; }
        public string MenuTitle { get; set; }
        public string Permission { get; set; }
        public List<ItemConf> Items { get; set; }
    
        public ItemShopCategory() {}

        public ItemShopCategory(string name, string menuTitle, string openPermission, List<ItemConf> items) {
            this.CategoryName = name;
            this.MenuTitle = menuTitle;
            this.Permission = openPermission;
            this.Items = items;
        }

    }
}
