using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.Modelos
{
    public class Response<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string mensaje { get; set; }
    }
}
