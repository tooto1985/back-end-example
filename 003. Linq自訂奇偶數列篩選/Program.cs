using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication15
{
    class Program
    {
        static void Main(string[] args)
        {
            var co =  new List<CustomObject>
            {
                new CustomObject{ Name = "a1" , Value = "1a"},
                new CustomObject{ Name = "b2" , Value = "2b"},
                new CustomObject{ Name = "c3" , Value = "3c"},
                new CustomObject{ Name = "d4" , Value = "4d"},
                new CustomObject{ Name = "e5" , Value = "5e"}
            };
            
            foreach (var a in co.Odd(x => true).ToList())
            {
                Console.WriteLine(a.Name + "/" + a.Value);
            }
            Console.Read();
        }
    }
    
    public static class Extension
    {
        public static IEnumerable<TSource> Odd<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var row = from bl in source select predicate(bl);
            if (row.Count(x => x == false) > 0)
            {
                return source;
            }
            return source.Where((a, i) => (i+1)%2 != 0);
        }

        public static IEnumerable<TSource> Even<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var row = from bl in source select predicate(bl);
            if (row.Count(x => x == false) > 0)
            {
                return source;
            }
            return source.Where((a, i) => (i+1)%2 == 0);
        }
    }

    public class CustomObject
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}

