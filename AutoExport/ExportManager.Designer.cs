namespace AutoExport
{
    partial class ExportManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._rightPanel = new System.Windows.Forms.Panel();
            this._removeButton = new System.Windows.Forms.Button();
            this._newButton = new System.Windows.Forms.Button();
            this._exportsDataGridView = new System.Windows.Forms.DataGridView();
            this._pathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._lastExportTimestampColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._exportsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _rightPanel
            // 
            this._rightPanel.Controls.Add(this._removeButton);
            this._rightPanel.Controls.Add(this._newButton);
            this._rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this._rightPanel.Location = new System.Drawing.Point(650, 0);
            this._rightPanel.Name = "_rightPanel";
            this._rightPanel.Size = new System.Drawing.Size(94, 411);
            this._rightPanel.TabIndex = 1;
            // 
            // _removeButton
            // 
            this._removeButton.Location = new System.Drawing.Point(7, 195);
            this._removeButton.Name = "_removeButton";
            this._removeButton.Size = new System.Drawing.Size(75, 23);
            this._removeButton.TabIndex = 3;
            this._removeButton.Text = "&Remove";
            this._removeButton.UseVisualStyleBackColor = true;
            this._removeButton.Click += new System.EventHandler(this.OnRemoveButtonClick);
            // 
            // _newButton
            // 
            this._newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._newButton.Location = new System.Drawing.Point(7, 157);
            this._newButton.Name = "_newButton";
            this._newButton.Size = new System.Drawing.Size(75, 23);
            this._newButton.TabIndex = 2;
            this._newButton.Text = "&Add";
            this._newButton.UseVisualStyleBackColor = true;
            this._newButton.Click += new System.EventHandler(this.OnNewButtonClick);
            // 
            // _exportsDataGridView
            // 
            this._exportsDataGridView.AllowUserToAddRows = false;
            this._exportsDataGridView.AllowUserToDeleteRows = false;
            this._exportsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._exportsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._pathColumn,
            this._lastExportTimestampColumn});
            this._exportsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._exportsDataGridView.Location = new System.Drawing.Point(0, 0);
            this._exportsDataGridView.Name = "_exportsDataGridView";
            this._exportsDataGridView.ReadOnly = true;
            this._exportsDataGridView.RowHeadersVisible = false;
            this._exportsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._exportsDataGridView.Size = new System.Drawing.Size(650, 411);
            this._exportsDataGridView.TabIndex = 2;
            this._exportsDataGridView.VirtualMode = true;
            // 
            // _pathColumn
            // 
            this._pathColumn.Frozen = true;
            this._pathColumn.HeaderText = "Export Path";
            this._pathColumn.Name = "_pathColumn";
            this._pathColumn.ReadOnly = true;
            this._pathColumn.Width = 450;
            // 
            // _lastExportTimestampColumn
            // 
            dataGridViewCellStyle1.Format = "o";
            this._lastExportTimestampColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this._lastExportTimestampColumn.HeaderText = "Last Export Timestamp";
            this._lastExportTimestampColumn.Name = "_lastExportTimestampColumn";
            this._lastExportTimestampColumn.ReadOnly = true;
            this._lastExportTimestampColumn.Width = 165;
            // 
            // ExportManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 411);
            this.Controls.Add(this._exportsDataGridView);
            this.Controls.Add(this._rightPanel);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(760, 450);
            this.Name = "ExportManager";
            this.Text = "Manage Auto Exports";
            this._rightPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._exportsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel _rightPanel;
        private System.Windows.Forms.Button _removeButton;
        private System.Windows.Forms.Button _newButton;
        private System.Windows.Forms.DataGridView _exportsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn _pathColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _lastExportTimestampColumn;
    }
}