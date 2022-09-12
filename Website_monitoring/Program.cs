using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace Website_monitoring
{
    internal class Program
    {
        public static List<string> websites = new List<string>();
        public static Ping ping = new Ping();
        

        static void Main(string[] args)
        {
            //a várakozási idő a kérések között ms-ban
            int timePeriod = 3000;
            //túl lassú válasz, a teszt kedvéért 20ms-re állítva
            int tooSlow = 40;

            //a listában szereplő url-ek beolvasása
            Beolvas();

            while (true)
            {
                //egy pingelés minden oldalra a listából, és ezek eredményeinek kezelése
                Ping(websites, tooSlow);
                Thread.Sleep(timePeriod);
            }
            

        }

        private static async void Ping(List<string> websites,int tooSlow)
        {
            
                foreach (var w in websites)
                {
                
                PingReply result = await ping.SendPingAsync(w);
                if (result.Status.ToString() != "Success")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("RESPONT STATUS IS NOT SUCCES");
                    Console.WriteLine($"{w} {result.Address} {result.RoundtripTime}ms {result.Status}");
                    Console.ForegroundColor = ConsoleColor.White;
                    using (StreamWriter sw = File.AppendText(@"..\..\resources\results.csv"))
                    {
                        sw.WriteLine("RESPONT STATUS IS NOT SUCCES");
                        sw.WriteLine($"{w};{result.Address};{result.RoundtripTime};{result.Status}");
                    }
                }
                else if (result.RoundtripTime > tooSlow)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("TOO SLOW RESPOND");
                    Console.WriteLine($"{w} {result.Address} {result.RoundtripTime}ms {result.Status}");
                    Console.ForegroundColor = ConsoleColor.White;
                    using (StreamWriter sw = File.AppendText(@"..\..\resources\results.csv"))
                    {
                        sw.WriteLine("TOO SLOW RESPOND");
                        sw.WriteLine($"{w};{result.Address};{result.RoundtripTime};{result.Status}");
                    }
                }
                else
                {
                    Console.WriteLine($"{w} {result.Address} {result.RoundtripTime}ms {result.Status}");
                    using (StreamWriter sw = File.AppendText(@"..\..\resources\results.csv"))
                    {
                        sw.WriteLine($"{w};{result.Address};{result.RoundtripTime};{result.Status}");
                    }
                }

                }


        }

        

        private static void Beolvas()
        {
            using(var sr = new StreamReader(@"..\..\resources\websites.csv", Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    websites.Add(sr.ReadLine());
                }
            }

            //beolvasás teszt
            //foreach (var w in websites)
            //{
            //    Console.WriteLine(w);
            //}
        }
    }
}
