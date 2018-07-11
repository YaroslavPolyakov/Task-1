using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CsvHelper;

namespace ConsoleApplication
{
    class Program
    {
        public class Data
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Scription { get; set; }

            public Data(string id, string title, string scription)
            {
                Id = id;
                Title = title;
                Scription = scription;
            }
        }
        public static void ParseCSV()
        {
            Console.Write("Path: ");
            string path = Console.ReadLine();
            using (var reader = new StreamReader(@path))
            {
                List<Data> data = new List<Data>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    data.Add(new Data(values[0], values[1], values[2]));
                }
                Console.WriteLine("Id\tTitle\tScription\t");
                foreach (Data item in data)
                {
                    Console.WriteLine("{0}\t{1}\t{2}",item.Id, item.Title, item.Scription);
                }
            }
        }
        public static void LoadJson()
        {
            string jsonString = "";
            Console.Write("Path: ");
            string path = Console.ReadLine();
            using (StreamReader sr = new StreamReader(@path))
            {
                while (true)
                {
                    string temp = sr.ReadLine();
                    if (temp == null) break;
                    jsonString += "\n" + temp;
                }
            }
            Console.WriteLine(jsonString);

            JArray jsonVal = JArray.Parse(jsonString) as JArray;
            dynamic data = jsonVal;

            foreach (dynamic item in data)
            {
                Console.WriteLine("{0}\n{1}\n{2}\n{3}", item.firstName, item.lastName, item.age, item.address);
                foreach (dynamic cars in item.car)
                {
                    Console.WriteLine("{0}\n{1}\n", cars.model, cars.price);
                }
            }

        }
        public class Monitoring
        {
            //Output all files from subdirectories
            public static void OutputAllFilse()
            {
                Console.Write("Path: ");
                string path = Console.ReadLine();
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (var all in dir.GetFiles("*", SearchOption.AllDirectories))
                {
                    Console.Write("Name: " + all.Name + "\n");
                    Console.WriteLine("Path: " + all.DirectoryName + "\n");
                }
            }
            //Output of a certain format
            public static void OutputNecessaryFormat()
            {
                Console.Write("Path: ");
                string path = Console.ReadLine();
                DirectoryInfo dir = new DirectoryInfo(path);
                Console.Write("Input format(example: .txt): ");
                string format = Console.ReadLine();
                foreach (var all in dir.GetFiles("*" + format, SearchOption.AllDirectories))
                {
                    Console.Write("Name: " + all.Name + "\n");
                    Console.WriteLine("Path: " + all.DirectoryName + "\n");
                }
            }
            //Read the file
            public static void ReadFile()
            {
                string text = "";
                Console.WriteLine("Input path and name file: ");
                string file = Console.ReadLine();
                using (StreamReader fs = new StreamReader(@file))
                {
                    while (true)
                    {
                        string temp = fs.ReadLine();
                        if (temp == null) break;
                        text += "\n" + temp;
                    }
                }
                Console.WriteLine(text);
            }

            public static void Parse()
            {
                //
            }

            //Monitoring directories
            [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
            public static void WatchDirectory()
            {
                //Console.WriteLine("Input path to watch the directory: ");
                //string path = Console.ReadLine();
                Console.Write("Input number of directories: ");
                int number = int.Parse(Console.ReadLine());
                List<string> list = new List<string>();
                Console.WriteLine("Input path to watch the directory: ");
                for (int i = 1; i <= number; i++)
                {
                    list.Add(Console.ReadLine());
                }
                foreach (string dir in list)
                {
                    Watch(dir);
                }
                // Wait for the user to quit the program.
                Console.WriteLine("Press \'q\' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
            private static void Watch(string watch_folder)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = watch_folder;
                // Watch for changes 
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //Watch all files
                watcher.Filter = "*";
                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);
                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
            private static void OnChanged(object source, FileSystemEventArgs e)
            {
                // Specify what is done when a file is changed, created, or deleted.
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            }
            private static void OnRenamed(object source, RenamedEventArgs e)
            {
                // Specify what is done when a file is renamed.
                Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("1.Output all files from subdirectories\n2.Output of a certain format\n3.Read the file\n4.Monitoring directories\n5.Parse json file\n6.Parse csv file\n\n");
                string number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        Monitoring.OutputAllFilse();
                        break;
                    case "2":
                        Monitoring.OutputNecessaryFormat();
                        break;
                    case "3":
                        Monitoring.ReadFile();
                        break;
                    case "4":
                        Monitoring.WatchDirectory();
                        break;
                    case "5":
                        LoadJson();
                        break;
                    case "6":
                        ParseCSV();
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
        }
    }
}