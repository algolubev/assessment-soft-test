using Assessment.Domain.Exceptions;
using Assessment.Domain.Utilities.GenericListExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Assessment.Domain.Tests
{
    public class RandomizedTestletTests
    {
        private bool CompareItemSets(List<Item> leftItems, List<Item> rightItems)
        {
            var areEqual = false;
            var maxItemsCount = Math.Max(leftItems.Count, rightItems.Count);
            var intersectedItemsCount = leftItems.Intersect(rightItems).Count();
            areEqual = (intersectedItemsCount == maxItemsCount);

            return areEqual;
        }


        [Trait("category", "defencive")]
        [Fact]
        public void Throws_argument_null_exception_on_missed_testlet_id()
        {
            // arrange
            string testletId = null;
            var items = new List<Item>();
            
            // act
            var caughtException = Assert.Throws<ArgumentException>(() => new Testlet(testletId, items));
            
            // assert
            Assert.IsType<ArgumentException>(caughtException);
        }

        [Trait("category", "defencive")]
        [Fact]
        public void Throws_argument_null_exception_on_missed_testlet_items()
        {
            // arrange
            var testletId = "id";
            List<Item> items = null;
 
            // act
            var caughtException = Assert.Throws<ArgumentNullException>(() => new Testlet(testletId, items));
            
            // assert
            Assert.IsType<ArgumentNullException>(caughtException);
        }

        [Trait("category", "defencive")]
        [Trait("category", "validation")]
        [Fact]
        public void Throws_argument_out_of_range_exception_on_incomplete_set_of_items()
        {
            // arrange
            var testletId = "id";
            var incompleteItems = ItemsBuilder.BuildItems(ItemType.Pretest, Testlet.MaxItemsCount - 1);

            // act
            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(() => new Testlet(testletId, incompleteItems));
            
            // assert
            Assert.IsType<ArgumentOutOfRangeException>(caughtException);
            Assert.IsType<TestletValidationException>(caughtException.InnerException);
        }

        [Trait("category", "defencive")]
        [Trait("category", "validation")]
        [Fact]
        public void Throws_argument_out_of_range_exception_on_overfull_set_of_items()
        {
            // arrange
            var testletId = "id";
            var overfullItems = ItemsBuilder.BuildItems(ItemType.Pretest, Testlet.MaxItemsCount + 1);

            // act
            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(() => new Testlet(testletId, overfullItems));

            // assert
            Assert.IsType<ArgumentOutOfRangeException>(caughtException);
            Assert.IsType<TestletValidationException>(caughtException.InnerException);
        }

        [Trait("category", "defencive")]
        [Trait("category", "validation")]
        [Fact]
        public void Throws_argument_out_of_range_exception_on_empty_set_of_items()
        {
            // arrange
            var testletId = "id";
            var emptyItems = new List<Item>();

            // act
            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(() => new Testlet(testletId, emptyItems));

            // assert
            Assert.IsType<ArgumentOutOfRangeException>(caughtException);
            Assert.IsType<TestletValidationException>(caughtException.InnerException);
        }

        [Trait("category", "defencive")]
        [Trait("category", "validation")]
        [Fact]
        public void Throws_argument_exception_on_incomplet_subset_of_pretest_items()
        {
            // arrange
            var testletId = "id";
            var lessPretestItems = ItemsBuilder.BuildItems(ItemType.Operational, Testlet.MaxOperationalItemsCount + 1);
            lessPretestItems.AddRange(ItemsBuilder.BuildItems(ItemType.Pretest, Testlet.MaxPretestItemsCount - 1));

            // act
            var caughtException = Assert.Throws<ArgumentException>(() => new Testlet(testletId, lessPretestItems));

            // assert
            Assert.IsType<ArgumentException>(caughtException);
            Assert.IsType<TestletValidationException>(caughtException.InnerException);
        }

        [Trait("category", "defencive")]
        [Trait("category", "validation")]
        [Fact]
        public void Throws_argument_exception_on_incomplet_subset_of_operational_items()
        {
            // arrange
            var testletId = "id";
            var lessOperationalItems = ItemsBuilder.BuildItems(ItemType.Operational, Testlet.MaxOperationalItemsCount - 1);
            lessOperationalItems.AddRange(ItemsBuilder.BuildItems(ItemType.Pretest, Testlet.MaxPretestItemsCount + 1));

            // act
            var caughtException = Assert.Throws<ArgumentException>(() => new Testlet(testletId, lessOperationalItems));
            
            // assert
            Assert.IsType<ArgumentException>(caughtException);
            Assert.IsType<TestletValidationException>(caughtException.InnerException);
        }

        [Trait("category", "validation")]
        [Fact]
        public void Shuffle_does_not_loose_items()
        {
            // arrange
            var originalItems = ItemsBuilder.BuildValidItems();
            var shufledItems = new List<Item>(originalItems);

            shufledItems.Shuffle();

            var areEqual = CompareItemSets(originalItems, shufledItems);
            Assert.True(areEqual);

            originalItems[1] = new Item { ItemType = ItemType.Pretest };
            areEqual = CompareItemSets(originalItems, shufledItems);
            Assert.False(areEqual);
        }


        [Trait("category", "randomization")]
        [Fact]
        public void Randomized_items_are_the_same_as_the_original()
        {
            // arrange
            var testletId = "id";
            var originalItems = ItemsBuilder.BuildValidItems();
            var sut = new Testlet(testletId, originalItems);
            
            // act
            var randomizedItems = sut.Randomize();

            // assert
            var expectedEqualItemsCount = Testlet.MaxItemsCount;
            var actualEqualItemsCount = randomizedItems.Intersect(originalItems).Count();
            Assert.Equal(expectedEqualItemsCount, actualEqualItemsCount);
        }

        [Trait("category", "randomization")]
        [Theory]
        [InlineData(0, ItemType.Pretest)]
        [InlineData(1, ItemType.Pretest)]
        public void Nth_item_has_expected_type(int itemIndex, ItemType expectedItemType)
        {
            // arrange
            var sut = new Testlet("UniqueId", ItemsBuilder.BuildValidItems());
            
            // act
            var randomizedItems = sut.Randomize();

            // assert
            var nthItem = randomizedItems[itemIndex];
            Assert.Equal(expectedItemType, nthItem.ItemType);
        }

        [Trait("category", "randomization")]
        [Fact]
        public void The_rest_of_items_include_two_pretest()
        {
            // arrange
            var sut = new Testlet("UniqueId", ItemsBuilder.BuildValidItems());

            // act
            var randomizedItems = sut.Randomize();

            // assert
            var expectedRemaindingRandimizedPretestItemsCount = Testlet.MaxTopPretestItemsCount;
            var remaindingRandimizedItems = randomizedItems.Skip(Testlet.MaxTopPretestItemsCount);
            var remaindingRandimizedPretestItems = remaindingRandimizedItems.Where(item => item.ItemType == ItemType.Pretest).ToList();
            Assert.Equal(expectedRemaindingRandimizedPretestItemsCount, remaindingRandimizedPretestItems.Count);
        }

        [Trait("category", "randomization")]
        [Fact]
        public void The_rest_of_items_include_six_operational()
        {
            // arrange
            var sut = new Testlet("UniqueId", ItemsBuilder.BuildValidItems());
            
            //act
            var randomizedItems = sut.Randomize();

            // assert
            var expectedRemaindingRandimizedOperationalItemsCount = Testlet.MaxOperationalItemsCount;
            var remaindingRandimizedItems = randomizedItems.Skip(Testlet.MaxTopPretestItemsCount);
            var remaindingRandimizedOperationalItems = remaindingRandimizedItems.Where(item => item.ItemType == ItemType.Operational).ToList();
            Assert.Equal(expectedRemaindingRandimizedOperationalItemsCount, remaindingRandimizedOperationalItems.Count);
        }

    }
}
