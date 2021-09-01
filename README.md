## Construction Personal Tracking System
Note: This repostory is archived and is now read only. For the completed project, see [personnel-tracking](https://github.com/zubeyir-bodur/personnel-tracking)
### Cloning the repository into VS 2019:
1. Make sure you have downloaded visual studio 2019 and the following:
  - ASP.Net ile Web Geliştirme
  - .Net ile masaüstü geliştirme
  - Evrensel Windows platformu geliştirme
  - .Net ile Mobil uygulaması geliştirme
2. Open visual studio and select Create a new project
3. Among the list of project templates, find ASP.NET Core Web Application and select it. If you cannot, click the "Clear all" and search it.
4. Name the project as "Construction-Personal-Tracking-System" and select "ASP.Net Core Empty" as ASP.NET Core 5.0;
5. After the project is constructed and opened, right-click the solution explorer and click "Open folder in File Explorer".
6. Delete all files except .sln file. Move your .sln file to somewhere else and clone the repository here. 
7. Then move your .sln file into one directory behind of your project. Say, the folder "Construction-Personal-Tracking-System", which contains 
the cloned files, should be in the same directory with the .sln file. In the end, you should have the repository directory and .sln
file in a single folder.
8. Now you need to edit the .sln file, so that your solution can find the path for your files. By default, it will try to locate the directory
it belongs. Open notepad and locate a line similar to this:
```` 
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Construction-Personal-Tracking-System", "Construction-Personal-Tracking-System.csproj", "{ABB1918E-97B7-4835-92C4-31133F1442ED}"
````
9. Change ``Construction-Personal-Tracking-System.csproj`` into ``Construction-Personal-Tracking-System\Construction-Personal-Tracking-System.csproj ``

10. Open & build the project.

Note: Name of project should be the same as the repository since the nuget packages download automatically. We will try to add photos if needed.

