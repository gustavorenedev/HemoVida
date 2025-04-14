using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_Stock")]
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
