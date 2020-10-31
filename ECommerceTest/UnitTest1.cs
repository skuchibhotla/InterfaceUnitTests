using ECommerce.Controllers;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using static ECommerce.Models.Models;

namespace ECommerceTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void ShouldReturnedCharged()
        {
            var cardMock = new Mock<ICard>();
            var addressInfoMock = new Mock<IAddressInfo>();
            var cartItemMock = new Mock<CartItem>();
            cartItemMock.Setup(c => c.Price).Returns(10);
            var cartServiceMock = new Mock<ICartService>();

            List<CartItem> items = new List<CartItem>();
            items.Add(cartItemMock.Object);

            cartServiceMock.Setup(p => p.Items()).Returns(items.AsEnumerable());
            var shipmentServiceMock = new Mock<IShipmentService>();

            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(true);

            // Act
            CartController controller = new CartController(cartServiceMock.Object, paymentServiceMock.Object, shipmentServiceMock.Object);
            var result = controller.Checkout(cardMock.Object, addressInfoMock.Object);

            // Assert
            Assert.AreEqual(result, "charged");
            shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Once);
        }

        [TestMethod]
        public void ShouldReturnNotCharged()
        {
            // Arrange
            var cardMock = new Mock<ICard>();
            var addressInfoMock = new Mock<IAddressInfo>();
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(false);

            var cartServiceMock = new Mock<ICartService>();
            var shipmentServiceMock = new Mock<IShipmentService>();

            List<CartItem> items = new List<CartItem>();

            CartController controller = new CartController(cartServiceMock.Object, paymentServiceMock.Object, shipmentServiceMock.Object);

            // Act
            var result = controller.Checkout(cardMock.Object, addressInfoMock.Object);

            // Assert
            Assert.AreEqual(result, "not charged");
            shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Never);
        }


    }
}
