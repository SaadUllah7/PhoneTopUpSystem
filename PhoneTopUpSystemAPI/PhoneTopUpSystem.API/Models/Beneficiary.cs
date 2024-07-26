using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneTopUpSystem.API;

public class Beneficiary
{
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public string Nickname { get; set; }
    public virtual User User { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}
