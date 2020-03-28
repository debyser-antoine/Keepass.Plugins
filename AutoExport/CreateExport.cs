using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class CreateExport : Form
    {
        private readonly KeePass.UI.PwInputControlGroup _passwordControler;

        public CreateExport()
        {
            InitializeComponent();
            _passwordControler = new KeePass.UI.PwInputControlGroup();
            _passwordControler.Attach(_passwordSecureTextBoxEx,
                                      _hiddenPasswordCheckBox,
                                      _repeatLabel,
                                      _repeatSecureTextBoxEx,
                                      _qualityLabel,
                                      _qualityProgressBar,
                                      _qualityResultLabel,
                                      _passwordToolTip,
                                      this,
                                      _hiddenPasswordCheckBox.Checked,
                                      false);
        }

        private void OnPathButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_pathTextBox.Text))
            {
                if (Uri.IsWellFormedUriString(_pathTextBox.Text, UriKind.Absolute))
                    _saveFileDialog.FileName = _pathTextBox.Text;
            }

            DialogResult ret =_saveFileDialog.ShowDialog();
            if (ret != DialogResult.OK)
                return;

            Uri result = new Uri(new Uri("file:///"), _saveFileDialog.FileName);
            _pathTextBox.Text = result.ToString();
        }

        public Uri Path
        {
            get {  return new Uri(_pathTextBox.Text); }
        }

        public KeePassLib.Security.ProtectedString Password
        {
            get { return _passwordSecureTextBoxEx.TextEx; }
        }

        private void OnPasswordTextChanged(object sender, EventArgs e)
        {
            _createButton.Enabled = _passwordControler.ValidateData(false);
        }

        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            _passwordControler.Release();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            KeePass.UI.UIUtil.ConfigureToolTip(_passwordToolTip);
        }
    }
}
