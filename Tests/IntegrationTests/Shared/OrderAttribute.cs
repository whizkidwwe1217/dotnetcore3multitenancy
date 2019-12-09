using System;

namespace IntegrationTests
{
    public class OrderAttribute : Attribute
    {
        public int I { get; set; }
        public OrderAttribute(int i)
        {
            I = i;
        }
    }
}