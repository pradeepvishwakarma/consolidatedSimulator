using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class DataSource
    {
        public Hashtable dataSources { get; set; }

        public DataSource()
        {
            this.dataSources=new Hashtable();
        }
    }
}
