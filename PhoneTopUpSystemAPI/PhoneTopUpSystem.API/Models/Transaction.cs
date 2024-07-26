using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneTopUpSystem.API;

public class Transaction
{
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [ForeignKey("Beneficiary")]
    public int BeneficiaryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}
