using GR.Records.Core.Parser;
using GR.Records.Core.Sorter;

namespace GR.Records.ConsoleApp
{
    class Program
    {
        static int Main(string[] args)
        {
            // Create services classes that will be injected into Application instance
            //
            var recordParser = new RecordParser();
            var recordSorter = new RecordSorter();

            // Create a run the application
            //
            var application = new Application(args, recordParser, recordSorter);
            return application.Run();
        }
    }
}
