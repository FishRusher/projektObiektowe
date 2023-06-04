using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBaza
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Cost { get; set; }
        public int Tax { get; set; }
        public string Description { get; set; }
        public string ImageName{ get; set; }
    }
}
