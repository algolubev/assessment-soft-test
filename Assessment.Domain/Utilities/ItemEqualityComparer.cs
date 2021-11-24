using System;
using System.Collections.Generic;

namespace Assessment.Domain.Utilities
{
    public class ItemEqualityComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            return (x.ItemId == y.ItemId);
        }

        public int GetHashCode(Item obj)
        {
            return obj.ItemId.GetHashCode();
        }
    }

}
