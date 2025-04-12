namespace HemoVida.Core.Entities;

public class Stock : BaseEntity
{
    public Stock(string typeBlood, string rhFactor, double mlQuantity)
    {
        TypeBlood = typeBlood;
        RhFactor = rhFactor;
        MlQuantity = mlQuantity;
    }

    public string TypeBlood { get; set; }
    public string RhFactor { get; set; }
    public double MlQuantity { get; set; }
}
