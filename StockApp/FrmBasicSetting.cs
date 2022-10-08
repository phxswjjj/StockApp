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
    public partial class FrmBasicSetting : Form
    {
        public FrmBasicSetting()
        {
            InitializeComponent();

            cbxKDJRange.Items.Clear();
            foreach (DisplayModel.KDJRangeType range in Enum.GetValues(typeof(DisplayModel.KDJRangeType)))
            {
                cbxKDJRange.Items.Add(range);
            }
        }

        private void FrmBasicSetting_Load(object sender, EventArgs e)
        {
            var setting = BasicSetting.Instance;
            txtPriceLimit.Text = setting.PriceLimit.ToString();
            txtContBonusTimes.Text = setting.ContBonusTimesLimit.ToString();
            txtSimulateMonths.Text = setting.SimulateMaxMonths.ToString();
            numTraceDays.Value = setting.TraceDays;

            cbxKDJRange.SelectedIndex = (int)setting.KDJRange;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var setting = Properties.Settings.Default;

            decimal price;
            if (!decimal.TryParse(txtPriceLimit.Text, out price))
            {
                MessageBox.Show($"非數值: {txtPriceLimit.Text}");
                txtPriceLimit.Focus();
                return;
            }
            setting.PriceLimit = price;

            int times;
            if (!int.TryParse(txtContBonusTimes.Text, out times))
            {
                MessageBox.Show($"非數值: {txtContBonusTimes.Text}");
                txtContBonusTimes.Focus();
                return;
            }
            setting.ContBonusTimesLimit = times;

            int months;
            if (!int.TryParse(txtSimulateMonths.Text, out months))
            {
                MessageBox.Show($"非數值: {txtSimulateMonths.Text}");
                txtSimulateMonths.Focus();
                return;
            }
            setting.SimulateMaxMonth = months;
            setting.KDJRange = cbxKDJRange.SelectedIndex;

            setting.TraceDays = (int)numTraceDays.Value;

            setting.Save();
            BasicSetting.Instance.Load();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
