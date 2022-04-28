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

        [TestMethod()]
        public void ParsingUrlTest()
        {
            var url = new Url("https://user:pass@www.mysite.com:1234/with/path?x=1&y=2#foo");
            Assert.AreEqual("https", url.Scheme);
            Assert.AreEqual("user:pass", url.UserInfo);
            Assert.AreEqual("www.mysite.com", url.Host);
            Assert.AreEqual(1234, url.Port);
            Assert.AreEqual("user:pass@www.mysite.com:1234", url.Authority);
            Assert.AreEqual("https://user:pass@www.mysite.com:1234", url.Root);
            Assert.AreEqual("/with/path", url.Path);
            Assert.AreEqual("x=1&y=2", url.Query);
            Assert.AreEqual("foo", url.Fragment);
        }
    }
}
