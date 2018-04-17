using System.Collections.Generic;
using GR.Records.Core.Models;

namespace GR.Records.Core.Parser
{
    public interface IRecordParser
    {
        Record ParseRecord(string data);
        IEnumerable<Record> ParseRecordFile(string fileName);
    }
}
