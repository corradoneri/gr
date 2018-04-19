Assumptions
-----------
- All data can fit in memory
- LastName and FirstName are required fields
- All other fields are nullable
- A file will only use one of the delimiters
- A file will not contain the two unused delimiters in its data
- Leading and trailing blanks in the data are acceptable and will not be trimmed
- Empty file is valid
- Empty lines with in a file are valid and can simply be ignored

Notes
-----
- Everything is written in .NET Core
- To run the console application type:

	dotnet records.dll <file1> <file2> <file3>

- Can optionally pass -1, -2 or -3 as last parameter to sort by one of the three criteria
- WebAPI can be run in debugger for simplicity
- Test cases use MS Test
