using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace work.logic
{
    public class Transaction { }
   
    public class Operator : Transaction
    {
        public Operator()
        {
            Console.WriteLine("Op");
        }
    }
    public class SystemNdc : Transaction
    {
        public SystemNdc()
        {
            Console.WriteLine("Sy");
        }
    }
    public class Balance : Transaction
    {
        public Balance()
        {
            Console.WriteLine("Ba");
        }
    }
}
