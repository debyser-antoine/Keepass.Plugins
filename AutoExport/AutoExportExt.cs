using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using KeePassLib.Keys;
using KeePassLib.Security;

namespace AutoExport
{
    public sealed class AutoExportExt : KeePass.Plugins.Plugin
    {
        private const string Tag = "AutoExportPlugin";
        private const string PasswordKeyName = "Password";
        private const string UrlKeyName = "URL";
        private const string KeePassDatabaseExtension = ".kdbx";


        private readonly object _locker = new object();
        private readonly ISet<string> _entries = new HashSet<string>();

        private KeePass.Plugins.IPluginHost _host;
        private KeePassLib.Interfaces.IStatusLogger _logger;

        public override bool Initialize(KeePass.Plugins.IPluginHost host)
        {
            bool ret = base.Initialize(host);
            if (!ret || ReferenceEquals(host, null))
                return false;

            _host = host;
            _logger = _host.MainWindow.CreateShowWarningsLogger();
            _host.MainWindow.FileSaving += OnDatabaseSaving;

            return true;
        }

        public override void Terminate()
        {
            if (!ReferenceEquals(_host, null)) // In case where Initialize was not called successfully
                _host.MainWindow.FileSaving -= OnDatabaseSaving;

            base.Terminate();
        }

        private void OnDatabaseSaving(object sender, KeePass.Forms.FileSavingEventArgs fileSavingEventArgs)
        {
            if (ReferenceEquals(fileSavingEventArgs, null) || ReferenceEquals(fileSavingEventArgs.Database, null) || !fileSavingEventArgs.Database.IsOpen)
                return;

            try
            {
                KeePassLib.Collections.PwObjectList<KeePassLib.PwEntry> entries = new KeePassLib.Collections.PwObjectList<KeePassLib.PwEntry>();
                fileSavingEventArgs.Database.RootGroup.FindEntriesByTag(Tag, entries, true);
                foreach (KeePassLib.PwEntry entry in entries)
                {
                    ProtectedString urlValue = entry.Strings.GetSafe(UrlKeyName);
                    if (urlValue.IsEmpty || !Uri.IsWellFormedUriString(urlValue.ReadString(), UriKind.Absolute))
                        continue;

                    Uri filePath = new Uri(urlValue.ReadString());
                    try
                    {
                        Export(fileSavingEventArgs.Database, filePath, entry.Strings.GetSafe(PasswordKeyName), _logger);
                    }
                    catch (Exception ex)
                    {
                        KeePassLib.Utility.MessageService.ShowWarning(string.Format(CultureInfo.InvariantCulture, "Auto Export [{0}] failed:", filePath.AbsolutePath), ex);
                    }
                }
            }
            catch (Exception e)
            {
                KeePassLib.Utility.MessageService.ShowWarning("Auto Exports failed:", e);
            }
        }

        private static void Export(KeePassLib.PwDatabase database, Uri filePath, KeePassLib.Security.ProtectedString password, KeePassLib.Interfaces.IStatusLogger logger)
        {
            Exception argumentError = CheckArgument(database, filePath, password);
            if (!ReferenceEquals(argumentError, null))
                throw argumentError;

            //Create new databse in temporary file
            KeePassLib.PwDatabase exportedDatabase = new KeePassLib.PwDatabase();
            exportedDatabase.Compression = KeePassLib.PwCompressionAlgorithm.GZip;
            KeePassLib.Serialization.IOConnectionInfo connectionInfo = new KeePassLib.Serialization.IOConnectionInfo();
            string storageDirectory = Path.GetDirectoryName(filePath.AbsolutePath);
            string tmpPath = Path.Combine(storageDirectory, string.Format("{0}{1}", Guid.NewGuid(), KeePassDatabaseExtension));
            connectionInfo.Path = tmpPath;
            connectionInfo.CredSaveMode = KeePassLib.Serialization.IOCredSaveMode.SaveCred;
            CompositeKey exportedKey = new KeePassLib.Keys.CompositeKey();
            exportedKey.AddUserKey(new KeePassLib.Keys.KcpPassword(password.ReadString()));
            exportedDatabase.New(connectionInfo, exportedKey);

            //Merge current database in temporary file
            exportedDatabase.MergeIn(database, KeePassLib.PwMergeMethod.OverwriteExisting, logger);
            exportedDatabase.Save(logger);
            exportedDatabase.Close();

            //Move temporaray file into target backup path
            if (File.Exists(filePath.AbsolutePath))
                File.Delete(filePath.AbsolutePath);
            File.Move(tmpPath, filePath.AbsolutePath);
        }

        private static Exception CheckArgument(KeePassLib.PwDatabase database, Uri filePath, KeePassLib.Security.ProtectedString password)
        {
            if (ReferenceEquals(database, null))
                return new ArgumentNullException(nameof(database));
            if (ReferenceEquals(filePath, null))
                return new ArgumentNullException(nameof(filePath));
            if (!filePath.IsFile)
                return new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Path [{0}] is not a file", filePath.AbsolutePath), nameof(filePath));
            if (!Path.GetExtension(filePath.AbsolutePath).Equals(KeePassDatabaseExtension, StringComparison.InvariantCultureIgnoreCase))
                return new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File [{0}] must be a KeePass database (*.{1})", filePath.AbsolutePath, KeePassDatabaseExtension), nameof(filePath));
            if (ReferenceEquals(password, null) || password.IsEmpty)
                return new ArgumentNullException(nameof(password));

            return null;
        }
    }
}
