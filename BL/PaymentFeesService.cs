namespace UATP.BL
{
    public class PaymentFeesService
    {
        private decimal _currentFee;
        private readonly Timer _timer;

        public PaymentFeesService()
        {
            _currentFee = GenerateRandomFee();
            _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        }

        public decimal GetFee()
        {
            return _currentFee;
        }

        private void UpdateFee(object state)
        {
            _currentFee *= GenerateRandomFee();
        }

        private decimal GenerateRandomFee()
        {
            Random random = new Random();
            return (decimal)random.NextDouble() * 2;
        }
    }
}