# rcendactgen
Red Canary Endpoint Activity Generator

## Building Project

- This project was written in .NET 6 on a MacOS machine. You will not need the .NET 6 dependency in order to run the program (verified with ubuntu in a docker container) but you will need it in order to run the source code.
- Inside the directory where you extracted the source code run `dotnet build`
- Then inside the same directory run the following command: `dotnet run --project src/rcendactgen.Main`

## Logging

- This program keeps a json log of the activity performed. Here is a sample:
```
{
  "ProcessStartActivities": [
    {
      "StartTime": "2022-07-26T07:29:37.583Z",
      "UserName": "Someusmbook2022\\someuser",
      "ProcessName": "ls -l",
      "ProcessCommandLine": "ls -l",
      "ProcessId": 57891
    }
  ],
  "FileActivities": [
    {
      "ActivityDescriptor": "create",
      "Timestamp": "2022-07-26T07:29:46.274Z",
      "FileFullPath": "/Users/someuser/Projects/rcendactgen/somefile0726/j.txt",
      "UserName": "Someusmbook2022\\someuser",
      "ProcessName": "rcendactgen.Mai",
      "ProcessCommandLine": "/Users/someuser/Projects/rcendactgen/releases/macos-arm64/rcendactgen.Main.dll",
      "ProcessId": 57885
    },
    {
      "ActivityDescriptor": "modified",
      "Timestamp": "2022-07-26T07:29:46.284Z",
      "FileFullPath": "/Users/someuser/Projects/rcendactgen/releases/macos-arm64/ProgramData/modify_file.txt",
      "UserName": "Someusmbook2022\\someuser",
      "ProcessName": "rcendactgen.Mai",
      "ProcessCommandLine": "/Users/someuser/Projects/rcendactgen/releases/macos-arm64/rcendactgen.Main.dll",
      "ProcessId": 57885
    },
    {
      "ActivityDescriptor": "delete",
      "Timestamp": "2022-07-26T07:29:46.285Z",
      "FileFullPath": "/Users/someuser/Projects/rcendactgen/releases/macos-arm64/ProgramData/delete_file.txt",
      "UserName": "Someusmbook2022\\someuser",
      "ProcessName": "rcendactgen.Mai",
      "ProcessCommandLine": "/Users/someuser/Projects/rcendactgen/releases/macos-arm64/rcendactgen.Main.dll",
      "ProcessId": 57885
    }
  ],
  "NetworkActivities": [
    {
      "Timestamp": "2022-07-26T07:29:46.289Z",
      "Username": "Someusmbook2022\\someuser",
      "Destination": {
        "Address": "http://jsonplaceholder.typicode.com/posts",
        "Port": 80
      },
      "Source": {
        "Address": "Someusmbook2022.fios-router.home",
        "Port": 80
      },
      "AmountOfDataSent": 39,
      "ProtocolOfDataSent": "tcp",
      "ProcessName": "rcendactgen.Mai",
      "ProcessCommandLine": "/Users/someuser/Projects/rcendactgen/releases/macos-arm64/rcendactgen.Main.dll",
      "ProcessId": 57885
    }
  ]
}
```
