using rcendactgen.Models;

namespace rcendactgen.Business;

public interface IProcessWrapper
{
    ProcessWrapperModel Start(string command, string args);
}