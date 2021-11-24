using System;
using Xunit;

namespace Assessment.Domain.Tests
{
    public class RandomizedTestletTests
    {
        [Fact]
        public void Items_are_unique()
        {
            var testletId = Guid.NewGuid().ToString();
            var items = new List<Item>();
            var sut = new Testlet(testletId, items);

            var randimizedItems = sut.Randomize();
            var uniqueItemsCount = randimizedItems.Distinct().Count();
            Assert.Equal(items.Count(), uniqueItemsCount);
        }

        [Fact]
        public void The_first_item_is_pretest()
        {
            var testletId = Guid.NewGuid().ToString();
            var items = new List<Item>();
            var sut = new Testlet(testletId, items);

            var randimizedItems = sut.Randomize();
            var firstRandimizedItem = randimizedItems[0];

            Assert.Equal(ItemTypeEnum.Pretest, firstRandimizedItem.ItemType);
        }

        [Fact]
        public void The_second_item_is_pretest()
        {
            var testletId = Guid.NewGuid().ToString();
            var items = new List<Item>();
            var sut = new Testlet(testletId, items);

            var randimizedItems = sut.Randomize();
            var firstRandimizedItem = randimizedItems[0];

            Assert.Equal(ItemTypeEnum.Pretest, firstRandimizedItem.ItemType);
        }

        [Fact]
        public void The_rest_of_items_include_two_pretest()
        {
            var expectedRemaindingRandimizedPretestItemsCount = 2;
            var testletId = Guid.NewGuid().ToString();
            var items = new List<Item>();
            var sut = new Testlet(testletId, items);

            var randimizedItems = sut.Randomize();
            var remaindingRandimizedItems = randimizedItems.Skip(2).ToList();
            var remaindingRandimizedPretestItems = remaindingRandimizedItems.Where(item => item.ItemType == ItemTypeEnum.Pretest).ToList();
            Assert.Equal(expectedRemaindingRandimizedPretestItemsCount, remaindingRandimizedPretestItems);
        }

        [Fact]
        public void The_rest_of_items_include_six_operational()
        {
            var expectedRemaindingRandimizedOperationalItemsCount = 6;
            var testletId = Guid.NewGuid().ToString();
            var items = new List<Item>();
            var sut = new Testlet(testletId, items);

            var randimizedItems = sut.Randomize();
            var remaindingRandimizedItems = randimizedItems.Skip(2).ToList();
            var remaindingRandimizedOperationalItems = remaindingRandimizedItems.Where(item => item.ItemType == ItemTypeEnum.Operational).ToList();
            Assert.Equal(expectedRemaindingRandimizedOperationalItemsCount, remaindingRandimizedOperationalItems);
        }

    }
}
