using StockApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class FrmEditGroup : Form
    {
        internal DisplayModel RefData { get; }

        private readonly List<CustomGroup> CustomGroups;

        public FrmEditGroup()
        {
            InitializeComponent();
        }
        internal FrmEditGroup(DisplayModel data, List<CustomGroup> customGroups) : this()
        {
            this.Text = $"{data.ComCode} {data.ComName}";

            this.RefData = data;
            this.CustomGroups = customGroups;

            this.Init(data, customGroups);
        }

        private void Init(DisplayModel data, List<CustomGroup> customGroups)
        {
            var padding = new Padding(21, 3, 3, 3);
            foreach (var group in customGroups)
            {
                var cbx = new CheckBox
                {
                    Appearance = Appearance.Button,
                    Text = group.Name,
                    Checked = group.ComCodes.Contains(data.ComCode),
                    Enabled = group.IsFavorite,
                    BackgroundImageLayout = ImageLayout.None,
                    Height = 26,
                    Padding = padding,
                    Width = 140,
                };
                if (group.ComCodes.Contains(data.ComCode))
                    cbx.Checked = true;
                CheckBox_CheckedChanged(cbx, null);
                cbx.CheckedChanged += CheckBox_CheckedChanged;
                flowLayoutPanel1.Controls.Add(cbx);
            }

            var filtedGroups = customGroups.Where(g => g.ComCodes.Contains(data.ComCode));
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var cbx = (CheckBox)sender;
            if (cbx.Checked)
                cbx.BackgroundImage = Resources.checked_checkbox;
            else
                cbx.BackgroundImage = Resources.unchecked_checkbox;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var customGroups = this.CustomGroups;
            var data = this.RefData;
            foreach (CheckBox cbx in flowLayoutPanel1.Controls)
            {
                var name = cbx.Text;
                var group = customGroups.FirstOrDefault(g => g.Name == name);
                if (group == null)
                {
                    group = new CustomGroup()
                    {
                        Name = name,
                    };
                    customGroups.Add(group);
                }
                if (group.ComCodes.Contains(data.ComCode) && !cbx.Checked)
                    group.ComCodes.Remove(data.ComCode);
                else if (!group.ComCodes.Contains(data.ComCode) && cbx.Checked)
                    group.ComCodes.Add(data.ComCode);
            }
            CustomGroup.Store(customGroups);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
