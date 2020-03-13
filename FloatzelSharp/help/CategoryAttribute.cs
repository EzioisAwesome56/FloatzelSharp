using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp.help
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CategoryAttribute : Attribute
    {

        public CategoryAttribute(Category category) => Category = category;

        public Category Category { get; }

    }
}
