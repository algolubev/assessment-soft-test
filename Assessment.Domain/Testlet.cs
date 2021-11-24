using Assessment.Domain.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Assessment.Domain
{
    public class Testlet
    {
        public string TestletId { get; private set; }
        private readonly List<Item> items;

        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            this.items = items;
        }

        public List<Item> Randomize()
        {
            var randomizedItems = new List<Item>(items);
            randomizedItems.Shuffle();
            
            var orderedItems = new List<Item>();

            var twoFirstPretestItems = randomizedItems
                .Where(item => item.ItemType == ItemType.Pretest)
                .Take(2)
                .ToList();
            orderedItems.AddRange(twoFirstPretestItems);

            var remainingItems = randomizedItems
                .Where(item => item.ItemId != twoFirstPretestItems[0].ItemId && item.ItemId != twoFirstPretestItems[1].ItemId)
                .ToList();
            orderedItems.AddRange(remainingItems);

            return orderedItems;
        }
    }
}