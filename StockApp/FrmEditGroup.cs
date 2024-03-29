﻿using LiteDB;
using Serilog;
using StockApp.Data;
using StockApp.Group;
using StockApp.Properties;
using StockApp.Utility;
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
using Unity;

namespace StockApp
{
    public partial class FrmEditGroup : Form
    {
        const int GroupControlHeight = 26;
        const int GroupControlWidth = 140;

        internal DisplayModel RefData { get; }
        public Button EmptyButton { get; }

        private TextBox txtAddGroup;

        public FrmEditGroup()
        {
            InitializeComponent();

            //避免 enter 聲音
            this.EmptyButton = new Button()
            {
                Name = "EmptyButton",
            };
            this.AcceptButton = EmptyButton;
        }
        internal FrmEditGroup(DisplayModel data) : this()
        {
            this.Text = $"{data.ComCode} {data.ComName}";

            this.RefData = data;

            this.Init(data);
        }

        private void Init(DisplayModel data)
        {
            List<CustomGroup> customGroups;
            var container = UnityHelper.Create();
            using (var conn = LocalDb.CreateSqlLite())
            {
                container.RegisterInstance(conn);
                var custGroupRepo = container.Resolve<CustomGroupRepository>();
                customGroups = custGroupRepo.GetAll();
            }

            customGroups.Sort((x, y) => x.SortIndex.CompareTo(y.SortIndex));
            foreach (var group in customGroups)
            {
                var cbx = CreateCheckBox(group.Name);
                cbx.Checked = group.ComCodes.Contains(data.ComCode);
                cbx.Enabled = group.IsFavorite;
                if (group.ComCodes.Contains(data.ComCode))
                    cbx.Checked = true;
                CheckBox_CheckedChanged(cbx, null);
                flowLayoutPanel1.Controls.Add(cbx);
            }

            txtAddGroup = new TextBox()
            {
                Name = "txtAddGroup",
                Height = GroupControlHeight,
                Width = GroupControlWidth,
            };
            flowLayoutPanel1.Controls.Add(txtAddGroup);

            txtAddGroup.KeyUp += TxtAddGroup_KeyUp;
        }

        private void FrmEditGroup_Load(object sender, EventArgs e)
        {
            txtAddGroup.Select();
        }

        private void TxtAddGroup_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            var textbox = (TextBox)sender;
            var groupName = textbox.Text.Trim();
            if (string.IsNullOrEmpty(groupName))
                return;

            foreach (var ctrl in flowLayoutPanel1.Controls)
            {
                if (!(ctrl is CheckBox cbx))
                    continue;

                if (cbx.Text == groupName)
                {
                    MessageBox.Show(this, $"{groupName} exists");
                    txtAddGroup.Text = "";
                    return;
                }
            }

            var newCbx = CreateCheckBox(groupName);
            newCbx.Checked = true;
            CheckBox_CheckedChanged(newCbx, null);
            flowLayoutPanel1.Controls.Add(newCbx);
            textbox.Text = "";
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
            var data = this.RefData;

            var container = UnityHelper.Create();
            List<CustomGroup> customGroups;
            using (var conn = LocalDb.CreateSqlLite())
            {
                container.RegisterInstance(conn);
                var custGroupRepo = container.Resolve<CustomGroupRepository>();
                customGroups = custGroupRepo.GetAll();
            }

            var changedGroups = new List<CustomGroup>();
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (!(ctrl is CheckBox cbx))
                    continue;
                var name = cbx.Text;
                var group = customGroups.FirstOrDefault(g => g.Name == name);
                if (group == null)
                {
                    group = new CustomGroup()
                    {
                        Name = name,
                    };
                    changedGroups.Add(group);
                }
                if (group.ComCodes.Contains(data.ComCode) && !cbx.Checked)
                {
                    group.ComCodes.Remove(data.ComCode);
                    changedGroups.Add(group);
                }
                else if (!group.ComCodes.Contains(data.ComCode) && cbx.Checked)
                {
                    group.ComCodes.Add(data.ComCode);
                    changedGroups.Add(group);
                }
            }

            if (changedGroups.Count > 0)
            {
                using (var conn = LocalDb.CreateSqlLite())
                {
                    container.RegisterInstance(conn);
                    var custGroupRepo = container.Resolve<CustomGroupRepository>();
                    var logger = container.Resolve<ILogger>()
                        .ForContext("class", this.GetType())
                        .ForContext("event", nameof(BtnSave_Click));

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            custGroupRepo.Updates(changedGroups);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, ex.Message);
                            trans.Rollback();
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private CheckBox CreateCheckBox(string name)
        {
            var padding = new Padding(21, 3, 3, 3);
            var cbx = new CheckBox
            {
                Appearance = Appearance.Button,
                Text = name,
                BackgroundImageLayout = ImageLayout.None,
                Padding = padding,
                Height = GroupControlHeight,
                Width = GroupControlWidth,
            };
            cbx.CheckedChanged += CheckBox_CheckedChanged;
            return cbx;
        }
    }
}
