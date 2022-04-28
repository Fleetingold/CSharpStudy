using Flurl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlurlDemoTests
{
    public class FlurlTest
    {
        [TestMethod()]
        public void SetQueryParamTest()
        {
            var url = "http://www.mysite.com".SetQueryParam("x", new[] { 1, 2, 3 });
            Assert.AreEqual("http://www.mysite.com?x=1&x=2&x=3", url);
        }
    }
}
