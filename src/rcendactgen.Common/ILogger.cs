namespace rcendactgen.Common;

public interface ILogger
{
    void WriteToLog<T>(T obj);
    void CreateFile();
}