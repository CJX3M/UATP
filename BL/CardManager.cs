using UATP.Models;

namespace UATP.BL
{
    public class CardManager
    {
        private readonly List<Card> _cards;
        private readonly object _lock = new object();

        public CardManager()
        {
            _cards = new List<Card>();
        }

        public Card CreateCard()
        {
            Card card;

            lock (_lock)
            {
                // Generate a new card number and check if it's unique
                string cardNumber;
                do
                {
                    cardNumber = GenerateCardNumber();
                } while (_cards.Any(c => c.CardNumber == cardNumber));

                // Create a new card and add it to the list
                card = new Card(cardNumber);
                _cards.Add(card);
            }

            return card;
        }

        public decimal Pay(string cardNumber, decimal amount)
        {
            decimal remainingBalance;

            lock (_lock)
            {
                // Find the card with the given number and update its balance
                Card card = _cards.FirstOrDefault(c => c.CardNumber == cardNumber);
                if (card == null)
                {
                    throw new ArgumentException("Invalid card number.");
                }
                card.Balance -= amount;
                remainingBalance = card.Balance;
            }

            return remainingBalance;
        }

        public decimal GetBalance(string cardNumber)
        {
            decimal balance;

            lock (_lock)
            {
                // Find the card with the given number and return its balance
                Card card = _cards.FirstOrDefault(c => c.CardNumber == cardNumber);
                if (card == null)
                {
                    throw new ArgumentException("Invalid card number.");
                }
                balance = card.Balance;
            }

            return balance;
        }

        private string GenerateCardNumber()
        {
            // Generate a 15-digit random number
            Random random = new Random();            

            return $"{random.Next(99999).ToString().PadLeft(5, '0')}{random.Next(99999).ToString().PadLeft(5, '0')}{random.Next(99999).ToString().PadLeft(5, '0')}";
        }
    }

}