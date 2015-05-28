REM Refresh compilation date (which we store in a file).

echo del /F %1\compiledate.txt
del /F %1\compiledate.txt

echo Creating compiledate.txt
date /T > %1\compiledate.txt
time /T >> %1\compiledate.txt


echo rmdir /S /Q %1\ZipFolder\randoop
rmdir /S /Q %1\ZipFolder\randoop
echo del randoop.zip
del randoop.zip

echo mkdir %1\ZipFolder
mkdir %1\ZipFolder

echo mkdir %1\ZipFolder\randoop
mkdir %1\ZipFolder\randoop

echo mkdir %1\ZipFolder\randoop\testresources
mkdir %1\ZipFolder\randoop\testresources

echo mkdir %1\ZipFolder\randoop\docs
mkdir %1\ZipFolder\randoop\docs

echo mkdir %1\ZipFolder\randoop\default_config_files
mkdir %1\ZipFolder\randoop\default_config_files

echo mkdir %1\ZipFolder\randoop\auxtools
mkdir %1\ZipFolder\randoop\auxtools

echo copy %1\auxtools\pageheap.exe %1\ZipFolder\randoop\auxtools\
copy %1\auxtools\pageheap.exe %1\ZipFolder\randoop\auxtools\


echo mkdir %1\ZipFolder\randoop\bin
mkdir %1\ZipFolder\randoop\bin

echo copy %1\RandoopCommon\bin\Release\RandoopCommon.dll %1\ZipFolder\randoop\bin\
copy %1\RandoopCommon\bin\Release\RandoopCommon.dll %1\ZipFolder\randoop\bin\
echo copy %1\RandoopCommon\bin\Release\RandoopCommon.pdb %1\ZipFolder\randoop\bin\
copy %1\RandoopCommon\bin\Release\RandoopCommon.pdb %1\ZipFolder\randoop\bin\

echo copy %1\AssemblyInfoPrinter\bin\Release\AssemblyInfoPrinter.exe %1\ZipFolder\randoop\bin\
copy %1\AssemblyInfoPrinter\bin\Release\AssemblyInfoPrinter.exe %1\ZipFolder\randoop\bin\
echo copy %1\AssemblyInfoPrinter\bin\Release\AssemblyInfoPrinter.pdb %1\ZipFolder\randoop\bin\
copy %1\AssemblyInfoPrinter\bin\Release\AssemblyInfoPrinter.pdb %1\ZipFolder\randoop\bin\

echo copy %1\RandoopTests\bin\Release\RandoopTests.exe %1\ZipFolder\randoop\bin\
copy %1\RandoopTests\bin\Release\RandoopTests.exe %1\ZipFolder\randoop\bin\
echo copy %1\RandoopTests\bin\Release\RandoopTests.pdb %1\ZipFolder\randoop\bin\
copy %1\RandoopTests\bin\Release\RandoopTests.pdb %1\ZipFolder\randoop\bin\

echo copy %1\Randoop\bin\Release\Randoop.exe %1\ZipFolder\randoop\bin\
copy %1\Randoop\bin\Release\Randoop.exe %1\ZipFolder\randoop\bin\
echo copy %1\Randoop\bin\Release\Randoop.pdb %1\ZipFolder\randoop\bin\
copy %1\Randoop\bin\Release\Randoop.pdb %1\ZipFolder\randoop\bin\

echo copy %1\RandoopBare\bin\Release\RandoopBare.exe %1\ZipFolder\randoop\bin\
copy %1\RandoopBare\bin\Release\RandoopBare.exe %1\ZipFolder\randoop\bin\
echo copy %1\RandoopBare\bin\Release\RandoopBare.pdb %1\ZipFolder\randoop\bin\
copy %1\RandoopBare\bin\Release\RandoopBare.pdb %1\ZipFolder\randoop\bin\

echo copy %1\RandoopImpl\bin\Release\RandoopImpl.dll %1\ZipFolder\randoop\bin\
copy %1\RandoopImpl\bin\Release\RandoopImpl.dll %1\ZipFolder\randoop\bin\
echo copy %1\RandoopImpl\bin\Release\RandoopImpl.pdb %1\ZipFolder\randoop\bin\
copy %1\RandoopImpl\bin\Release\RandoopImpl.pdb %1\ZipFolder\randoop\bin\


copy %1\packages\Mono.Cecil.0.9.5.3\lib\net40\Mono.Cecil.dll %1\ZipFolder\randoop\bin\
copy %1\packages\Mono.Cecil.0.9.5.3\lib\net40\Mono.Cecil.Mdb.dll %1\ZipFolder\randoop\bin\
copy %1\packages\Mono.Cecil.0.9.5.3\lib\net40\Mono.Cecil.Pdb.dll %1\ZipFolder\randoop\bin\
copy %1\packages\Mono.Cecil.0.9.5.3\lib\net40\Mono.Cecil.Rocks.dll %1\ZipFolder\randoop\bin\

echo copy %1\README.txt %1\ZipFolder\randoop\
copy %1\README.txt %1\ZipFolder\randoop\

echo copy %1\RELEASE_NOTES.txt %1\ZipFolder\randoop\
copy %1\RELEASE_NOTES.txt %1\ZipFolder\randoop\

echo copy %1\compiledate.txt %1\ZipFolder\randoop\
copy %1\compiledate.txt %1\ZipFolder\randoop\

echo copy "%1\Randoop Manual.doc" %1\ZipFolder\randoop\docs\
copy "%1\Randoop Manual.doc" %1\ZipFolder\randoop\docs\

echo copy "%1\Randoop Developer Notes.doc" %1\ZipFolder\randoop\docs\
copy "%1\Randoop Developer Notes.doc" %1\ZipFolder\randoop\docs\

copy %1\default_config_files\seed_sbyte.txt  %1\ZipFolder\randoop\default_config_files\seed_sbyte.txt
copy %1\default_config_files\seed_byte.txt  %1\ZipFolder\randoop\default_config_files\seed_byte.txt
copy %1\default_config_files\seed_short.txt  %1\ZipFolder\randoop\default_config_files\seed_short.txt
copy %1\default_config_files\seed_ushort.txt  %1\ZipFolder\randoop\default_config_files\seed_ushort.txt
copy %1\default_config_files\seed_int.txt  %1\ZipFolder\randoop\default_config_files\seed_int.txt
copy %1\default_config_files\seed_uint.txt  %1\ZipFolder\randoop\default_config_files\seed_uint.txt
copy %1\default_config_files\seed_long.txt  %1\ZipFolder\randoop\default_config_files\seed_long.txt
copy %1\default_config_files\seed_ulong.txt  %1\ZipFolder\randoop\default_config_files\seed_ulong.txt
copy %1\default_config_files\seed_char.txt  %1\ZipFolder\randoop\default_config_files\seed_char.txt
copy %1\default_config_files\seed_float.txt  %1\ZipFolder\randoop\default_config_files\seed_float.txt
copy %1\default_config_files\seed_double.txt  %1\ZipFolder\randoop\default_config_files\seed_double.txt
copy %1\default_config_files\seed_bool.txt  %1\ZipFolder\randoop\default_config_files\seed_bool.txt
copy %1\default_config_files\seed_decimal.txt  %1\ZipFolder\randoop\default_config_files\seed_decimal.txt
copy %1\default_config_files\seed_string.txt  %1\ZipFolder\randoop\default_config_files\seed_string.txt

copy %1\default_config_files\mapped_methods.txt  %1\ZipFolder\randoop\default_config_files\mapped_methods.txt

echo copy %1\default_config_files\forbid_fields.txt %1\ZipFolder\randoop\default_config_files\forbid_fields.txt
copy %1\default_config_files\forbid_fields.txt %1\ZipFolder\randoop\default_config_files\forbid_fields.txt
echo copy %1\default_config_files\forbid_types.txt %1\ZipFolder\randoop\default_config_files\forbid_types.txt
copy %1\default_config_files\forbid_types.txt %1\ZipFolder\randoop\default_config_files\forbid_types.txt
echo copy %1\default_config_files\forbid_members.txt %1\ZipFolder\randoop\default_config_files\forbid_members.txt
copy %1\default_config_files\forbid_members.txt %1\ZipFolder\randoop\default_config_files\forbid_members.txt
echo copy %1\default_config_files\require_types.txt %1\ZipFolder\randoop\default_config_files\require_types.txt
copy %1\default_config_files\require_types.txt %1\ZipFolder\randoop\default_config_files\require_types.txt
echo copy %1\default_config_files\require_members.txt %1\ZipFolder\randoop\default_config_files\require_members.txt
copy %1\default_config_files\require_members.txt %1\ZipFolder\randoop\default_config_files\require_members.txt
echo copy %1\default_config_files\require_fields.txt %1\ZipFolder\randoop\default_config_files\require_fields.txt
copy %1\default_config_files\require_fields.txt %1\ZipFolder\randoop\default_config_files\require_fields.txt

REM echo "C:\Program Files\WinZip\WINZIP32.EXE" -min -a -r %1\randoop.zip %1\ZipFolder
REM "C:\Program Files\WinZip\WINZIP32.EXE" -min -a -r %1\randoop.zip %1\ZipFolder
