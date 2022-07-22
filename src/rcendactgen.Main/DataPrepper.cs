using rcendactgen.Common;

namespace rcendactgen.Main;

public static class DataPrepper
{
    private static string relativePathModify = "ProgramData/modify_file.txt";
    private static string relativePathDelete = "ProgramData/delete_file.txt";
    public static void PrepareProgramData()
    {
        File.WriteAllText(relativePathModify, "");
        File.WriteAllText(relativePathDelete, "");
        UpdateGlobals();
    }

    private static void UpdateGlobals()
    {
        Globals.MODIFYFILE_ABSOLUTE_FILEPATH = Path.GetFullPath(relativePathModify);
        Globals.DELETEFILE_ABSOLUTE_FILEPATH = Path.GetFullPath(relativePathDelete);
    }
}