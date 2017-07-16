using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;

namespace PatchManagerService
{

    [Table("PatchInfo")]
    public class PatchInfo
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string NameFromXml { get; set; }
        public string NameForDoc { get; set; }
        public string Description { get; set; }
        public string ProductVersion { get; set; }
        public string PatchType { get; set; }
        public string Authors { get; set; }
        public string Zendesk { get; set; }
        public string DefectIds { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime CreationDate { get; set; }
        public string DownloadLink { get; set; }
        public string ModifiedFiles { get; set; }
        public string Platform { get; set; }
        public string TargetProductCode { get; set; }
        public string TargetProductName { get; set; }

        private static readonly string XmlUrl = ConfigurationManager.AppSettings["XmlUrl"];

        public static PatchInfo GetPatchInfo(int patchNumber)
        {
            var patchInfo = new PatchInfo();

            var urlMain = XmlUrl + "P-";
            var xml = ".xml";
            var urlCore = urlMain + patchNumber + "/P-" + patchNumber + xml;
            var urlAgent64 = urlMain + "A64-" + patchNumber + "/P-A64-" + patchNumber + xml;
            var urlAgent86 = urlMain + "A86-" + patchNumber + "/P-A86-" + patchNumber + xml;
            var urlMcmp = urlMain + "M64-" + patchNumber + "/P-M64-" + patchNumber + xml;
            var urlLmu64 = urlMain + "L64-" + patchNumber + "/P-L64-" + patchNumber + xml;
            var urlLmu86 = urlMain + "L86-" + patchNumber + "/P-L86-" + patchNumber + xml;

            var urlCollection = new List<string>() { urlCore, urlAgent64, urlAgent86, urlMcmp, urlLmu64, urlLmu86 };

            bool patchAlreadyFound = false;

            foreach (var url in urlCollection)
            {
                try
                {
                    var xmlPatch = new XmlDocument();
                    xmlPatch.Load(url);

                    if (patchAlreadyFound)
                    {
                        patchInfo.DownloadLink += "\n" + url;
                    }

                    if (!patchAlreadyFound)
                    {
                        patchAlreadyFound = true;
                        patchInfo.NameForDoc = "P-" + patchNumber;
                        patchInfo.NameFromXml = xmlPatch.GetElementsByTagName("d2p1:name").Item(0)?.InnerText;
                        patchInfo.DefectIds = string.Join(",",
                            xmlPatch.GetElementsByTagName("d2p1:defectIds").Cast<XmlNode>()
                                .Select(node => node.ChildNodes)
                                .SingleOrDefault().Cast<XmlNode>()
                                .Select(node => node.InnerText).ToList());
                        patchInfo.ModifiedFiles = string.Join(",",
                            xmlPatch.GetElementsByTagName("d2p1:modifiedFiles").Cast<XmlNode>()
                                .Select(node => node.ChildNodes)
                                .SingleOrDefault().Cast<XmlNode>()
                                .Select(node => node.InnerText).ToList());
                        patchInfo.Description = xmlPatch.GetElementsByTagName("d2p1:description").Item(0)?.InnerText;
                        patchInfo.PatchType = xmlPatch.GetElementsByTagName("d2p1:patchType").Item(0)?.InnerText;
                        patchInfo.Platform = xmlPatch.GetElementsByTagName("d2p1:platform").Item(0)?.InnerText;
                        patchInfo.ProductVersion =
                            xmlPatch.GetElementsByTagName("d2p1:productVersion").Item(0)?.InnerText;
                        patchInfo.TargetProductCode =
                            xmlPatch.GetElementsByTagName("d2p1:targetProductCode").Item(0)?.InnerText;
                        patchInfo.TargetProductName =
                            xmlPatch.GetElementsByTagName("d2p1:targetProductName").Item(0)?.InnerText;
                        patchInfo.Zendesk = xmlPatch.GetElementsByTagName("zendesk").Item(0)?.InnerText;
                        patchInfo.DownloadLink = url.Replace(".xml", ".msi");
                    }
                }

                catch (XmlException)
                {
                    Console.Write("item does not exist");
                }
                catch (WebException)
                {
                    Console.Write("patch does not exist");
                }

                Console.WriteLine(patchInfo.NameFromXml);
            }

            return patchInfo;
        }

        public static int GetPatchNumber(PatchInfo patchInfo)
        {
            var name = patchInfo.NameFromXml;
            var number = name.Split(" - ".ToCharArray())[1];
            return Convert.ToInt32(number);
        }
    }
}
