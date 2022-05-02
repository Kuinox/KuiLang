using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KuiLang
{
    public static class StackExtension
    {
        public static Stack<T> Add<T>(this Stack<T> @this, T element)
        {
            @this.Push(element);
            return @this;
        }

        public static List<T> Plus<T>(this List<T> @this, T element)
        {
            @this.Add(element);
            return @this;
        }
    }
}
