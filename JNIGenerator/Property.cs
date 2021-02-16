using System;
using System.Collections.Generic;

namespace JNIGenerator
{
    public class Property
    {
        public string Name { get; set; }
        public LType Type { get; set; }
        public bool Required { get; set; }
        public int ArrayLength { get; set; }

        public Property()
        {

        }

        internal Property Clone()
        {
            return new Property
            {
                Name = Name,
                Type = Type.Clone(),
                Required = Required,
                ArrayLength = ArrayLength
            };
        }
    }
}