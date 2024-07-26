using System.ComponentModel.DataAnnotations;

namespace PhoneTopUpSystem.API;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public bool IsVerified { get; set; }
    public ICollection<Beneficiary> Beneficiaries { get; set; }

}