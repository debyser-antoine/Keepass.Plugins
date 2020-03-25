using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class ExportManager : Form
    {
        private readonly BindingList<ExportItem> _exports;

        public ExportManager()
        {
            InitializeComponent();
        }

        public ExportManager(IEnumerable<ExportItem> exports)
            : this()
        {
            if (ReferenceEquals(exports, null))
                throw new ArgumentNullException("exports");

            _exports = new BindingList<ExportItem>();
            foreach(ExportItem export in exports.OrderBy(e => e.Path.ToString()))
                _exports.Add(export);

            _exportsDataGridView.AutoGenerateColumns = false;
            _exportsDataGridView.DataSource = _exports;
            _pathColumn.DataPropertyName = "Path";
            _lastExportTimestampColumn.DataPropertyName = "LastExportTimeStamp";
        }

        public IEnumerable<ExportItem> Exports
        {
            get { return _exports; }
        }

        private void OnNewButtonClick(object sender, EventArgs e)
        {
            CreateExport createExport = new CreateExport();
            DialogResult ret = createExport.ShowDialog();
            if (ret != DialogResult.OK)
                return;

            foreach(ExportItem export in _exports)
            {
                if (!Uri.Equals(export.Path, createExport.Path))
                    continue;

                MessageBox.Show(this, "An export on  [{0}], already exist (Remove it first)", "Unable to create export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _exports.Add(new ExportItem(null, createExport.Path, null, createExport.Password));
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            List<ExportItem> toRemove = _exportsDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(dgvr => dgvr.DataBoundItem as ExportItem).ToList();
            foreach (ExportItem item in toRemove)
                _exports.Remove(item);
        }
    }
}
