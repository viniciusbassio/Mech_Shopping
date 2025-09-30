using MechShopping.ProductAPI.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MechShopping.ProductAPI.Model
{
    [Table("Produto")]
    public class Product: BaseEntity
    {
        [Column("Nome_Produto")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("Preco")]
        [Required]
        [Range(1,20000)]
        public decimal Price {  get; set; }

        [Column("Descricao")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Column("Nome_Categoria")]
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [Column("imagem_url")]
        [StringLength(300)]
        public string? ImageURL { get; set; }

    }
}
