using System;

namespace Assessment.Domain
{
    public class Item
    {
        public ItemType ItemType { get; set; }
        public string ItemId { get; private set; }
        public string Text { get; set; }

        public Item()
        {
            ItemId = Guid.NewGuid().ToString();
        }
    }
}
