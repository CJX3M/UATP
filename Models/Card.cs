namespace UATP.Models
{
    public class Card
    {
        public Card(string cardNumber)
        {
            CardNumber = cardNumber;
        }

        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
    }
}