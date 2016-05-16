using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Win32;

namespace Offline_Repository_generator
{
    
    class Program
    {
       static void Main(string[] args)  
        {
           
            Console.WriteLine("** Offline Repository Generator **\nInput an amount of Repositories");
            int j = int.Parse(Console.ReadLine());
            for (int i = 1; i <= j; i++ )
            {
                Random r = new Random();
                int n1 = r.Next(11111111, 99999000);
                int n2 = i + 1000;
                int n3 = r.Next(1111, 9000);
                int n4 = r.Next(1000, 9000);
                int n5 = r.Next(100000, 999999);
                int n6 = r.Next(100000, 999999);
                Microsoft.Win32.RegistryKey key, FileConfigurations, FileSystemCacheConfiguration, Specification, File, SpecificationFile;
                key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\\AppRecovery\\Core\\Repositories\\" + n1 + "-" + n2 + "-" + n3 + "-" + n4 + "-" + n5 + n6);
                key.SetValue("", "Replay.Core.Contracts.Repositories.RepositoryConfiguration");
                key.SetValue("Id", n1 + "-" + n2 + "-" + n3 + "-" + n4 + "-" + n5 + n6);
                key.SetValue("RepositoryCheckState", "1", RegistryValueKind.DWord);
                key.SetValue("RootRpfsFileId", "4503599627370501", RegistryValueKind.QWord);
                key.SetValue("VerifyRepositoryLocationsInterval", "00:01:00");
                key.SetValue("VolumeId", i+10);
                FileConfigurations = key.CreateSubKey("FileConfigurations");
                FileConfigurations.SetValue("", "System.Collections.Generic.List`1[[Replay.Core.Contracts.Repositories.RepositoryFileConfiguration, Core.Contracts, Version=5.4.3.106, Culture=neutral, PublicKeyToken=null]]");
                FileSystemCacheConfiguration = key.CreateSubKey("FileSystemCacheConfiguration");
                FileSystemCacheConfiguration.SetValue("", "Replay.Core.Contracts.Repositories.FileSystemCacheConfiguration");
                FileSystemCacheConfiguration.SetValue("CacheLevel", "0", RegistryValueKind.DWord);
                FileSystemCacheConfiguration.SetValue("FileSystemCacheDataPath", "C:\\ProgramData\\AppRecovery\\FileSystemCachedData");
                FileSystemCacheConfiguration.SetValue("LastTimeRefresh","0001-01-02T00:00:00");
                FileSystemCacheConfiguration.SetValue("RefreshCachePeriod", "30.00:00:00");
                Specification = key.CreateSubKey("Specification");
                Specification.SetValue("", "Replay.Core.Contracts.Repositories.RepositorySpecification");
                Specification.SetValue("AllocationPolicy", "2", RegistryValueKind.DWord);
                Specification.SetValue("EnableCompression", "1", RegistryValueKind.DWord);
                Specification.SetValue("EnableDedupe", "1", RegistryValueKind.DWord);
                Specification.SetValue("EnableDirectoryAutoRepair", "1", RegistryValueKind.DWord);
                Specification.SetValue("MaxConcurrentOperations", "64", RegistryValueKind.DWord);
                Specification.SetValue("Name", "Repository1"+i);
                File = FileConfigurations.CreateSubKey("0");
                File.SetValue("", "Replay.Core.Contracts.Repositories.RepositoryFileConfiguration");
                File.SetValue("FileSystemDeviceId", "1", RegistryValueKind.DWord);
                File.SetValue("Id", n1 + "-" + n2 + "-" + n3 + "-" + n4 + "-" + n6 + "111111");
                File.SetValue("IsDataPathValid", "0", RegistryValueKind.DWord);
                File.SetValue("IsMetadataPathValid", "0", RegistryValueKind.DWord);
                SpecificationFile = File.CreateSubKey("Specification");
                SpecificationFile.SetValue("", "Replay.Core.Contracts.Repositories.RepositoryFileSpecification");
                SpecificationFile.SetValue("AverageBytesPerRecord", "8192", RegistryValueKind.DWord);
                SpecificationFile.SetValue("BytesPerSector", "512", RegistryValueKind.DWord);
                SpecificationFile.SetValue("CreateOnCifs", "0", RegistryValueKind.DWord);
                SpecificationFile.SetValue("DataPath", "C:\\Repository1"+i);
                SpecificationFile.SetValue("MetadataPath", "C:\\Repository1" + i);
                SpecificationFile.SetValue("Size", "5368709120", RegistryValueKind.QWord);
                SpecificationFile.SetValue("WriteCachingPolicy", "2", RegistryValueKind.DWord);
               // key.Close();
                              
            }
            Console.WriteLine(j+" Offline Repository(ies) were successfully added, please Restart Core Service");
            Console.ReadKey();
         }
         
    }
}
