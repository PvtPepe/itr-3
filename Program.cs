using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace itr_3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckParams(args)) return;
            int compInd, plInd;
            var rng = new RNGCryptoServiceProvider();
            var rnd = new Random();
            byte[] key = new byte[16];
            compInd = rnd.Next(args.Length);
            rng.GetBytes(key);
            string strKey = ToStringX(key);
            ComputeHash(System.Text.Encoding.UTF8.GetBytes(args[compInd]), System.Text.Encoding.UTF8.GetBytes(strKey));
            plInd = ShowMenu(args);
            if (plInd == 0) return;
            plInd--;
            Console.WriteLine($"Your move: {args[plInd]}");
            Console.WriteLine($"Computer move: {args[compInd]}");
            ChooseWinner(compInd, plInd, args.Length);
            Console.WriteLine($"HMAC key: {strKey}");
            Console.ReadLine();
        }

        static bool CheckParams(string[] a)
        {
            bool i = false;
            if (a.Length < 3)
            {
                Console.WriteLine("There must be at least 3 params. Example: rock paper scissors");
                i = true;
            }
            if (a.Length % 2 == 0)
            {
                Console.WriteLine("Params amount should be uneven. Example: rock paper scissors");
                i = true;
            }
            if (SearchDuplicates(a))
            {
                Console.WriteLine("Params contain duplicates. Example: rock paper scissors");
                i = true;
            }
            return i;
        }

        static bool SearchDuplicates(string[] n)
        {
            var set = new HashSet<string>();
            foreach (var item in n)
                if (!set.Add(item))
                    return true;
            return false;
        }

        static void ComputeHash(byte[] s, byte[] key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                byte[] a = hmac.ComputeHash(s);
                Console.WriteLine($"HMAC: {ToStringX(a)}");
            }
        }

        static string ToStringX(byte[] a) 
        {
            return string.Join("", a.Select(b => b.ToString("X2"))); 
        }

        static int ShowMenu(string[] a)
        {
            int res = 0;
            while (true)
            {
                for (int i = 0; i < a.Length; i++)
                    Console.WriteLine($"{i + 1}) {a[i]}");
                Console.WriteLine("0) Exit");
                Console.Write("Enter your move: ");
                if (int.TryParse(Console.ReadLine(), out res) && res <= a.Length && res >= 0) return res;
            }
        }

        static void ChooseWinner(int compInd, int plInd, int len)
        {
            int diff = plInd - compInd;
            int half = (len - 1) / 2;
            if (plInd == compInd) Console.WriteLine("Draw!");
            else 
            {
                if ((diff < 0 && diff >= -half) || (diff > 0 && diff > half && -diff < -half))
                    Console.WriteLine("You lose!");
                else
                    Console.WriteLine("You win!");
            }
        }
    }
}

