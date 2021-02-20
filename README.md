# Payment processing API

## A user-centric API that processes payment and updates the status of the payment to:
- 'success' (if transaction was successful),
- 'failed' (if the payment did not go through) or
- 'pending'(if the transaction is yet to be processed)


### The endpoint:


`POST /api/v1/PaymentProcessor`

**You pass in to the controller**: a PaymentDTO class that looks like this:

```json
{
  "creditCardNumber": "string",
  "cardHolder": "string",
  "expirationDate": "DateTime",
  "securityCode": "string",
  "amount": "decimal"
}
```
This is processed and depending on what the value of amount is passed in, it gets processed as a cheap, expensive or premium payment.
Also, depending on what happens to the transaction, you get a 200 Ok, 400 BadRequest or 500 InternalServerError
