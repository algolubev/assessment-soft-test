using Assessment.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Assessment.Domain.Tests
{
    public class RandomizedTestletTests
    {
        private readonly List<Item> sut;
        private const int MaxItemNumber = 10;

        public RandomizedTestletTests()
        {
            // arrange
            var items = ItemsBuilder.BuildItems(ItemType.Operational, 6);
            items.AddRange(ItemsBuilder.BuildItems(ItemType.Pretest, 4));

            var testlet = new Testlet(Guid.NewGuid().ToString(), items);
            
            // act
            sut = testlet.Randomize();
        }

        [Fact]
        public void Items_are_unique()
        {
            var expectedUniqueItemsCount = MaxItemNumber;

            var uniqueItemsCount = sut.Distinct(new ItemEqualityComparer()).Count();
            Assert.Equal(expectedUniqueItemsCount, uniqueItemsCount);
        }

        [Theory]
        [InlineData(1, ItemType.Pretest)]
        [InlineData(2, ItemType.Pretest)]
        public void Nth_item_has_expected_type(int itemIndex, ItemType expectedItemType)
        {
            if (itemIndex <= 0 || itemIndex > MaxItemNumber)
            {
                throw new ArgumentOutOfRangeException($"itemIndex should be between 1 and { MaxItemNumber }.");
            }
            --itemIndex;

            var nthItem = sut[itemIndex];
            Assert.Equal(expectedItemType, nthItem.ItemType);
        }

        [Fact]
        public void The_rest_of_items_include_two_pretest()
        {
            var expectedRemaindingRandimizedPretestItemsCount = 2;
            var remaindingRandimizedItems = sut.Skip(2).ToList();
            var remaindingRandimizedPretestItems = remaindingRandimizedItems.Where(item => item.ItemType == ItemType.Pretest).ToList();
            Assert.Equal(expectedRemaindingRandimizedPretestItemsCount, remaindingRandimizedPretestItems.Count);
        }

        [Fact]
        public void The_rest_of_items_include_six_operational()
        {
            var expectedRemaindingRandimizedOperationalItemsCount = 6;
            var remaindingRandimizedItems = sut.Skip(2).ToList();
            var remaindingRandimizedOperationalItems = remaindingRandimizedItems.Where(item => item.ItemType == ItemType.Operational).ToList();
            Assert.Equal(expectedRemaindingRandimizedOperationalItemsCount, remaindingRandimizedOperationalItems.Count);
        }

    }
}
