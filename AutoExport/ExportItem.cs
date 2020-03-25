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
        private KeePassLib.Security.ProtectedString _password;

        public ExportItem(string uuid, Uri path, DateTime? lastExportTimestamp)
            : this(uuid, path, lastExportTimestamp, null)
        {
        }

        public ExportItem(string uuid, Uri path, DateTime? lastExportTimestamp, KeePassLib.Security.ProtectedString password)
        {
            _uuid = uuid;
            _path = path;
            _lastExportTimeStamp = lastExportTimestamp;
            _password = password;

            if (ReferenceEquals(path, null) || (!path.IsFile && !path.IsUnc))
                throw new ArgumentNullException("path");
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

        public KeePassLib.Security.ProtectedString Password
        {
            get { return _password; }
        }
    }
}
