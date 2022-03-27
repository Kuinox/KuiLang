using KuiLang.Build;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class Interpeter
    {
        public static int Run(ICollection<BuiltType> programTypes)
        {
            var programType = programTypes.SingleOrDefault(s => s.FullName == "Program");
            if (programType == null)
            {
                Console.Error.WriteLine("The Assembly does not contain a 'Program' type.");
                return -1;
            }
            BuiltMethod? method = programType.Methods["Main"];
            if (method == null)
            {
                Console.Error.WriteLine("The type Program does not contain a Main method.");
                return -1;
            }
            if (!method.IsStatic)
            {
                Console.Error.WriteLine("The Main Method must be static.");
                return -1;
            }
        }


        public static void ExecuteStaticMethod(BuiltMethod method, object[] arguments)
        {
            foreach (BuiltStatement statement in method.Statements)
            {
                
            }
        }

    }
}
