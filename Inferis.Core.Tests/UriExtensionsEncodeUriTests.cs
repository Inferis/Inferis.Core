using System.Collections.Generic;
using Inferis.Core.Extensions;
using NUnit.Framework;

namespace Inferis.Core.Tests
{
    [TestFixture]
    public class UriExtensionsEncodeUriTests
    {
        [Test]
        public void EncodeForUri_NullParametersAndNoInitialQuestionMark_ReturnsEmptyString()
        {
            Dictionary<string, string> source = null;
            Assert.IsEmpty(source.EncodeForUri(false));
        }

        [Test]
        public void EncodeForUri_NullParametersAndInitialQuestionMark_ReturnsQuestionMark()
        {
            Dictionary<string, string> source = null;
            Assert.AreEqual("?", source.EncodeForUri(true));
        }

        [Test]
        public void EncodeForUri_EmptyParametersAndNoInitialQuestionMark_ReturnsEmptyString()
        {
            var source = new Dictionary<string, string>();
            Assert.IsEmpty(source.EncodeForUri(false));
        }

        [Test]
        public void EncodeForUri_EmptyParametersAndInitialQuestionMark_ReturnsQuestionMark()
        {
            var source = new Dictionary<string, string>();
            Assert.AreEqual("?", source.EncodeForUri(true));
        }

        [Test]
        public void EncodeForUri_OneParameterAndNoInitialQuestionMark_ReturnsSinglePair()
        {
            var source = new Dictionary<string, string>() {{"test", "value"}};
            Assert.AreEqual("test=value", source.EncodeForUri(false));
        }

        [Test]
        public void EncodeForUri_OneParameterAndInitialQuestionMark_ReturnsSinglePair()
        {
            var source = new Dictionary<string, string>() { { "test", "value" } };
            Assert.AreEqual("?test=value", source.EncodeForUri(true));
        }

        [Test]
        public void EncodeForUri_MultiParameterAndNoInitialQuestionMark_ReturnsSinglePair()
        {
            var source = new Dictionary<string, string>() { { "test", "value" }, { "foo", "bar" } };
            Assert.AreEqual("test=value&foo=bar", source.EncodeForUri(false));
        }

        [Test]
        public void EncodeForUri_MultiParameterAndInitialQuestionMark_ReturnsSinglePair()
        {
            var source = new Dictionary<string, string>() { { "test", "value" }, { "foo", "bar" } };
            Assert.AreEqual("?test=value&foo=bar", source.EncodeForUri(true));
        }

        [Test]
        public void EncodeForUri_UrlEncodesKeys()
        {
            var source = new Dictionary<string, string>() { { "?&", "value" }, { "a + b", "bar" } };
            Assert.AreEqual("?%3F%26=value&a%20%2B%20b=bar", source.EncodeForUri(true));
        }

        [Test]
        public void EncodeForUri_UrlEncodesValues()
        {
            var source = new Dictionary<string, string>() { { "foo", "%bar" }, { "bar", "+ baz +" } };
            Assert.AreEqual("?foo=%25bar&bar=%2B%20baz%20%2B", source.EncodeForUri(true));
        }
    }
}
