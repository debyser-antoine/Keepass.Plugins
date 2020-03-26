namespace AutoExport
{
    partial class CreateExport
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
            this._pathLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._pathTextBox = new System.Windows.Forms.TextBox();
            this._cancelButton = new System.Windows.Forms.Button();
            this._createButton = new System.Windows.Forms.Button();
            this._pathButton = new System.Windows.Forms.Button();
            this._passwordSecureTextBoxEx = new KeePass.UI.SecureTextBoxEx();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // _pathLabel
            // 
            this._pathLabel.AutoSize = true;
            this._pathLabel.Location = new System.Drawing.Point(13, 13);
            this._pathLabel.Name = "_pathLabel";
            this._pathLabel.Size = new System.Drawing.Size(67, 13);
            this._pathLabel.TabIndex = 0;
            this._pathLabel.Text = "Export path :";
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Location = new System.Drawing.Point(21, 39);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(59, 13);
            this._passwordLabel.TabIndex = 1;
            this._passwordLabel.Text = "Password :";
            // 
            // _pathTextBox
            // 
            this._pathTextBox.Location = new System.Drawing.Point(86, 10);
            this._pathTextBox.Name = "_pathTextBox";
            this._pathTextBox.ReadOnly = true;
            this._pathTextBox.Size = new System.Drawing.Size(655, 20);
            this._pathTextBox.TabIndex = 2;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(698, 113);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _createButton
            // 
            this._createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._createButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._createButton.Location = new System.Drawing.Point(617, 113);
            this._createButton.Name = "_createButton";
            this._createButton.Size = new System.Drawing.Size(75, 23);
            this._createButton.TabIndex = 4;
            this._createButton.Text = "Create";
            this._createButton.UseVisualStyleBackColor = true;
            // 
            // _pathButton
            // 
            this._pathButton.Location = new System.Drawing.Point(747, 8);
            this._pathButton.Name = "_pathButton";
            this._pathButton.Size = new System.Drawing.Size(26, 23);
            this._pathButton.TabIndex = 5;
            this._pathButton.Text = "...";
            this._pathButton.UseVisualStyleBackColor = true;
            this._pathButton.Click += new System.EventHandler(this.OnPathButtonClick);
            // 
            // _passwordSecureTextBoxEx
            // 
            this._passwordSecureTextBoxEx.Location = new System.Drawing.Point(86, 36);
            this._passwordSecureTextBoxEx.Name = "_passwordSecureTextBoxEx";
            this._passwordSecureTextBoxEx.Size = new System.Drawing.Size(655, 20);
            this._passwordSecureTextBoxEx.TabIndex = 6;
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "Keepass databse|*kdbx";
            this._saveFileDialog.RestoreDirectory = true;
            // 
            // CreateExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 148);
            this.Controls.Add(this._passwordSecureTextBoxEx);
            this.Controls.Add(this._pathButton);
            this.Controls.Add(this._createButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._pathTextBox);
            this.Controls.Add(this._passwordLabel);
            this.Controls.Add(this._pathLabel);
            this.Name = "CreateExport";
            this.Text = "Create Export";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _pathLabel;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.TextBox _pathTextBox;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _createButton;
        private System.Windows.Forms.Button _pathButton;
        private KeePass.UI.SecureTextBoxEx _passwordSecureTextBoxEx;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}