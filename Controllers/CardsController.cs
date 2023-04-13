using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UATP.BL;
using UATP.Models;

namespace UATP
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly CardManager _cardManager;
        private readonly PaymentFeeCalculator _paymentFeeCalculator;

        public CardsController()
        {
            // Create a new instance of the CardManager and PaymentFeeCalculator classes
            _cardManager = new CardManager();
            _paymentFeeCalculator = PaymentFeeCalculator.Instance;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Card> CreateCard()
        {
            // Create a new card and return it
            Card card = _cardManager.CreateCard();

            return card;
        }

        [HttpPost("{cardNumber}/pay")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<decimal> Pay(string cardNumber, decimal amount)
        {
            // Calculate the payment fee
            decimal fee = _paymentFeeCalculator.CalculateFee(amount);

            // Process the payment and return the remaining balance
            decimal remainingBalance = _cardManager.Pay(cardNumber, amount + fee);

            return remainingBalance;
        }

        [HttpGet("{cardNumber}/balance")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<decimal> GetBalance(string cardNumber)
        {
            // Get the card balance and return it
            decimal balance = _cardManager.GetBalance(cardNumber);

            return balance;
        }
    }
}