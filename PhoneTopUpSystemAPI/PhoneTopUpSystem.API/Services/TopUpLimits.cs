namespace PhoneTopUpSystem.API;

public static class TopUpLimits{
    public static readonly decimal topUpLimitPerUserPerMonth = 3000;
    public static readonly decimal verifiedBeneficiaryTopUpLimitPerMonth = 1000;
    public static readonly decimal unVerifiedBeneficiaryTopUpLimitPerMonth = 500;
}
