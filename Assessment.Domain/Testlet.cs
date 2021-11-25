using Assessment.Domain.Exceptions;
using Assessment.Domain.Utilities.GenericListExtension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assessment.Domain
{
    public class Testlet
    {
        public const int MaxPretestItemsCount = 4;
        public const int MaxOperationalItemsCount = 6;
        public const int MaxItemsCount = MaxPretestItemsCount + MaxOperationalItemsCount;
        public const int MaxTopPretestItemsCount = 2;

        public string TestletId { get; private set; }
        private readonly List<Item> items;

        public Testlet(string testletId, List<Item> items)
        {
            if (string.IsNullOrWhiteSpace(testletId))
            {
                throw new ArgumentException("Cannot be null or empty.", "testletId");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            TestletId = testletId;

            ValidateItems(items);
            this.items = new List<Item>(items);
        }

        protected void ValidateItems(List<Item> items)
        {
            if (items.Count != MaxItemsCount)
            {
                throw new ArgumentOutOfRangeException("argument items", new TestletValidationException($"Testlet contains only { MaxItemsCount } items."));
            }
            var pretestItemsCount = items.Where(item => item.ItemType == ItemType.Pretest).Count();
            if (pretestItemsCount != MaxPretestItemsCount)
            {
                throw new ArgumentException("items", new TestletValidationException($"Testlet contains only { MaxPretestItemsCount } pretest items."));
            }
            var operationalItemsCount = items.Where(item => item.ItemType == ItemType.Operational).Count();
            if (pretestItemsCount != MaxPretestItemsCount)
            {
                throw new ArgumentException("items", new TestletValidationException($"Testlet contains only { MaxOperationalItemsCount } operational items."));
            }
        }

        public List<Item> Randomize()
        {
            var randomizedItems = new List<Item>(items);
            randomizedItems.Shuffle();

            var orderedItems = new List<Item>();

            var twoFirstPretestItems = randomizedItems
                .Where(item => item.ItemType == ItemType.Pretest)
                .Take(MaxTopPretestItemsCount)
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