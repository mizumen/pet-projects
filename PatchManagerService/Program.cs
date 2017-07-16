using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;
//using MySql.Data.MySqlClient;

// ReSharper disable AssignNullToNotNullAttribute

namespace PatchManagerService
{
    class Program
    {
        private static System.Timers.Timer aTimer;
        public static readonly int TimeDelayOnCheckNextPatch = int.Parse(ConfigurationManager.AppSettings["TimeDelayOnCheckNextPatch"]);

        static void Main(string[] args)
       {

           //UpdateGoogleDocManually();

           UpdateGoogleDocContinuously();


        }

        static void UpdateGoogleDocContinuously()
        {
            var google = new GoogleSheet(TimeDelayOnCheckNextPatch);

            aTimer = new System.Timers.Timer();
            aTimer.Interval = TimeDelayOnCheckNextPatch;

            aTimer.Elapsed += google.GetLatestPatchFromXmlAndUpdateGoogleDoc;//google.GetLatestPatchFromXmlAndUpdateGoogleDoc;

            aTimer.AutoReset = true;

            aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program at any time... ");
            Console.ReadLine();
        }

        static void UpdateGoogleDocManually() //method to check functionality, puts specified patch on top, so most likely patch order will be incorrect
        {
            var sheetService = GoogleSheet.GetGoogleService();
            GoogleSheet.UpdateTopCellWithPatchInfo(sheetService, PatchInfo.GetPatchInfo(1985));

        }

    }
}
