## How it works
----------------------------------------------------------
1. The configuration sits in FileAnalyzer\appsettings.json
2. The Logging section can be leveraged to change the logging level. (ex. For debug logs change the value of "Default" to "Debug")
3. Folder path is the path where the app scans to find  *.csv files
4. Providers section determines the  if 'a' found CSV file need to be processed and the Column indices of the 'Date' and 'Value' columns

## Unit tests
----------------------------------------------------------
There are few unit tests to make sure the app behaves accurately (naming can be improved)

## Why .netCore
----------------------------------------------------------
1. I had never done a .net core console app prior to this hence used this as a learning opportunity
2. Wanted to investigate on .net core's dependency injection within a console app.


## Possible improvements
----------------------------------------------------------
The binary search for upper and lower bound can be handed over to seperate tasks and can leverage the ConcurrentList class that I included in the project

## Technologies
----------------------------------------------------------
1. .net Core 2.x
2. Nunit + Mock for testing
