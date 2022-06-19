
namespace StockApp
{
    partial class FrmSimulator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.numStartVolume = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numDownRate = new System.Windows.Forms.NumericUpDown();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numStartPrice = new System.Windows.Forms.NumericUpDown();
            this.cbxExDividend = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDownRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 98);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Legend = "Legend1";
            series1.Name = "StockSeries";
            series1.YValuesPerPoint = 4;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(776, 340);
            this.chart1.TabIndex = 0;
            this.chart1.TabStop = false;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始日期";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(78, 9);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker1.TabIndex = 3;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "投入股數";
            // 
            // numStartVolume
            // 
            this.numStartVolume.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numStartVolume.Location = new System.Drawing.Point(78, 42);
            this.numStartVolume.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numStartVolume.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numStartVolume.Name = "numStartVolume";
            this.numStartVolume.Size = new System.Drawing.Size(120, 22);
            this.numStartVolume.TabIndex = 4;
            this.numStartVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartVolume.ThousandsSeparator = true;
            this.numStartVolume.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numStartVolume.Enter += new System.EventHandler(this.numStartVolume_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "跌幅追加(%)";
            // 
            // numDownRate
            // 
            this.numDownRate.Location = new System.Drawing.Point(78, 70);
            this.numDownRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDownRate.Name = "numDownRate";
            this.numDownRate.Size = new System.Drawing.Size(120, 22);
            this.numDownRate.TabIndex = 7;
            this.numDownRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDownRate.ThousandsSeparator = true;
            this.numDownRate.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numDownRate.Enter += new System.EventHandler(this.numStartVolume_Enter);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(411, 45);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(81, 39);
            this.btnExecute.TabIndex = 8;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "價格";
            // 
            // numStartPrice
            // 
            this.numStartPrice.DecimalPlaces = 2;
            this.numStartPrice.Location = new System.Drawing.Point(298, 42);
            this.numStartPrice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numStartPrice.Name = "numStartPrice";
            this.numStartPrice.Size = new System.Drawing.Size(83, 22);
            this.numStartPrice.TabIndex = 5;
            this.numStartPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartPrice.ThousandsSeparator = true;
            this.numStartPrice.Enter += new System.EventHandler(this.numStartVolume_Enter);
            // 
            // cbxExDividend
            // 
            this.cbxExDividend.AutoSize = true;
            this.cbxExDividend.Checked = true;
            this.cbxExDividend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxExDividend.Location = new System.Drawing.Point(298, 72);
            this.cbxExDividend.Name = "cbxExDividend";
            this.cbxExDividend.Size = new System.Drawing.Size(72, 16);
            this.cbxExDividend.TabIndex = 9;
            this.cbxExDividend.Text = "含除權息";
            this.cbxExDividend.UseVisualStyleBackColor = true;
            // 
            // FrmSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbxExDividend);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.numDownRate);
            this.Controls.Add(this.numStartPrice);
            this.Controls.Add(this.numStartVolume);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chart1);
            this.Name = "FrmSimulator";
            this.Text = "FrmSimulator";
            this.Load += new System.EventHandler(this.FrmSimulator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDownRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numStartVolume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numDownRate;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numStartPrice;
        private System.Windows.Forms.CheckBox cbxExDividend;
    }
}