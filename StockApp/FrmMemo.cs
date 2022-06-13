using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class FrmMemo : Form
    {
        internal DisplayModel RefData { get; }

        private FrmMemo()
        {
            InitializeComponent();
        }
        internal FrmMemo(DisplayModel data) : this()
        {
            this.Text = $"{data.ComCode} {data.ComName}";

            this.RefData = data;

            this.Init(data);
        }

        private void Init(DisplayModel data)
        {
            if (data.HoldStock.HasValue)
                txtHoldStock.Text = data.HoldStock.Value.ToString();
            if (data.HoldValue.HasValue)
                txtHoldValue.Text = data.HoldValue.Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var data = new MemoContent(this.RefData);

            var sHoldStock = txtHoldStock.Text.Trim();
            var sHoldValue = txtHoldValue.Text.Trim();

            int iHoldStock;
            decimal iHoldValue;

            if (string.IsNullOrEmpty(sHoldStock))
                data.HoldStock = null;
            else if (!int.TryParse(sHoldStock, out iHoldStock))
            {
                MessageBox.Show($"非數值: {sHoldStock}");
                txtHoldStock.Focus();
                return;
            }
            else
                data.HoldStock = iHoldStock;

            if (string.IsNullOrEmpty(sHoldValue))
                data.HoldValue = null;
            else if (!decimal.TryParse(sHoldValue, out iHoldValue))
            {
                MessageBox.Show($"非數值: {sHoldValue}");
                txtHoldValue.Focus();
                return;
            }
            else
                data.HoldValue = iHoldValue;

            MemoContent.Update(data);

            this.RefData.SetExtra(data);
            this.DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var data = new MemoContent(this.RefData);
            data.HoldStock = null;
            data.HoldValue = null;
            MemoContent.Remove(data);

            this.RefData.SetExtra(data);
            this.DialogResult = DialogResult.OK;
        }
    }
}
