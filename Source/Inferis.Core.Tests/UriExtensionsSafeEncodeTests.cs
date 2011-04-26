using System;
using System.Text;
using Inferis.Core.Extensions;
using NUnit.Framework;

namespace Inferis.Core.Tests
{
    [TestFixture]
    public class UriExtensionsSafeEncodeTests
    {
        #region SafeEncodeTests
        [Test]
        public void SafeEncode_WithNullString_ReturnsEmpty()
        {
            string source = null;
            Assert.IsEmpty(source.SafeEncode());
        }

        [Test]
        public void SafeEncode_WithEmptyString_ReturnsEmpty()
        {
            string source = null;
            Assert.IsEmpty(source.SafeEncode());
        }

        [Test]
        public void SafeEncode_WithPlainString_ReturnsSame()
        {
            var source = "abc";
            Assert.AreEqual(source, source.SafeEncode());
        }
        [Test]
        public void SafeEncode_WithOneEncodableCharacter_ReturnsCorrectEncoding()
        {
            var source = "ab?cd";
            Assert.AreEqual("ab%3Fcd", source.SafeEncode());
        }

        [Test]
        public void SafeEncode_WithAllEncodableCharacters_ReturnsCorrectEncoding()
        {
            var source = @" !*'();:@&=+$,/?%#[]""";
            Assert.AreEqual("%20%21%2A%27%28%29%3B%3A%40%26%3D%2B%24%2C%2F%3F%25%23%5B%5D%22", source.SafeEncode());
        }
        #endregion

        #region SafeEncodeToTests
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SafeEncodeTo_WithNullTarget_Throws()
        {
            string source = null;
            source.SafeEncodeTo(null);
        }

        [Test]
        public void SafeEncodeTo_WithNullString_ReturnsEmpty()
        {
            string source = null;
            var target = new StringBuilder();
            source.SafeEncodeTo(target);
            Assert.AreEqual(0, target.Length);
        }

        [Test]
        public void SafeEncodeTo_WithEmptyString_ReturnsEmpty()
        {
            string source = null;
            var target = new StringBuilder();
            source.SafeEncodeTo(target);
            Assert.AreEqual(0, target.Length);
        }

        [Test]
        public void SafeEncodeTo_WithPlainString_ReturnsSame()
        {
            var source = "abc";
            var target = new StringBuilder();
            source.SafeEncodeTo(target);
            Assert.AreEqual(source, target.ToString());
        }

        [Test]
        public void SafeEncodeTo_WithOneEncodableCharacter_ReturnsCorrectEncoding()
        {
            var source = "ab?cd";
            var target = new StringBuilder();
            source.SafeEncodeTo(target);
            Assert.AreEqual("ab%3Fcd", target.ToString());
        }

        [Test]
        public void SafeEncodeTo_WithAllEncodableCharacters_ReturnsCorrectEncoding()
        {
            var source = @" !*'();:@&=+$,/?%#[]""";
            var target = new StringBuilder();
            source.SafeEncodeTo(target);
            Assert.AreEqual("%20%21%2A%27%28%29%3B%3A%40%26%3D%2B%24%2C%2F%3F%25%23%5B%5D%22", target.ToString());
        }
        #endregion
        }
}
