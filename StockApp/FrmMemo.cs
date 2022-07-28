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
        private MemoType DataType { get; set; } = MemoType.Memo;

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

        public void ShowTrace()
        {
            DataType = MemoType.Trace;
            lblHoldStock.Visible = false;
            txtHoldStock.Visible = false;

            lblHoldValue.Text = "追蹤價格";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            IMemoContent data;

            if (DataType == MemoType.Trace)
                data = new TraceMemoContent(this.RefData);
            else
                data = new MemoContent(this.RefData);

            var sHoldStock = txtHoldStock.Text.Trim();
            var sHoldValue = txtHoldValue.Text.Trim();

            int iHoldStock;
            decimal iHoldValue;

            if (string.IsNullOrEmpty(sHoldStock))
                data.Stock = null;
            else if (!int.TryParse(sHoldStock, out iHoldStock))
            {
                MessageBox.Show($"非數值: {sHoldStock}");
                txtHoldStock.Focus();
                return;
            }
            else
                data.Stock = iHoldStock;

            if (string.IsNullOrEmpty(sHoldValue))
                data.Value = null;
            else if (!decimal.TryParse(sHoldValue, out iHoldValue))
            {
                MessageBox.Show($"非數值: {sHoldValue}");
                txtHoldValue.Focus();
                return;
            }
            else
                data.Value = iHoldValue;

            data.Update();

            this.RefData.SetExtra(data);
            this.DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            IMemoContent data;

            if (DataType == MemoType.Trace)
                data = new TraceMemoContent(this.RefData);
            else
                data = new MemoContent(this.RefData);

            data.Stock = null;
            data.Value = null;
            data.Remove();

            this.RefData.SetExtra(data);
            this.DialogResult = DialogResult.OK;
        }

        private enum MemoType
        {
            Memo,
            Trace,
        }
    }
}
