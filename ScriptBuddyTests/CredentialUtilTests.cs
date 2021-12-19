using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBuddy;

namespace ScriptBuddyTests
{
    /// <summary>
    /// This test class tests all the methods related to the CredentialUtils class.
    /// </summary>
    [TestClass]
    public class CredentialUtilTests
    {
        [DataTestMethod]
        [DataRow("", false, "too short")]
        [DataRow("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false, "too long")]
        [DataRow("AAAJjjj99mn3n49", true, "")]
        public void TestValidateUsername(string username, bool expectedValid, string expectedMessage)
        {
            (bool valid, string message) result = CredentialUtils.validateUsername(username);
            Assert.AreEqual((expectedValid, expectedMessage), result);
        }

        [DataTestMethod]
        [DataRow("", false, "too short")]
        [DataRow("AAAAAAA", false, "too short")]
        [DataRow("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false, "too long")]
        [DataRow("AAAJjjj99mn3n49", true, "")]
        public void TestValidatePassword(string username, bool expectedValid, string expectedMessage)
        {
            (bool valid, string message) result = CredentialUtils.validatePassword(username);
            Assert.AreEqual((expectedValid, expectedMessage), result);
        }
    }
}
