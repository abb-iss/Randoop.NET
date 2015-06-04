This projects include two major components:
1. a VS2010 add-in with GUI for Randoop.NET
2. an installer of the VS add-in

Important notes:
- before build the installer, make sure copy the latest Randoop.NET executable and configuration files from "randoop-NET-src\ZipFolder\randoop\* (the ZipFolder will be created in the post-build session of randoop-NET-src project)" to "randoop-VS-plugin-2010\Randoop\". All these files will be read and wrapped into the installer.
