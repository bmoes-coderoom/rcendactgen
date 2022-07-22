namespace rcendactgen.Business;

public interface ILogManager
{
    void WriteLog<T>(T obj);
}