using System;
using DataService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataService.Tests
{
    [TestClass()]
    public class AccessServiceTests
    {
        [TestMethod()]
        public void IsUserAuthorizedTest()
        {
            Assert.IsTrue(true);
        }
    }
}

namespace DataAccess.Tests
{
    [TestClass]
    public class AccessServiceTests
    {
        [TestMethod]
        public void AccessServiceConstructorWorks()
        {
            try
            {
                var dataAccess = new AccessService();
                if (dataAccess == null) throw new Exception("AccessService constructor returned null");
            }
            catch (Exception e)
            {
                Assert.Fail("AccessService constructor threw exception: " + e.Message);
                throw;
            }
        }
    }
}
