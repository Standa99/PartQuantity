# PartQuantity
Arkance homework

## Instructions
- clone this repo
- place input file to projectDirectory\Data
- run with Visual Studio 2022 -> run app 
   - run using command line - set QTY_ENV=production
                            - dotnet run
- provide input fileName (without *.csv)
- in projectDirectory\Outputs find output file in following name format yyyy-MM-dd_HH-mm-ss_inputFileName_output.csv

## Notes
A few hours later, I realized the bug that a different default path must be set when running via the command line. This bug was fixed by implementing the QTY_ENV variable
