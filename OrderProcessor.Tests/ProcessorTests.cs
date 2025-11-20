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
        public void ChecKOrder_StandardCustomer_WithInsufficientFunds_ShouldRejectOrder()
        {
            var customer = new Customer { AccountBalance = 99.9m , IsPremium=false};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            
            var order = new Order { Amount = 100m };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }


        [Test]
        public void ProcessOrder_StandardCustomer_RushOrder_WithInSufficientFunds_ShouldRejectOrder()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 110m , IsPremium=false};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush=true};

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }

        [Test]
        public void ChecKOrder_StandardCustomer_WithZeroBalance_ShouldRejectOrder()
        {
            var customer = new Customer { AccountBalance = 0m, IsPremium = false };
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);

            var order = new Order { Amount = 1m };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }



    }
}