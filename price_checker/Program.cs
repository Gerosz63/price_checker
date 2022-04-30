using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Timers;

namespace price_checker
{
    internal class Program
    {
        static System.Timers.Timer aTimer;
        static System.Timers.Timer Beee;
        static System.Timers.Timer countdown;
        static TimeSpan timerd = new TimeSpan(1, 0, 0);
        static void Main(string[] args)
        {
            
            aTimer = new System.Timers.Timer(6 * 1000); //one hour in milliseconds
            Beee = new System.Timers.Timer(1000*5);
            countdown = new System.Timers.Timer(1000);
            Beee.Elapsed += new ElapsedEventHandler(OnTimedEventbee);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            countdown.Elapsed += new ElapsedEventHandler(OnTimedEventdown);
            aTimer.Start(); 
            countdown.Start();
            while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
        private static void OnTimedEventdown(object source, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Sleeping...");
            Console.WriteLine(timerd);
            timerd-= TimeSpan.FromSeconds(1);
        }
        private static void OnTimedEventbee(object source, ElapsedEventArgs e)
        {
            Console.Beep(1000, 1000);
            Console.Beep(4000, 1000);
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            countdown.Stop();
            Console.Clear();
            Console.WriteLine("Loading data...");
            string Url = "https://www.cdkeys.com/pc/astroneer-pc-steam";
            CookieContainer cookieJar = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.CookieContainer = cookieJar;
            request.Accept = @"text/html, application/xhtml+xml, */*";
            request.Referer = @"https://www.cdkeys.com/pc/astroneer-pc-steam";
            request.Headers.Add("Accept-Language", "en-GB");
            request.UserAgent = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";
            request.Host = @"www.cdkeys.com";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string htmlString;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                htmlString = reader.ReadToEnd();
            }
            Console.WriteLine("Formatting data...");
            string[] list = htmlString.Split('\n');
            string price = list.First(x => x.Contains("\"price\": \"")).Trim().Replace("\"price\": \"", "").Replace(",", "").Replace("\"", "").Replace(".", ",");
            double price2 = Convert.ToDouble(price);
            if (price2 < 10000)
            {
                Console.WriteLine("Better price found!");
                Console.WriteLine(price2);
                Beee.Start(); 
                aTimer.Stop();
                Console.ReadKey();
                aTimer.Start();
                Beee.Stop();
            }
            else
            {
                Console.WriteLine("The price is not decreased!");
            }
            Console.WriteLine("Prepearing for sleeping..");
            Thread.Sleep(1000);
            timerd = new TimeSpan(1, 0, 0);
            countdown.Start();
        }
    }
}
