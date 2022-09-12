using Inmeta.Moving.DataAccess;
using System;
using Xunit;

namespace Inmeta.Moving.Services.Tests
{
    public class OrdersServiceTests
    {
        [Fact]
        public void Constructor_ShouldThrowException_IfDatabaseContextIsNull()
        {
            // Arrange // Act

            IOrdersDatabaseContext context = null;

            // Assert

            Assert.Throws<ArgumentNullException>(() => new OrdersService(context));
        }
    }
}