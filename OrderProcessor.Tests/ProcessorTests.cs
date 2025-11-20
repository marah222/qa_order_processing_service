using System.Reflection.Metadata;
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
        public void ProcessOrder_PremiumCustomer_WithInsufficientFunds_ApprovesOrder()
        {
            // Arrange
            // Set IsPremium to true and Balance to $10 (less than order amount)
            var customer = new Customer
            {
                AccountBalance = 10m,
                IsPremium = true
            };

            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);

            // Create an order of $500 (greater than balance)
            var order = new Order { Amount = 500m };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            // This proves the "Premium Customer Override" logic works
            Assert.IsTrue(result.IsApproved, "Premium customers should be approved regardless of insufficient funds.");
        }


        [Test]
        public void ProcessOrder_PremiumCustomer_WithSufficientFunds_ApprovesOrder()
        {
            // Arrange
            var customer = new Customer { IsPremium=true, AccountBalance = 200m };
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 100m };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsTrue(result.IsApproved);
        }

        [Test]
        public void ProcessOrder_StandardCustomer_WithInsufficientFunds_ApprovesOrder()
        {
            // Arrange
            var customer = new Customer { IsPremium = false, AccountBalance = 200m };
            _mockCustomerRepo.Setup(r => r.GetCustomer(1)).Returns(customer);
            var order = new Order { Amount = 300m };

            // Act
            var result = _processor.ProcessOrder(1, order);

            // Assert
            Assert.IsFalse(result.IsApproved);
        [TestCase(1, 200, 100)]
        public void ProcessOrder_PremiumCustomerWithRush_ShouldNotChargeFee(int id, decimal accountBalance, decimal orderAmount)
        {
            // Arrange
            var premiumCustomer = new Customer() { AccountBalance = accountBalance, IsPremium = true };
            _mockCustomerRepo.Setup(r => r.GetCustomer(id)).Returns(premiumCustomer);

            var order = new Order { Amount = orderAmount, IsRush = true };

            //Act
            var result = _processor.ProcessOrder(id, order);

            //Assert
            Assert.That(premiumCustomer.AccountBalance, Is.EqualTo(accountBalance - orderAmount));
        }
        [Test]
        [TestCase(1, 200, 100)]
        public void ProcessOrder_StandardCustomerWithRush_ShouldNotChargeFee(int id, decimal accountBalance, decimal orderAmount)
        {
            // Arrange
            var premiumCustomer = new Customer() { AccountBalance = accountBalance, IsPremium = false };
            _mockCustomerRepo.Setup(r => r.GetCustomer(id)).Returns(premiumCustomer);

            var order = new Order { Amount = orderAmount, IsRush = true };

            //Act
            var result = _processor.ProcessOrder(id, order);

            //Assert
            Assert.That(premiumCustomer.AccountBalance, Is.EqualTo(accountBalance - orderAmount  - 20));
        }
        
        [Test]
        [TestCase(1,200,100, true)]
        [TestCase(1,200,100, false)]
        public void ProcessOrder_NonRushOrder_ShouldNotChargeFee(int  id, decimal accountBalance, decimal orderAmount, bool isPremium)
        {
            // Arrange
            var premiumCustomer = new Customer() { AccountBalance = accountBalance, IsPremium = isPremium };
            _mockCustomerRepo.Setup(r => r.GetCustomer(id)).Returns(premiumCustomer);

            var order = new Order { Amount = orderAmount, IsRush = false };

            //Act
            var result = _processor.ProcessOrder(id, order);

            //Assert
            Assert.That(premiumCustomer.AccountBalance, Is.EqualTo(accountBalance - orderAmount));
        }
    }
}