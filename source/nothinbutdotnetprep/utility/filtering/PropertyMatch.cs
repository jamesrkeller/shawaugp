﻿using System;

namespace nothinbutdotnetprep.utility.filtering
{
    public class PropertyMatch<ItemToMatch,PropertyType> : IMatchAnItem<ItemToMatch>
    {
        Func<ItemToMatch, PropertyType> accessor;
        IMatchAnItem<PropertyType> property_match;

        public PropertyMatch(Func<ItemToMatch, PropertyType> accessor, IMatchAnItem<PropertyType> property_match)
        {
            this.accessor = accessor;
            this.property_match = property_match;
        }

        public bool matches(ItemToMatch item)
        {
            return property_match.matches(accessor(item));
        }
    }
}