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
        
        // test the positive case
        [Test]
        public void ProcessOrder_WithSufficientFunds_ApprovesOrder()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 200 };
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100 };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }


        #region Task E

        [Test]
        public void ProcessOrder_WithSufficientFunds_Rush_NonPremium_Approved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 200m , IsPremium = false};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush = true };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert

            Assert.IsTrue(result.IsApproved);
        }
        
         // test the edge case where account balance equals order amount
        [Test]
        public void ProcessOrder_WithExactFunds_ApprovesOrder()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 100 };
            _mockCustomerRepo.Setup(r => r.GetCustomer(2)).Returns(customer);
            var order = new Order { Amount = 100 };
            // Act
            var result = _processor.ProcessOrder(2, order);

            //Assert
            Assert.IsTrue(result.IsApproved);
        }
        [Test]
        public void ProcessOrder_WithNonSufficientFunds_Rush_NonPremium_NotApproved()
        {
            // Arrange

            var customer = new Customer { AccountBalance = 100m , IsPremium = false};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush = true };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_WithSufficientFunds_Rush_Premium_Approved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 200m , IsPremium = true};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush = true };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        // test the negative case 
        [Test]

        public void ProcessOrder_WithInsufficientFunds_DeclinesOrder()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 50 };
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100 };
            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }

        public void ProcessOrder_WithNonSufficientFunds_Rush_Premium_Approved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 100m , IsPremium = true};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush = true };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }
        

        [Test]
        public void ProcessOrder_WithSufficientFunds_NotRush_Premium_Approved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 100m , IsPremium = true};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush = false };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_WithNonSufficientFunds_NotRush_Premium_Approved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 100m , IsPremium = true};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 10000m , IsRush = false };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_WithSufficientFunds_NotRush_NonPremium_Approved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 100m , IsPremium = false};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m , IsRush = false };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_WithNonSufficientFunds_NotRush_NonPremium_NotApproved()
        {
            // Arrange
            var customer = new Customer { AccountBalance = 100m , IsPremium = false};
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 10000m , IsRush = false };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        }


        #endregion
    }
}