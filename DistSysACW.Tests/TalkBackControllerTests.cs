using DistSysACW.Controllers;
using DistSysACW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DistSysACW.Tests
{
    [TestClass]
    public class TalkBackControllerTests
    {
        private TalkBackController _talkBackController;
        [TestInitialize]
        public void Initialise()
        {
            var userContextMock = new Mock<UserContext>();
            _talkBackController = new TalkBackController(userContextMock.Object);
        }

        [TestMethod]
        public void Hello()
        {
            Assert.AreEqual("Hello World", _talkBackController.Hello());
        }

        [TestMethod]
        public void Sort_Ok()
        {
            int[] values = { 0, 2, 1, 2, 3, 0 };
            int[] expected = { 0, 0, 1, 2, 2, 3 };
            
            var result = _talkBackController.Sort(values) as OkObjectResult;
            var result_value = result.Value;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result_value);
            Assert.AreEqual(200, result.StatusCode);
            CollectionAssert.AreEqual(expected, result.Value as int[]);
        }
    }
}
