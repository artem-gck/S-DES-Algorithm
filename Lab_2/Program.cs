using Lab_2;

const string Hello = "Program for encrypt and decrypt data in files .txt files";
const string InputPath = "Input path to input file: ";
const string OutputPath = "Input path to output file: ";
const string InputKey = "Input key in binary form: ";

Console.WriteLine(Hello);

Console.Write(InputPath);
var inputPath = Console.ReadLine();
Console.Write(OutputPath);
var outputPath = Console.ReadLine();
var key = ConsoleValidation.ValidateBinary(InputKey);

FileWorker.EncryptFile(inputPath, outputPath, key);
Console.WriteLine();

Console.Write(InputPath);
inputPath = Console.ReadLine();
Console.Write(OutputPath);
outputPath = Console.ReadLine();
key = ConsoleValidation.ValidateBinary(InputKey);

FileWorker.DecryptFile(inputPath, outputPath, key);