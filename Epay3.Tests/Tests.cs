using Epay3.Common;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRandomNumber()
        {
            var generateRandomNumberCode = CommonExtensions.GenerateRandomNumberCode(5);
            Assert.AreEqual(5, generateRandomNumberCode.Length);
        }

        [Test]
        public void EncryptDecrypt()
        {
            var plain = "hello";

            var processed = plain.Encrypt("epay").Decrypt("epay");

            Assert.AreEqual(plain,processed);
        }

        [Test]
        public void Mask()
        {
            string original = "Gökçer Gökdal";
            var masked = original.Mask();

            Assert.AreEqual("G*** G***",masked);
            // todo test more
        }

    }
}