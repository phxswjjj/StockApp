using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class FrmLoading : Form
    {
        private List<Task<string>> Tasks = new List<Task<string>>();
        private Dictionary<int, string> TaskNames = new Dictionary<int, string>();
        private readonly Stopwatch Stopwatch;

        public int MinimizeMilliseconds { get; set; } = 1000;
        public bool IsCompleted { get; set; } = false;

        public FrmLoading()
        {
            InitializeComponent();

            var sw = new System.Diagnostics.Stopwatch(); ;
            sw.Start();
            this.Stopwatch = sw;
        }

        private void FrmLoading_Load(object sender, EventArgs e)
        {
            this.Start();
        }
        private void FrmLoading_FormClosing(object sender, FormClosingEventArgs e)
        {
            var sw = this.Stopwatch;
            while (sw.ElapsedMilliseconds < this.MinimizeMilliseconds)
                System.Threading.Thread.Sleep(100);
        }

        public bool Start(int milliseconds = 500)
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();

            return Task.WaitAll(this.Tasks.ToArray(), milliseconds);
        }

        internal Task<TResult> AddTask<TResult>(string name, Func<TResult> doSomething)
        {
            var task = Task.Factory.StartNew(doSomething);
            var taskCompletedNaming = task.ContinueWith<string>((t) => name);
            this.Tasks.Add(taskCompletedNaming);
            this.TaskNames.Add(taskCompletedNaming.GetHashCode(), name);
            return task;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var total = this.Tasks.Count;

            for (var i = 1; i <= this.Tasks.Count; i++)
            {
                var waitTasks = this.Tasks.Where(t => !t.IsCompleted)
                    .ToArray();
                if (waitTasks.Length == 0)
                    break;

                Task<string> task;
                if (waitTasks.Length < total - i + 1)
                    task = Task.FromResult("??");
                else
                {
                    task = Task.Factory
                        .ContinueWhenAny(waitTasks, (t) => t.Result);
                }
                var info = new ProgressInfo()
                {
                    CompletedJobName = task.Result,
                };
                var waitTask = waitTasks.FirstOrDefault(t => !t.IsCompleted);
                if (waitTask != null)
                    info.WaitJobName = this.TaskNames[waitTask.GetHashCode()];

                var percent = (decimal)i / total * 100;
                backgroundWorker1.ReportProgress((int)percent, info);
            }
            backgroundWorker1.ReportProgress(100, new ProgressInfo() { CompletedJobName = "All" });
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var info = (ProgressInfo)e.UserState;
            var percent = e.ProgressPercentage;
            progressBar1.Value = percent;
            var waitInfo = "";
            if (!string.IsNullOrEmpty(info.WaitJobName))
                waitInfo = $", now wait {info.WaitJobName}";
            this.Text = $"{info.CompletedJobName} completed({percent}%){waitInfo}";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.IsCompleted = true;
            //keep
            progressBar1.Value = 99;
            this.Close();
        }

        private class ProgressInfo
        {
            public string CompletedJobName { get; set; }
            public string WaitJobName { get; set; }
        }
    }
}
