using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoExport
{
    public class ExportItem
    {
        private readonly string _uuid;
        private readonly Uri _path;
        private readonly DateTime? _lastExportTimeStamp;

        public ExportItem(string uuid, Uri path, DateTime? lastExportTimestamp)
        {
            _uuid = uuid;
            _path = path;
            _lastExportTimeStamp = lastExportTimestamp;
        }

        public string Uuid
        {
            get { return _uuid; }
        }

        public Uri Path
        {
            get { return _path; }
        }

        public DateTime? LastExportTimeStamp
        {
            get { return _lastExportTimeStamp; }
        }
    }
}
