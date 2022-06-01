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
        public const string FavoriteFilePath = "Favorite.json";
        public const string HateFilePath = "Hate.json";

        public string FilePath => this.FormMode == FormModeType.Favorite ? FavoriteFilePath : HateFilePath;

        public FormModeType FormMode { get; private set; } = FormModeType.Favorite;

        public FrmFavorite()
        {
            InitializeComponent();
        }
        public DialogResult ShowHateDialog(IWin32Window owner)
        {
            this.FormMode = FormModeType.Hate;
            this.Text = "Hate";
            return this.ShowDialog(owner);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var list = txtFavorite.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            JsonCache.Store(this.FilePath, list);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmFavorite_Load(object sender, EventArgs e)
        {
            if (File.Exists(this.FilePath))
            {
                var list = JsonCache.Load<List<string>>(this.FilePath);
                txtFavorite.Text = string.Join(Environment.NewLine, list.ToArray());
            }
        }

        public enum FormModeType
        {
            Favorite,
            Hate,
        }
    }
}
