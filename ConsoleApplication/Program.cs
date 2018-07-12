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
            public static void ParseCSV(string path)
            {
                try
                {
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
                            Console.WriteLine("{0}\t{1}\t{2}", item.Id, item.Title, item.Scription);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            public static void ParseJson(string path)
            {
                try
                {
                    string jsonString = "";
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
                catch (Exception e)
                {
                    Console.WriteLine("Error: "+e.Message);
                }
    }
        }
        public class Monitoring
        {
            //Output all files from subdirectories
            public static void OutputAllFilse(string path)
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    foreach (var all in dir.GetFiles("*", SearchOption.AllDirectories))
                    {
                        Console.Write("Name: " + all.Name + "\n");
                        Console.WriteLine("Path: " + all.DirectoryName + "\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            //Output of a certain format
            public static void OutputNecessaryFormat(string path, string format)
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    foreach (var all in dir.GetFiles("*" + format, SearchOption.AllDirectories))
                    {
                        Console.Write("Name: " + all.Name + "\n");
                        Console.WriteLine("Path: " + all.DirectoryName + "\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            //Read the file
            public static void ReadFile(string path)
            {
                try
                {
                    string text = "";
                    using (StreamReader fs = new StreamReader(@path))
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
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            //Monitoring directories
            [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
            public static void WatchDirectory(List<string> list)
            {
                try
                {
                    foreach (string dir in list)
                    {
                        Watch(dir);
                    }
                    // Wait for the user to quit the program.
                    Console.WriteLine("Press \'q\' to quit the sample.");
                    while (Console.Read() != 'q') ;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            public static void Watch(string watch_folder)
            {
                try
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
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            public static void OnChanged(object source, FileSystemEventArgs e)
            {
                // Specify what is done when a file is changed, created, or deleted.
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            }
            public static void OnRenamed(object source, RenamedEventArgs e)
            {
                // Specify what is done when a file is renamed.
                Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            }
        }
        static void Main(string[] args)
        {
            string path;
            while (true)
            {
                Console.Write("1.Output all files from subdirectories\n2.Output of a certain format\n3.Read the file\n4.Monitoring directories\n5.Parse json file\n6.Parse csv file\n0.Exit \n\n");
                string number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        {
                            Console.Write("Path: ");
                            path = Console.ReadLine();
                            Monitoring.OutputAllFilse(path);
                            break;
                        }
                    case "2":
                        {
                            Console.Write("Path: ");
                            path = Console.ReadLine();
                            Console.Write("Input format(example: .txt): ");
                            string format = Console.ReadLine();
                            Monitoring.OutputNecessaryFormat(path, format);
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("Input path and name file: ");
                            path = Console.ReadLine();
                            Monitoring.ReadFile(path);
                            break;
                        }
                    case "4":
                        {
                            Console.Write("Input count of directories: ");
                            int count = int.Parse(Console.ReadLine());
                            List<string> list = new List<string>();
                            Console.WriteLine("Input path to watch the directory: ");
                            for (int i = 1; i <= count; i++)
                            {
                                list.Add(Console.ReadLine());
                            }
                            Monitoring.WatchDirectory(list);
                            break;
                        }
                    case "5":
                        {
                            Console.Write("Path: ");
                            path = Console.ReadLine();
                            Data.ParseJson(path);
                        }
                        break;
                    case "6":
                        {
                            Console.Write("Path: ");
                            path = Console.ReadLine();
                            Data.ParseCSV(path);
                        }
                        break;
                    case "0":
                        {
                            Environment.Exit(0);
                        }
                        break;
                    default:
                        Console.WriteLine(";(");
                        break;
                }
            }
        }
    }
}