using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace AutoExport
{
    public sealed class AutoExportExt : KeePass.Plugins.Plugin
    {
        private const string Tag = "AutoExportPlugin";
        private const string LastExportTimeKeyName = "AutoExport_LastExportTime";
        private const string KeePassDatabaseExtension = ".kdbx";
        private const int NetworkServerNotFoundErrorCode = unchecked((int) 0x80070035);

        private static readonly KeePassLib.PwUuid _groupUuid = new KeePassLib.PwUuid(new byte[]
                                                               {
                                                                   0x34,
                                                                   0xA9,
                                                                   0xAA,
                                                                   0x23,
                                                                   0xD4,
                                                                   0xEC,
                                                                   0x41,
                                                                   0xAA,
                                                                   0xB8,
                                                                   0xB3,
                                                                   0xF9,
                                                                   0xFF,
                                                                   0xB8,
                                                                   0x55,
                                                                   0xD6,
                                                                   0x1B
                                                               });

        private KeePass.Plugins.IPluginHost _host;
        private KeePassLib.Interfaces.IStatusLogger _windowLogger;
        private KeePassLib.Interfaces.IStatusLogger _statusBarLogger;

        public override bool Initialize(KeePass.Plugins.IPluginHost host)
        {
            bool ret = base.Initialize(host);
            if (!ret || ReferenceEquals(host, null))
                return false;

            _host = host;
            _windowLogger = _host.MainWindow.CreateShowWarningsLogger();
            _statusBarLogger = _host.MainWindow.CreateStatusBarLogger();
            _host.MainWindow.FileSaving += OnDatabaseSaving;

            return true;
        }

        public override void Terminate()
        {
            if (!ReferenceEquals(_host, null)) // In case where Initialize was not called successfully
                _host.MainWindow.FileSaving -= OnDatabaseSaving;

            base.Terminate();
        }

        public override ToolStripMenuItem GetMenuItem(KeePass.Plugins.PluginMenuType t)
        {
            // Provide a menu item for the main location(s)
            if (t == KeePass.Plugins.PluginMenuType.Main)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = "Manage Auto Export";
                tsmi.Click += this.OnManageAutoExportClicked;
                return tsmi;
            }

            return null; // No menu items in other locations
        }

        private void OnManageAutoExportClicked(object sender, EventArgs e)
        {
            if (ReferenceEquals(_host, null) || ReferenceEquals(_host.Database, null) || !_host.Database.IsOpen)
                return;

            KeePassLib.Collections.PwObjectList<KeePassLib.PwEntry> entries = new KeePassLib.Collections.PwObjectList<KeePassLib.PwEntry>();
            _host.Database.RootGroup.FindEntriesByTag(Tag, entries, true);
            ExportManager exportManager = new ExportManager(entries.Select(ConvertToExportItem));
            exportManager.ShowDialog();

            //Remove deleted entry
            bool hasDelete = false;
            foreach(KeePassLib.PwEntry entry in entries)
            {
                string uuid = entry.Uuid.ToHexString();
                if (exportManager.Exports.Any(ei => !string.IsNullOrEmpty(ei.Uuid) && string.Equals(ei.Uuid, uuid, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                hasDelete = true;
                KeePassLib.PwDeletedObject deletedObject = new KeePassLib.PwDeletedObject(entry.Uuid, DateTime.Now);
                _host.Database.DeletedObjects.Add(deletedObject);
            }

            //Create new entry
            bool hasNew = false;
            KeePassLib.PwGroup savingGroup = FindOrCreatePluginGroup();
            foreach (ExportItem exportItem in exportManager.Exports)
            {
                if (!string.IsNullOrEmpty(exportItem.Uuid))
                    continue;

                hasNew = true;
                KeePassLib.PwEntry newExportEntry = new KeePassLib.PwEntry(true, true);
                newExportEntry.Strings.Set(KeePassLib.PwDefs.TitleField, new KeePassLib.Security.ProtectedString(false, Path.GetFileName(exportItem.Path.LocalPath)));
                newExportEntry.Strings.Set(KeePassLib.PwDefs.UrlField, new KeePassLib.Security.ProtectedString(false, exportItem.Path.ToString()));
                newExportEntry.IconId = KeePassLib.PwIcon.Disk;
                newExportEntry.Tags = new List<string>() { Tag };
                newExportEntry.Strings.Set(KeePassLib.PwDefs.PasswordField, exportItem.Password);
                savingGroup.AddEntry(newExportEntry, true);
            }

            if (hasDelete || hasNew) //Perform action on database
            {
                _host.Database.MergeIn(_host.Database, KeePassLib.PwMergeMethod.Synchronize);

                //Refresh GUI (Event is launched by main UI thread)
                _host.MainWindow.UpdateUI(false, null, true, savingGroup, true, null, true);
                _host.MainWindow.RefreshEntriesList();
            }
        }

        private ExportItem ConvertToExportItem(KeePassLib.PwEntry entry)
        {
            string urlValue = entry.Strings.ReadSafe(KeePassLib.PwDefs.UrlField);
            string lastExportTimestampStr = entry.Strings.ReadSafe(LastExportTimeKeyName);
            DateTime? lastExportTimestamp = null;
            if (!string.IsNullOrEmpty(lastExportTimestampStr))
            {
                lastExportTimestamp = DateTime.Parse(lastExportTimestampStr);
                lastExportTimestamp = new DateTime(lastExportTimestamp.Value.Ticks, DateTimeKind.Utc);
            }
            return new ExportItem(entry.Uuid.ToHexString(), new Uri(urlValue), lastExportTimestamp);
        }

        private KeePassLib.PwGroup FindOrCreatePluginGroup()
        {
            KeePassLib.PwGroup group = _host.Database.RootGroup.FindGroup(_groupUuid, true);
            if (ReferenceEquals(group, null))
            {
                group = new KeePassLib.PwGroup(false, true, "Auto Exports", KeePassLib.PwIcon.Disk)
                        {
                            Uuid = _groupUuid,
                        };
                _host.Database.RootGroup.AddGroup(group, true);
            }

            return group;
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
                    string urlValue = entry.Strings.ReadSafe(KeePassLib.PwDefs.UrlField);
                    if (string.IsNullOrEmpty(urlValue) || !Uri.IsWellFormedUriString(urlValue, UriKind.Absolute))
                        continue;

                    Uri filePath = new Uri(urlValue);
                    try
                    {
                        if (Export(fileSavingEventArgs.Database, filePath, entry.Strings.GetSafe(KeePassLib.PwDefs.PasswordField), _windowLogger))
                            entry.Strings.Set(LastExportTimeKeyName, new KeePassLib.Security.ProtectedString(false, DateTime.UtcNow.ToString("o")));
                    }
                    catch (IOException ioex)
                    {
                        if (ioex.HResult == NetworkServerNotFoundErrorCode) //Network path not found
                            _statusBarLogger.SetText(string.Format(CultureInfo.InvariantCulture, "Auto Export [{0}] failed:", filePath.LocalPath), KeePassLib.Interfaces.LogStatusType.Warning);
                        else
                            KeePassLib.Utility.MessageService.ShowWarning(string.Format(CultureInfo.InvariantCulture, "Auto Export [{0}] failed:", filePath.LocalPath), ioex);
                    }
                    catch (Exception ex)
                    {
                        KeePassLib.Utility.MessageService.ShowWarning(string.Format(CultureInfo.InvariantCulture, "Auto Export [{0}] failed:", filePath.LocalPath), ex);
                    }
                }
            }
            catch (Exception e)
            {
                KeePassLib.Utility.MessageService.ShowWarning("Auto Exports failed:", e);
            }
        }


        private static bool Export(KeePassLib.PwDatabase database, Uri filePath, KeePassLib.Security.ProtectedString password, KeePassLib.Interfaces.IStatusLogger logger)
        {
            Exception argumentError = CheckArgument(database, filePath, password);
            if (!ReferenceEquals(argumentError, null))
                throw argumentError;

            if (string.Equals(database.IOConnectionInfo.Path, filePath.LocalPath, StringComparison.InvariantCultureIgnoreCase))
                return false; //Don't export myself

            //Create new database in temporary file
            KeePassLib.PwDatabase exportedDatabase = new KeePassLib.PwDatabase();
            exportedDatabase.Compression = KeePassLib.PwCompressionAlgorithm.GZip;
            KeePassLib.Serialization.IOConnectionInfo connectionInfo = new KeePassLib.Serialization.IOConnectionInfo();
            string storageDirectory = Path.GetDirectoryName(filePath.LocalPath);
            string tmpPath = Path.Combine(storageDirectory, string.Format("{0}{1}", Guid.NewGuid(), KeePassDatabaseExtension));
            connectionInfo.Path = tmpPath;
            connectionInfo.CredSaveMode = KeePassLib.Serialization.IOCredSaveMode.SaveCred;
            KeePassLib.Keys.CompositeKey exportedKey = new KeePassLib.Keys.CompositeKey();
            exportedKey.AddUserKey(new KeePassLib.Keys.KcpPassword(password.ReadString()));
            exportedDatabase.New(connectionInfo, exportedKey);
            exportedDatabase.RootGroup.Name = database.RootGroup.Name;

            //Merge current database in temporary file
            exportedDatabase.MergeIn(database, KeePassLib.PwMergeMethod.OverwriteExisting, logger);
            exportedDatabase.Save(logger);
            exportedDatabase.Close();

            //Move temporary file into target backup path
            if (File.Exists(filePath.LocalPath))
                File.Delete(filePath.LocalPath);
            File.Move(tmpPath, filePath.LocalPath);

            return true;
        }

        private static Exception CheckArgument(KeePassLib.PwDatabase database, Uri filePath, KeePassLib.Security.ProtectedString password)
        {
            if (ReferenceEquals(database, null))
                return new ArgumentNullException("database");
            if (ReferenceEquals(filePath, null))
                return new ArgumentNullException("filePath");
            if (!filePath.IsFile)
                return new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Path [{0}] is not a file", filePath.LocalPath), "filePath");
            if (!Path.GetExtension(filePath.LocalPath).Equals(KeePassDatabaseExtension, StringComparison.InvariantCultureIgnoreCase))
                return new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File [{0}] must be a KeePass database (*.{1})", filePath.LocalPath, KeePassDatabaseExtension), "filePath");
            if (ReferenceEquals(password, null) || password.IsEmpty)
                return new ArgumentNullException("password");

            return null;
        }
    }
}
