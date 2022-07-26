using System.Reflection;
using rcendactgen.Common;

namespace rcendactgen.Main;
/// <summary>
/// This class will prepare program data needed for the program to run File modification and deletion scenarios
/// </summary>
public static class DataPrepper
{
    private static string relativePathModify = "ProgramData/modify_file.txt";
    private static string relativePathDelete = "ProgramData/delete_file.txt";
    public static void PrepareProgramData()
    {
        Globals.EXE_DIR = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        try
        {
            File.WriteAllText($"{Globals.EXE_DIR}/{relativePathModify}", "");
            File.WriteAllText($"{Globals.EXE_DIR}/{relativePathDelete}", "");
            UpdateProgramDataGlobals();
        }
        catch (Exception)
        {
            Console.WriteLine(@"There was an error preparing the program's data for file modification and deletion.
You may experience an error when the program attempts to run those scenarios.
Please check that you have proper permissions in the program's directory and restart this program.");
        }

    }

    private static void UpdateProgramDataGlobals()
    {
        Globals.MODIFYFILE_ABSOLUTE_FILEPATH = $"{Globals.EXE_DIR}/{relativePathModify}";
        Globals.DELETEFILE_ABSOLUTE_FILEPATH = $"{Globals.EXE_DIR}/{relativePathDelete}";
    }
}