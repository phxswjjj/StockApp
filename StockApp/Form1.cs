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
    public partial class Form1 : Form
    {

        string[] AssignComCodes = new string[] { };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            List<string> assignComCodes;
            if (File.Exists(FrmFavorite.FilePath))
                assignComCodes = JsonCache.Load<List<string>>(FrmFavorite.FilePath);
            else
                assignComCodes = new List<string>();

            AssignComCodes = assignComCodes.ToArray();

            var list = CompanyAvgBonus.GetAll();
            list.Sort(new CompanyAvgBonus.Expect7DiffComparer());

            var list2 = list
                .Where(l => l.CurrentPrice < 60)
                .Take(100).ToList();
            foreach (var code in assignComCodes)
            {
                if (list2.Any(l => l.ComCode == code))
                    continue;
                var data = list.Find(l => l.ComCode == code);
                if (data == null)
                    continue;
                list2.Add(data);
            }
            list2.Sort(new CompanyAvgBonus.Expect7DiffComparer());

            var binding = new BindingList<CompanyAvgBonus>(list2);
            dataGridView1.DataSource = binding;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow grow in dataGridView1.Rows)
                RefreshCellStyle(grow);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grow = dataGridView1.Rows[e.RowIndex];

            RefreshCellStyle(grow);

            dataGridView1.Refresh();
        }

        private void RefreshCellStyle(DataGridViewRow grow)
        {
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            if (AssignComCodes.Contains(data.ComCode))
                grow.DefaultCellStyle.BackColor = Color.LightYellow;

            var defaultForeColor = grow.Cells[nameof(CompanyAvgBonus.ComCode)].Style.ForeColor;

            grow.Cells[nameof(CompanyAvgBonus.Expect5)].Style.ForeColor = defaultForeColor;
            grow.Cells[nameof(CompanyAvgBonus.Expect7)].Style.ForeColor = defaultForeColor;
            grow.Cells[nameof(CompanyAvgBonus.Expect9)].Style.ForeColor = defaultForeColor;

            if (data.CurrentPrice < data.Expect9)
                grow.Cells[nameof(CompanyAvgBonus.Expect9)].Style.ForeColor = Color.Red;
            else if (data.CurrentPrice < data.Expect7)
                grow.Cells[nameof(CompanyAvgBonus.Expect7)].Style.ForeColor = Color.Red;
            else if (data.CurrentPrice < data.Expect5)
                grow.Cells[nameof(CompanyAvgBonus.Expect5)].Style.ForeColor = Color.Red;
        }

        private void 觀察清單ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new FrmFavorite();
            if (editor.ShowDialog(this) == DialogResult.OK)
            {
                LoadData();
            }
        }
    }
}
