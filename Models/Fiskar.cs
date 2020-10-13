using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labb_3.Models
{
    public class Fiskar
    {
        //Konstruktor
        public Fiskar() { }

        //Publica egenskaper
        public int ID { get; set; }
        public string Art { get; set; }
        public int Vikt { get; set; }
        public string Vatten { get; set; }
        public long Persnr { get; set; }
        public int Betenr { get; set; }



    }
}
