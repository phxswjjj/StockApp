using System;
using System.Collections.Concurrent;
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
        private ConcurrentBag<ITaskInfo> Tasks = new ConcurrentBag<ITaskInfo>();
        private readonly Stopwatch Stopwatch;

        public int MinimizeMilliseconds { get; set; } = 1000;
        public bool IsCompleted { get; set; } = false;
        private bool IsRunning = false;

        public FrmLoading()
        {
            InitializeComponent();

            var sw = new System.Diagnostics.Stopwatch(); ;
            sw.Start();
            this.Stopwatch = sw;
        }

        private void FrmLoading_Load(object sender, EventArgs e)
        {
            if (!this.IsRunning)
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
            this.IsRunning = true;
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();

            return Task.WaitAll(this.Tasks.Select(j => j.Instance).ToArray(), milliseconds);
        }

        internal Task<TResult> AddTask<TResult>(string name, Func<TResult> doSomething)
        {
            var job = new LoadingTask<TResult>(name, doSomething);
            job.Start();
            this.Tasks.Add(job);

            return job;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var total = this.Tasks.Count;

            var waitTasks = this.Tasks.Where(j => !j.IsCompleted);
            for (var i = 0; i <= total; i++)
            {
                if (waitTasks == null || !waitTasks.Any())
                    break;

                var completedJob = Task.WhenAny(waitTasks.Select(j => j.Instance).ToArray()).Result as ITaskInfo;
                if (completedJob != null)
                {
                    var info = new ProgressInfo()
                    {
                        CompletedJobName = completedJob.Name,
                    };

                    var completed = this.Tasks.Count(j => j.IsCompleted);
                    var percent = (decimal)completed / total * 100;
                    backgroundWorker1.ReportProgress((int)percent, info);

                    waitTasks = waitTasks.Where(j => !j.IsCompleted);
                }
                else
                    break;
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
        private interface ITaskInfo
        {
            Task Instance { get; }
            string Name { get; }
            bool IsCompleted { get; }
        }
        private class LoadingTask<TResult> : Task<TResult>, ITaskInfo
        {
            public LoadingTask(string jobName, Func<TResult> function) : base(function)
            {
                this.Name = jobName;
            }

            public Task Instance => this;

            public string Name { get; }
        }
    }
}
