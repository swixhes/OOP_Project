using DiscountMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Constructor_ValidParameters_ShouldSetProperties()
        {
            // Arrange & Act
            var user = new RegisteredUser(1, "test@mail.com", "����", "��������", "+38(067)-1234567", "12345", 100);

            // Assert
            Assert.AreEqual("test@mail.com", user.Email);
            Assert.AreEqual("����", user.FirstName);
            Assert.AreEqual("��������", user.LastName);
            Assert.AreEqual("+38(067)-1234567", user.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Email_InvalidFormat_ShouldThrowException()
        {
            // Arrange & Act
            var user = new RegisteredUser(1, "invalidEmail", "����", "��������", "+38(067)-1234567", "12345", 100);
        }
    }

}