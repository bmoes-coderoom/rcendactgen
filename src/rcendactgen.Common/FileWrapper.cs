namespace rcendactgen.Common;

public class FileWrapper
{
    public FileStream Create(string path)
    {
        return File.Create(path);
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