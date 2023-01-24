using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work.logic
{
    public class Part
    {
        public string DateTime { get; set; }
        public string Number { get; set; }
        public PartType Type = PartType.Other;
        public Dictionary<string, string> lines = new Dictionary<string, string>();
        public List<string> Other = new List<string>();
        public bool Important = false;

        public enum PartType
        {
            Dispense,
            Deposite,
            Other
        }
    }
}
