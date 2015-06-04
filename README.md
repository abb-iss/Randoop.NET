# Randoop.NET
Randoop.NET is an API fuzzing unit test generator for .NET libraries.

There are four projects in the repository:

1. randoop-NET-src-orig: this is the original Randoop.NET project downloaded from https://randoop.codeplex.com/. It is a command line based tool developed in VS2008.

2. randoop-NET-src: this is a version of Randoop.NET by fixing a few bugs and incorporating a couple of new features (also upgraded to VS2010). The major differences between this version and the original tool are described in the document of "ChangeSets.docx".

3. randoop-VS-plugin-2010: this is a project that creates a GUI for the Randoop.NET and installs the tool as an add-in to the Visual Studio 2010. The introduction to this add-in is described in the document of "Randoop.NET VS add-in Manual.pdf".

4. ClassLibrary1: this is a test project which illustrates how the VS add-in of Randoop.NET generates unit tests in a MSTest format and runs the tests directly from the IDE with the capability to collect testing information, such as code coverage.  