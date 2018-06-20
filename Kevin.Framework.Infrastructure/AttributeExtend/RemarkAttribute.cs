using System;

namespace Kevin.Framework.Infrastructure.AttributeExtend
{
    public class RemarkAttribute : Attribute
    {
        public string Text { get; set; }

        public RemarkAttribute(string text)
        {
            Text = text;
        }
    }
}
