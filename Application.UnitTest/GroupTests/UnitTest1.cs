using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.UnitTest.GroupTest
{
    [TestClass]
    public class GroupTests
    {

        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IRepository<Category>> _mockCategoryRepository;
        private Mock<IAppLogger<ProductService>> _mockAppLogger; 
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}