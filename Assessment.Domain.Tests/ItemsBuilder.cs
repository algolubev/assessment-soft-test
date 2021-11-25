using System.Collections.Generic;

namespace Assessment.Domain.Tests
{
    public static class ItemsBuilder
    {
        public static List<Item> BuildItems(ItemType itemType, int number)
        {
            var items = new List<Item>();

            for (var n = 0; n < number; n++)
            {
                items.Add(new Item
                {
                    ItemType = itemType
                });
            }

            return items;
        }

        public static List<Item> BuildValidItems()
        {
            var items = BuildItems(ItemType.Operational, Testlet.MaxOperationalItemsCount);
            items.AddRange(BuildItems(ItemType.Pretest, Testlet.MaxPretestItemsCount));
 
            return items;
        }

    }
}
