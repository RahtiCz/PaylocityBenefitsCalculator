namespace Api.Models
{
    public class Paycheck
    {
       public decimal Earnings { get; set; }
       public decimal Deductions { get; set; }
       public decimal NetPay { get; set; }
    }
}
