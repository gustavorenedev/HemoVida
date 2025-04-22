using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_stock")]
public class Stock : BaseEntity
{
    public string BloodType { get; set; }
    public string RhFactor { get; set; }
    public double MlQuantity { get; set; }
}
