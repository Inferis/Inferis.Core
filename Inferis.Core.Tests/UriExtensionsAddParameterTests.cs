using System;
using Inferis.Core.Extensions;
using NUnit.Framework;

namespace Inferis.Core.Tests
{
    [TestFixture]
    public class UriExtensionsAddParametersTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddParameters_ToNullUri_Throws()
        {
            Uri source = null;
            source.AddParameters(null);
        }
    }
}
