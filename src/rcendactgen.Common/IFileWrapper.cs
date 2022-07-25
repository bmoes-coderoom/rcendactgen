namespace rcendactgen.Common;

public interface IFileWrapper
{
    FileWrapObj Create(string path);
    void WriteAllText(string path, string content);
    void Delete(string path);
}