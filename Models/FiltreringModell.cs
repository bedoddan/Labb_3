using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labb_3.Models
{
    public class FiltreringModell
    {
        public IEnumerable<Fiskar> FiskFiltLista { get; set; }
        public IEnumerable<Personer> PersonFiltLista { get; set; }
        public IEnumerable<Beten> BeteFiltLista { get; set; }
        public IEnumerable<Totalus> TotalusFiltLista { get; set; } 
    }
}
