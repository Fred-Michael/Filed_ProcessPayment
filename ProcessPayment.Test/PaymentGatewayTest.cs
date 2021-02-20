using Moq;
using ProcessPayment.API.Controllers;
using ProcessPayment.Data.Services;
using ProcessPayment.Models;
using System;
using Xunit;

namespace ProcessPayment.Test
{
    public class PaymentGatewayTest
    {
        private Mock<ICheapPaymentGateway> _cheapPay;
        private Mock<IExpensivePaymentGateway> _expensivePay;
        private Mock<IPremiumPaymentGateway> _premiumPay;
        private Mock<IStatusRepository> _statusRepo;
        private PaymentProcessor _ctrl;

        public PaymentGatewayTest()
        {
            _cheapPay = new Mock<ICheapPaymentGateway>();
            _expensivePay = new Mock<IExpensivePaymentGateway>();
            _premiumPay = new Mock<IPremiumPaymentGateway>();
            _statusRepo = new Mock<IStatusRepository>();

            _ctrl = new PaymentProcessor(_cheapPay.Object, _expensivePay.Object, _premiumPay.Object, _statusRepo.Object);
        }

        [Fact]
        public void CheapPaymentTestAsync()
        {
            //Arrange
            var newCheapPayment = new PaymentModel
            {
                PaymentId = Guid.NewGuid().ToString(),
                CreditCardNumber = "5610591081018250",
                CardHolder = "Fred Doe",
                SecurityCode = "019",
                ExpirationDate = DateTime.Today.AddDays(5),
                Amount = 19.5M,
                PaymentStateId = "6c44e08b-d001-4802-8843-22a714e81fae"
            };
            var cheapsetup =_cheapPay.Setup(p => p.AddCheapPayment(newCheapPayment)).ReturnsAsync(true);

            //Act and Assert
            _expensivePay.Verify(x => x.AddExpensivePayment(newCheapPayment),Times.Never);
            _premiumPay.Verify(x => x.AddPremiumPayment(newCheapPayment), Times.Never);
        }

        [Fact]
        public void ExpensivePaymentTest()
        {
            //Arrange
            var newExpensivePayment = new PaymentModel
            {
                CreditCardNumber = "5610591081018250",
                CardHolder = "Fred Doe",
                SecurityCode = "019",
                ExpirationDate = DateTime.Today.AddDays(5),
                Amount = 370.11M,
                PaymentStateId = "6c44e08b-d001-4802-8843-22a714e81fae"
            };
            var expensiveSetup = _expensivePay.Setup(p => p.AddExpensivePayment(newExpensivePayment)).ReturnsAsync(true);

            //Act and Assert
            _cheapPay.Verify(x => x.AddCheapPayment(newExpensivePayment), Times.Never);
            _premiumPay.Verify(e => e.AddPremiumPayment(newExpensivePayment), Times.Never);
        }

        [Fact]
        public void PremiumPaymentTest()
        {
            //Arrange
            var newPremiumPayment = new PaymentModel
            {
                CreditCardNumber = "5610591081018250",
                CardHolder = "Fred Doe",
                SecurityCode = "019",
                ExpirationDate = DateTime.Today.AddDays(5),
                Amount = 800M,
                PaymentStateId = "6c44e08b-d001-4802-8843-22a714e81fae"
            };
            var premiumSetup = _premiumPay.Setup(p => p.AddPremiumPayment(newPremiumPayment)).ReturnsAsync(true);

            //Act and Assert
            _cheapPay.Verify(x => x.AddCheapPayment(newPremiumPayment), Times.Never);
            _expensivePay.Verify(e => e.AddExpensivePayment(newPremiumPayment), Times.Never);
        }
    }
}