﻿using rcendactgen.Business;
using rcendactgen.Common;
using rcendactgen.Main;

DataPrepper.PrepareProgramData();

Globals.ACTIVITYLOG_DIRECTORY = "activitylogs";
Globals.ACTIVITYLOG_ABSOLUTE_FILEPATH = $"{Globals.ACTIVITYLOG_DIRECTORY}/activitylog_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.json";
var processManager = new ProcessManager();
var fileManager = new FileManager();
var networkManager = new NetworkManager();

string input = null;
Console.WriteLine("Enter Full Path for Proc Executable with optional arguments");
input = Console.ReadLine();

processManager.StartProcess(input);
Console.WriteLine(@"Enter Full Path with FileName and extension in order to create a file.
Afterwards the program will run modify and delete file scenarios on its own generated files");
input = Console.ReadLine();
fileManager.DoFileAction("create",input);
fileManager.DoFileAction("modify");
fileManager.DoFileAction("delete");
await networkManager.TransmitDataAsync();
Console.WriteLine("The program is complete. Press enter to exit");
Console.ReadLine();