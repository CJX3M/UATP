namespace UATP.BL
{
    public sealed class PaymentFeeCalculator
    {
        private readonly PaymentFeesService _paymentFeesService;
        private readonly ReaderWriterLockSlim _lock;
        private static readonly Lazy<PaymentFeeCalculator> _instance = new Lazy<PaymentFeeCalculator>(() => new PaymentFeeCalculator());

        public static PaymentFeeCalculator Instance => _instance.Value;

        private PaymentFeeCalculator()
        {
            _paymentFeesService = new PaymentFeesService();
            _lock = new ReaderWriterLockSlim();
        }

        public decimal CalculateFee(decimal amount)
        {
            decimal fee;
            try
            {
                _lock.EnterReadLock();
                fee = amount * _paymentFeesService.GetFee();
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return Math.Round(fee, 2);
        }

        public void UpdateFee()
        {
            try
            {
                _lock.EnterWriteLock();
                _paymentFeesService.UpdateFee();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

}