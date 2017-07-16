using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

using Data = Google.Apis.Sheets.v4.Data;

namespace PatchManagerService
{
    //Additionally, you can choose if you want to overwrite existing data after a table or insert new rows for the new data.By default, the input overwrites data after the table.
    //To write the new data into new rows, specify insertDataOption = INSERT_ROWS

    // good article to read http://www.thecybria.com/blog/insert-new-row-into-google-sheet-using-google-apis-sheets-v4-services/
    class GoogleSheet
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets }; // static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static readonly string ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
        static readonly string SheetId = ConfigurationManager.AppSettings["GoogleSheetId"];
        public static int docStartPoint = 1;
        private int _TimeDelayOnCheckNextPatch;

        public GoogleSheet(int TimeDelayOnCheckNextPatch = 10000)
        {
            _TimeDelayOnCheckNextPatch = TimeDelayOnCheckNextPatch;
        }

        public void GetLatestPatchFromXmlAndUpdateGoogleDoc(Object source, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                var sheetService = GoogleSheet.GetGoogleService();
                var latestPatchFromDoc = Convert.ToInt32(GoogleSheet.CheckLatestPatchNumberFromDoc(sheetService));
                Console.WriteLine("latest patch from doc is : " + latestPatchFromDoc);
                Thread.Sleep(1000);

                for (int i = 1; i < 4; i++)
                {
                    var patch = PatchInfo.GetPatchInfo(latestPatchFromDoc + i);
                    Console.WriteLine("Trying to find newer patch info " + (latestPatchFromDoc + i));
                    Thread.Sleep(1000);

                    if (patch.NameFromXml != null)
                    {
                        Console.WriteLine("Found Patch info : " + patch.NameFromXml + "\n adding it to google sheet document.");

                        GoogleSheet.UpdateTopCellWithPatchInfo(sheetService, patch);

                        Console.WriteLine("Patch added successfully");
                        Thread.Sleep(1000);

                        break;
                    }
                }
                Console.WriteLine("Waiting for " + _TimeDelayOnCheckNextPatch / 1000 + " seconds to check again." + "\n " + e.SignalTime);
            }

        }

        public static SheetsService GetGoogleService()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        public static void CreateNewLineOnTop(SheetsService service)
        {
            var requestBody = new Request()
            {
                InsertDimension = new InsertDimensionRequest()
                {
                    Range = new DimensionRange()
                    {
                        SheetId = 0,
                        Dimension = "ROWS",
                        StartIndex = docStartPoint,
                        EndIndex = docStartPoint + 1
                    }
                }
            };
            var requestContainer = new List<Request>();
            requestContainer.Add(requestBody);

            var request = new BatchUpdateSpreadsheetRequest { Requests = requestContainer };
            var request2 = new SpreadsheetsResource.BatchUpdateRequest(service, request, SheetId);

            request2.Execute();

            Console.WriteLine("done!");
        }

        public static string CheckLatestPatchNumberFromDoc(SheetsService service)
        {
            var patchPattern = new Regex(@"P-\d{4}");
            for (int i = 1; i < 10; i++)
            {
                var range = "A" + i;
                SpreadsheetsResource.ValuesResource.GetRequest getRequest = service.Spreadsheets.Values.Get(SheetId, range);
                ValueRange getResponse = getRequest.Execute();
                IList<IList<Object>> getValues = getResponse.Values;
                var cellvalue = getValues?[0].ToArray()[0].ToString() ?? string.Empty;
                if (patchPattern.IsMatch(cellvalue))
                {
                    docStartPoint = i - 1;
                    return cellvalue.Split("-".ToCharArray())[1];
                }
            }

            throw new ArgumentException("Could not find latest patch after check first 10 rows.");
        }

        public static void UpdateTopCellWithPatchInfo(SheetsService service, PatchInfo pathInfo)
        {
            CreateNewLineOnTop(service);
            ValueRange valueRange = new ValueRange();
            var oblist = new List<object>()
            {
                pathInfo.NameForDoc,
                pathInfo.Description,
                pathInfo.ProductVersion,
                pathInfo.PatchType,
                pathInfo.Authors,
                pathInfo.Zendesk,
                pathInfo.DefectIds,
                pathInfo.AdditionalInfo,
                DateTime.Now.ToShortDateString(), //pathInfo.CreationDate,
                pathInfo.DownloadLink
            };
            valueRange.Values = new List<IList<object>> { oblist };
            valueRange.MajorDimension = "ROWS";//"ROWS";//COLUMNS

            

            String range = "History!A" + (docStartPoint + 1) + ":J" + (docStartPoint + 1);  // update cell F5 

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = service.Spreadsheets.Values.Update(valueRange, SheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            updateRequest.Execute();
        }
    }
}
