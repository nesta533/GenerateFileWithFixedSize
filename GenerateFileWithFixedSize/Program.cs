using System;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;

namespace GenerateFileWithFixedSize
{
    class Program
    {
        static void Main(string[] args)
        {

            var app = new CommandLineApplication();

            app.HelpOption("--help|-h|-?");
            app.ShowHelp("usage:GenerateLargeFile -o file -s size");
            var optFile = app.Option("--output -o <value>", "file path", CommandOptionType.SingleValue);
            var optSize = app.Option("--size -s <value>", "file size in K units", CommandOptionType.SingleValue);
            Func<CommandOption, CommandOption, int> onAppExecute = ((file, size) =>
            {
                if (!file.HasValue())
                {
                    Console.Error.WriteLine($"The file path is null.");
                    return -1;
                }

                string targetFile = file.Value();
                if (string.IsNullOrEmpty(targetFile))
                {
                    Console.Error.WriteLine($"The file path is invalid.");
                    return -2;
                }

                if (!size.HasValue())
                {
                    Console.Error.WriteLine($"The size is null.");
                    return -1;
                }

                int fixedSize = 0;
                if (!int.TryParse(size.Value(), out fixedSize))
                {
                    Console.Error.WriteLine($"Invalid size.");
                    return -3;
                }

                const int kunit = 1024;
                fixedSize = fixedSize * kunit;

                using (FileStream fs = new FileStream(targetFile, FileMode.Create))
                {
                    fs.Seek(fixedSize, SeekOrigin.Begin);
                    byte[] myByteArray = Enumerable.Repeat((byte)'a', fixedSize).ToArray();
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(myByteArray, 0, fixedSize);
                }

                Console.WriteLine($"Successful written!");
                return 0;
            });

            app.OnExecute(() => { return onAppExecute(optFile, optSize); });

            app.Execute(args);

        }
    }
}
