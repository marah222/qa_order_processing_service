using Moq;

namespace OrderProcessor.Tests
{
    public class ProcessorTests
    {
        private Mock<ICustomerRepository> _mockCustomerRepo;
        private Processor _processor;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _processor = new Processor(_mockCustomerRepo.Object);
        }

        [Test]
        public void ProcessOrder_WithSufficientFunds_ApprovesOrder()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 200m };
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_WithSufficientFunds_EdgeCase()
        {
            //Arange
            var customer = new Customer { AccountBalance = 250 };
            _mockCustomerRepo.Setup(r => r.GetCustomer(2)).Returns(customer);
            var order = new Order { Amount = 250 };

            // Act
            var result = _processor.ProcessOrder(2, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_WithSufficientFunds_NegativeCase()
        {
            //Arange
            var customer = new Customer { AccountBalance = 250 };
            _mockCustomerRepo.Setup(r => r.GetCustomer(3)).Returns(customer);
            var order = new Order { Amount = 300 };

            // Act
            var result = _processor.ProcessOrder(3, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }
    }
}