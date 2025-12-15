using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

namespace Farmacia_V_M.R.E.A_
{
    [Table("DetalleFacturas")]
    public class DetalleFactura
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int FacturaId { get; set; }
        public int MedicinaId { get; set; }
        public string NombreMedicina { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}