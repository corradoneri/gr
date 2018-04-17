using GR.Records.Core.Parser;
using GR.Records.Core.Sorter;

namespace GR.Records.ConsoleApp
{
    class Program
    {
        static int Main(string[] args)
        {
            var recordParser = new RecordParser();
            var recordSorter = new RecordSorter();
            return new Application(args, recordParser, recordSorter).Run();
        }
    }
}
