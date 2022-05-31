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
        public const string FilePath = "Favorite.json";

        public FrmFavorite()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var list = txtFavorite.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            JsonCache.Store(FilePath, list);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmFavorite_Load(object sender, EventArgs e)
        {
            if (File.Exists(FilePath))
            {
                var list = JsonCache.Load<List<string>>(FilePath);
                txtFavorite.Text = string.Join(Environment.NewLine, list.ToArray());
            }
        }
    }
}
