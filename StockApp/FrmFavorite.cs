using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class FrmFavorite : Form
    {
        public FormModeType FormMode { get; private set; } = FormModeType.Favorite;

        public string[] ViewCodes { get; private set; }

        public FrmFavorite()
        {
            InitializeComponent();
        }
        public DialogResult ShowFavoriteDialog(IWin32Window owner, List<string> codes)
        {
            this.ViewCodes = codes.ToArray();
            return this.ShowDialog(owner);
        }
        public DialogResult ShowHateDialog(IWin32Window owner, List<string> codes)
        {
            this.FormMode = FormModeType.Hate;
            this.Text = "Hate";
            this.ViewCodes = codes.ToArray();
            return this.ShowDialog(owner);
        }

        private void FrmFavorite_Load(object sender, EventArgs e)
        {
            txtFavorite.Text = string.Join(Environment.NewLine, this.ViewCodes);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var list = txtFavorite.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            this.ViewCodes = list.ToArray();

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var list = txtFavorite.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            this.ViewCodes = list.ToArray();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public enum FormModeType
        {
            Favorite,
            Hate,
        }
    }
}
