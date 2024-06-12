//class Program
//{
//    static HashSet<string> uploadedFiles = new HashSet<string>(); 
//    static Queue<string> filesToUpload = new Queue<string>();

//    static void Main()
//    {
//        string folderPath = @"C:\Users\user\Desktop\Upload";

//        if (!Directory.Exists(folderPath))
//        {
//            Directory.CreateDirectory(folderPath);
//            Console.WriteLine("AutoFiles folder created.");
//        }
//        foreach (string filePath in Directory.GetFiles(folderPath))
//        {
//            if (!uploadedFiles.Contains(filePath))
//            {
//                filesToUpload.Enqueue(filePath);
//            }
//        }

//        FileSystemWatcher watcher = new FileSystemWatcher
//        {
//            Path = folderPath,
//            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
//            Filter = "*.*"
//        };

//        watcher.Created += OnChanged;
//        watcher.EnableRaisingEvents = true;

//        Console.WriteLine("Monitoring folder: " + folderPath);
//        Console.WriteLine("Press 'q' to quit the sample.");

//        UploadFilesInQueue();

//        while (Console.Read() != 'q') ;
//    }

//    private static async void OnChanged(object source, FileSystemEventArgs e)
//    {
//        string filePath = e.FullPath;

//        string extension = Path.GetExtension(filePath).ToLower();
//        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
//        {
//            if (!uploadedFiles.Contains(filePath))
//            {
//                await Task.Delay(1000); 

//                if (File.Exists(filePath))
//                {
//                    filesToUpload.Enqueue(filePath);
//                    UploadFilesInQueue(); 
//                }
//            }
//            else
//            {
//                Console.WriteLine($"File already uploaded: {filePath}");
//            }
//        }
//    }
//    private static async void UploadFilesInQueue()
//    {
//        while (filesToUpload.Count > 0)
//        {
//            string filePath = filesToUpload.Dequeue();
//            await UploadFile(filePath);
//        }
//    }

//    private static async Task UploadFile(string filePath)
//    {
//        using (var client = new HttpClient())
//        {
//            try
//            {
//                var form = new MultipartFormDataContent();
//                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
//                {
//                    var fileContent = new StreamContent(fileStream);
//                    form.Add(fileContent, "file", Path.GetFileName(filePath));

//                    HttpResponseMessage response = await client.PostAsync("https://7lqwh9qp-7260.inc1.devtunnels.ms/api/upload/upload", form);
//                    response.EnsureSuccessStatusCode();

//                    Console.WriteLine($"File uploaded successfully: {filePath}");
//                    uploadedFiles.Add(filePath); 
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error uploading file: {ex.Message}");
//                return; 
//            }
//        }

//        await Task.Delay(1000);

//        try
//        {
//            File.Delete(filePath);
//            Console.WriteLine($"File deleted successfully: {filePath}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error deleting file: {ex.Message}");
//        }
//    }

//}







using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static HashSet<string> uploadedFiles = new HashSet<string>();
    static Queue<string> filesToUpload = new Queue<string>();
    static string systemId = GetLocalIPAddress(); // Use the IP address of the system as the systemId

    static void Main()
    {
        string folderPath = @"C:\Users\user\Desktop\Upload";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Console.WriteLine("AutoFiles folder created.");
        }
        foreach (string filePath in Directory.GetFiles(folderPath))
        {
            if (!uploadedFiles.Contains(filePath))
            {
                filesToUpload.Enqueue(filePath);
            }
        }

        FileSystemWatcher watcher = new FileSystemWatcher
        {
            Path = folderPath,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            Filter = "*.*"
        };

        watcher.Created += OnChanged;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Monitoring folder: " + folderPath);
        Console.WriteLine("Press 'q' to quit the sample.");

        UploadFilesInQueue();

        while (Console.Read() != 'q') ;
    }

    private static async void OnChanged(object source, FileSystemEventArgs e)
    {
        string filePath = e.FullPath;

        string extension = Path.GetExtension(filePath).ToLower();
        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
        {
            if (!uploadedFiles.Contains(filePath))
            {
                await Task.Delay(1000);

                if (File.Exists(filePath))
                {
                    filesToUpload.Enqueue(filePath);
                    UploadFilesInQueue();
                }
            }
            else
            {
                Console.WriteLine($"File already uploaded: {filePath}");
            }
        }
    }

    private static async void UploadFilesInQueue()
    {
        while (filesToUpload.Count > 0)
        {
            string filePath = filesToUpload.Dequeue();
            await UploadFile(filePath);
        }
    }

    private static async Task UploadFile(string filePath)
    {
        using (var client = new HttpClient())
        {
            try
            {
                var form = new MultipartFormDataContent();
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var fileContent = new StreamContent(fileStream);
                    form.Add(fileContent, "file", Path.GetFileName(filePath));

                    HttpResponseMessage response = await client.PostAsync($"https://7lqwh9qp-7260.inc1.devtunnels.ms/api/upload/upload?systemId={systemId}", form);
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($"File uploaded successfully: {filePath}");
                    uploadedFiles.Add(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return;
            }
        }

        await Task.Delay(1000);

        try
        {
            File.Delete(filePath);
            Console.WriteLine($"File deleted successfully: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
        }
    }

    private static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
