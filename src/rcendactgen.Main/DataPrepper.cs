using rcendactgen.Common;

namespace rcendactgen.Main;

public static class DataPrepper
{
    private static string relativePathModify = "ProgramData/modify_file.txt";
    private static string relativePathDelete = "ProgramData/delete_file.txt";
    public static void PrepareProgramData()
    {
        try
        {
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}{relativePathModify}", "");
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}{relativePathDelete}", "");
            UpdateGlobals();
        }
        catch (Exception)
        {
            Console.WriteLine(@"There was an error preparing the program's data for file modification and deletion.
You may experience an error when the program attempts to run those scenarios.
Please check that you have proper permissions in the program's directory and restart this program.");
        }

    }

    private static void UpdateGlobals()
    {
        Globals.MODIFYFILE_ABSOLUTE_FILEPATH = Path.GetFullPath(relativePathModify);
        Globals.DELETEFILE_ABSOLUTE_FILEPATH = Path.GetFullPath(relativePathDelete);
    }
}