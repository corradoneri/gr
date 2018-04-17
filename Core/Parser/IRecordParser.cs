using GR.Records.Core.Models;

namespace GR.Records.Core.Parser
{
    public interface IRecordParser
    {
        Record ParseRecord(string data);
        RecordList ParseRecordFile(string path);
    }
}
