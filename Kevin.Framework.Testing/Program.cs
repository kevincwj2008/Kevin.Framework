using Kevin.Framework.Infrastructure.PagingQuery;
using Kevin.Framework.Testing.Code;
using System.Collections.Generic;

namespace Kevin.Framework.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var listDog = new List<Dog>() { new Dog() { id = 1, name = "dog1", Home = "home1" }, new Dog() { id = 2, name = "dog2", Home = "home2" }, new Dog() { id = 3, name = "dog3", Home = "home3" } };
            IPageResult<Animal> aa = new PageResult<Dog>() { Data = listDog, TotalCount = 3, CurrentPage = 1, TotalPage = 2 };
            
        }
    }
}
