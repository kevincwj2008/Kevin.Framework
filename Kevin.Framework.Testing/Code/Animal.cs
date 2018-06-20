using Kevin.Framework.Infrastructure.Validate;

namespace Kevin.Framework.Testing.Code
{
    public class Animal
    {
        [Phone]
        public int id { get; set; }

        public string name { get; set; }
    }
}
