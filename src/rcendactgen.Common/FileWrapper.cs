namespace rcendactgen.Common;

public class FileWrapper : IFileWrapper
{
    public FileWrapObj Create(string path)
    {
        FileInfo file = new FileInfo(path);
        // Creates directory if does not exist
        file.Directory.Create();
        File.Create(path);
        return new FileWrapObj
        {
            AbsoluteFilePath = file.FullName
        
        };
    }

    public void WriteAllText(string path, string content)
    {
        File.WriteAllText(path,content);
    }

    public void Delete(string path)
    {
        File.Delete(path);
    }
}