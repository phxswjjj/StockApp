﻿using System;
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
        }

        private void FrmBasicSetting_Load(object sender, EventArgs e)
        {
            var setting = BasicSetting.Instance;
            txtPriceLimit.Text = setting.PriceLimit.ToString();
            txtContBonusTimes.Text = setting.ContBonusTimesLimit.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal price;
            if (!decimal.TryParse(txtPriceLimit.Text, out price))
            {
                MessageBox.Show($"非數值: {txtPriceLimit.Text}");
                txtPriceLimit.Focus();
                return;
            }
            Properties.Settings.Default.PriceLimit = price;

            int times;
            if (!int.TryParse(txtContBonusTimes.Text, out times))
            {
                MessageBox.Show($"非數值: {txtContBonusTimes.Text}");
                txtContBonusTimes.Focus();
                return;
            }
            Properties.Settings.Default.ContBonusTimesLimit = times;

            Properties.Settings.Default.Save();
            BasicSetting.Instance.Load();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
