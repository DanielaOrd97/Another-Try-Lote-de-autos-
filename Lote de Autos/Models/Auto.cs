using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoteAutos.Models
{
    public class Auto
    {
        //Imagen
        public int Id { get; set; }
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string? Version { get; set; }
        public ushort Año { get; set; }
        public decimal Precio { get; set; }
        public int Kilometraje { get; set; }
        public string? Motor {  get; set; }
        public string Transmision { get; set; } = null!;
        public string? Carroceria { get; set; }
        public string? Descripcion { get; set; }
        public string Imagen { get; set; } = null!;
    }
}
