using Microsoft.AspNetCore.Mvc;
using ProcessPayment.Data.Services;
using ProcessPayment.DTO;
using ProcessPayment.Models;
using ProcessPayment.Utilities;
using System;
using System.Threading.Tasks;

namespace ProcessPayment.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentProcessor : ControllerBase
    {
        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        private readonly IExpensivePaymentGateway _expensivePaymentGateway;
        private readonly IPremiumPaymentGateway _premiumPaymentGateway;
        private readonly IStatusRepository _statusRepo;
        private int paymentCounter = 0;

        //dependency injection of interfaces into the PaymentProcessor constructor
        public PaymentProcessor(ICheapPaymentGateway cheapPaymentGateway, IExpensivePaymentGateway expensivePaymentGateway,
                                IPremiumPaymentGateway premiumPaymentGateway, IStatusRepository statusRepository)
        {
            _cheapPaymentGateway = cheapPaymentGateway;
            _expensivePaymentGateway = expensivePaymentGateway;
            _premiumPaymentGateway = premiumPaymentGateway;
            _statusRepo = statusRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.Amount < 1M)
                    return BadRequest(Message.ResponseMessage("Error", errors: new { message = "Amount cannot be less than 1 pound" }));

                if (model.ExpirationDate < DateTime.Today)
                    return BadRequest(Message.ResponseMessage("Error", errors: new { message = "Sorry, your card has expired" }));

                var paymentAmount = model.Amount;

                string id;
                PaymentModel payment;
                bool changePaymentStatus;

                //handling of cheap payments
                if (paymentAmount < 21M)
                {
                    do
                    {
                        id = Guid.NewGuid().ToString();
                        payment = await _cheapPaymentGateway.GetCheapPaymentById(id);
                    } while (payment != null);

                    var CheapPayment = new PaymentModel
                    {
                        PaymentId = id,
                        CreditCardNumber = model.CreditCardNumber,
                        CardHolder = model.CardHolder,
                        ExpirationDate = model.ExpirationDate,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount
                    };

                    try
                    {
                        var result = await _cheapPaymentGateway.AddCheapPayment(CheapPayment);

                        //update the status of the payment
                        PaymentState status;
                        if (result) 
                        {
                            var RecentlyAdded = await _cheapPaymentGateway.GetCheapPaymentById(id);
                            status = await _statusRepo.GetStatusByName("success");
                            RecentlyAdded.PaymentStateId = status.PaymentStateId; //error here
                            changePaymentStatus = await _cheapPaymentGateway.UpdateCheapPayment(RecentlyAdded);

                            if (changePaymentStatus)
                            {
                                paymentCounter = 0;
                                return Ok(Message.ResponseMessage("Success", data: new { message = $"Payment of {(model.Amount > 1 ? model.Amount + " pounds" : model.Amount + " pound")} was successful" }));
                            }
                            //if update of status doesn't succeed perform a rollback to 'pending'
                            paymentCounter = 0;
                            status = await _statusRepo.GetStatusByName("failed");
                            RecentlyAdded.PaymentStateId = status.PaymentStateId;
                            changePaymentStatus = await _cheapPaymentGateway.UpdateCheapPayment(RecentlyAdded);
                            return StatusCode(500, Message.ResponseMessage("Error", errors: new { message = $"Couldn't update payment status" }));
                        }
                        else
                            return StatusCode(500, Message.ResponseMessage("Error", errors: new { message = $"Couldn't make payment of {(model.Amount > 1 ? model.Amount + " pounds" : model.Amount + " pound")}. Try again" }));
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, Message.ResponseMessage("Server Error", errors: new { message = ex.Message }));
                    }
                }
                //handling of expensive payments
                if (paymentAmount >= 21M && paymentAmount <= 500M)
                {
                    do
                    {
                        id = Guid.NewGuid().ToString();
                        payment = await _expensivePaymentGateway.GetExpensivePaymentById(id);
                    } while (payment != null);

                    var ExpensivePayment = new PaymentModel
                    {
                        PaymentId = id,
                        CreditCardNumber = model.CreditCardNumber,
                        CardHolder = model.CardHolder,
                        ExpirationDate = model.ExpirationDate,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount
                    };

                    try
                    {
                        var result = await _expensivePaymentGateway.AddExpensivePayment(ExpensivePayment);

                        //update the status of the payment
                        PaymentState status;
                        if (result)
                        {
                            var RecentlyAdded = await _expensivePaymentGateway.GetExpensivePaymentById(id);
                            status = await _statusRepo.GetStatusByName("success");
                            RecentlyAdded.PaymentStateId = status.PaymentStateId;
                            changePaymentStatus = await _expensivePaymentGateway.UpdateExpensivePayment(RecentlyAdded);

                            if (changePaymentStatus)
                            {
                                paymentCounter = 0;
                                return Ok(Message.ResponseMessage("Success", data: new { message = $"Payment of {model.Amount} pounds was successful" }));
                            }
                            //if update of status doesn't succeed perform a rollback to 'pending'
                            paymentCounter = 0;
                            status = await _statusRepo.GetStatusByName("failed");
                            RecentlyAdded.PaymentStateId = status.PaymentStateId;
                            changePaymentStatus = await _expensivePaymentGateway.UpdateExpensivePayment(RecentlyAdded);
                            return StatusCode(500, Message.ResponseMessage("Error", errors: new { message = $"Couldn't update payment status" }));
                        }
                        else
                            return StatusCode(500, Message.ResponseMessage("Error", errors: new { message = $"Couldn't make payment of {(model.Amount > 1 ? model.Amount + " pounds" : model.Amount + " pound")}. Try again" }));
                    }
                    catch (Exception ex)
                    {
                        //Execute the cheapPaymentGateway once if expensive payment fails
                        if (paymentCounter < 1)
                        {
                            var tryCheapPayment = await _cheapPaymentGateway.AddCheapPayment(ExpensivePayment);

                            if (tryCheapPayment)
                                paymentCounter++;
                                return Ok(Message.ResponseMessage("Success", data: new { message = $"Payment of {model.Amount} pounds was successful" }));
                        }
                        return StatusCode(500, Message.ResponseMessage("Server Error", errors: new { message = ex.Message }));
                    }
                }
                if (paymentAmount > 500)
                {
                    //handling of premium payments
                    premiumPay:
                    do
                    {
                        id = Guid.NewGuid().ToString();
                        payment = await _expensivePaymentGateway.GetExpensivePaymentById(id);
                    } while (payment != null);

                    var PremiumPayment = new PaymentModel
                    {
                        PaymentId = id,
                        CreditCardNumber = model.CreditCardNumber,
                        CardHolder = model.CardHolder,
                        ExpirationDate = model.ExpirationDate,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount
                    };

                    PaymentState status;
                    try
                    {
                        var result = await _premiumPaymentGateway.AddPremiumPayment(PremiumPayment);
                        
                        //update the status of the payment
                        if (result)
                        {
                            var RecentlyAdded = await _premiumPaymentGateway.GetPremiumPaymentById(id);
                            status = await _statusRepo.GetStatusByName("success");
                            RecentlyAdded.PaymentStateId = status.PaymentStateId;
                            changePaymentStatus = await _premiumPaymentGateway.UpdatePremiumPayment(RecentlyAdded);

                            if (changePaymentStatus)
                            {
                                paymentCounter = 0;
                                return Ok(Message.ResponseMessage("Success", data: new { message = $"Payment of {model.Amount} pounds was successful" }));
                            }
                            //if update of status doesn't succeed perform a rollback to 'pending'
                            paymentCounter = 0;
                            status = await _statusRepo.GetStatusByName("failed");
                            RecentlyAdded.PaymentStateId = status.PaymentStateId;
                            changePaymentStatus = await _premiumPaymentGateway.UpdatePremiumPayment(RecentlyAdded);
                            return StatusCode(500, Message.ResponseMessage("Error", errors: new { message = $"Couldn't update payment status" }));
                        }
                        else
                            return StatusCode(500, Message.ResponseMessage("Error", errors: new { message = $"Couldn't make payment of {(model.Amount > 1 ? model.Amount + " pounds" : model.Amount + " pound")}. Try again" }));
                    }
                    catch (Exception ex)
                    {
                        while (paymentCounter < 3)
                        {
                            paymentCounter++;
                            goto premiumPay;
                        }

                        return StatusCode(500, Message.ResponseMessage("Server Error", errors: new { message = ex.Message }));
                    }
                }
            }

            return BadRequest(Message.ResponseMessage("Error", errors: new { message = ModelState }));
        }
    }
}