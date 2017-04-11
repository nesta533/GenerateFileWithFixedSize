using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var optSize = app.Option("--size -s <value>", "file size", CommandOptionType.SingleValue);

            Func<CommandOption, CommandOption, int> onAppExecute = ((file, size) =>
            {

                return 0;
            });

            app.Execute(args);


            const int fixedSize = 100 * 1024; //10 M
            FileStream fs = new FileStream(@"c:\temp\huge_dummy_file.txt", FileMode.Create);
            fs.Seek(fixedSize, SeekOrigin.Begin);
            byte[] myByteArray = Enumerable.Repeat((byte)'a', fixedSize).ToArray();
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(myByteArray, 0, fixedSize);
            fs.Close();

        }
    }
}
