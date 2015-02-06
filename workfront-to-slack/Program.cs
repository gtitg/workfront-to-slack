using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workfront_to_slack
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = ConfigurationManager.AppSettings["WorkFront_Username"];

            Console.WriteLine("workfront username: " + test);
        }
    }
}
