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
            foreach(ExportItem export in exports)
                _exports.Add(export);

            _exportsDataGridView.AutoGenerateColumns = false;
            _exportsDataGridView.DataSource = _exports;
            _pathColumn.DataPropertyName = "Path";
            _lastExportTimestampColumn.DataPropertyName = "LastExportTimeStamp";
        }

        private void OnNewButtonClick(object sender, EventArgs e)
        {
            CreateExport createExport = new CreateExport();
            DialogResult ret = createExport.ShowDialog();
            if (ret != DialogResult.OK)
                return;
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {

        }
    }
}
