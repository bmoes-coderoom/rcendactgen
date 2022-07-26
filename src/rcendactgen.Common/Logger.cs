using Newtonsoft.Json;

namespace rcendactgen.Common;
/// <summary>
/// Core Logging class
/// </summary>
public class Logger : ILogger
{
    private static readonly string activityLogDir = $"{Globals.EXE_DIR}/activitylogs";
    private static readonly string absoluteFilePath = $"{activityLogDir}/activitylog_{DateTime.Now.ToString("yyyyMMddHHmmss")}.json";
    public void WriteToLog<T>(T obj)
    {
        try
        {
            // Update json data string
            string jsonData = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(absoluteFilePath, jsonData);
        }
        catch (Exception ex)
        {
            // Attempt to write error to file
            try
            {
                DateTime dt = DateTime.Now;
                string errorFile = $"errors.txt";
                string contents = $"Timestamp {dt}: LOG START\n{ex.StackTrace}\nLOG END";
                System.IO.File.AppendAllText(errorFile, contents);
            }
            catch (Exception errEx)
            {
                // display to console
                Console.WriteLine(errEx.StackTrace);
            }

        }
    }

    public void CreateFile()
    {
        try
        {
            if (!Directory.Exists(activityLogDir))
            {
                Directory.CreateDirectory(activityLogDir);
            }

            if (!File.Exists(absoluteFilePath))
            {
                File.WriteAllText(absoluteFilePath, "");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@$"There was an error creating the file structure for the activity log.
You may experience integrity issues with logging.
Please verify you have proper access rights in the current directory and that there is enough disk space.
The available exception is: {ex.Message}");
        }

    }
}