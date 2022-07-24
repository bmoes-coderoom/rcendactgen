namespace rcendactgen.Common;

public interface IFileWrapper
{
    FileStream Create(string path);
    void WriteAllText(string path, string content);
    void Delete(string path);
}