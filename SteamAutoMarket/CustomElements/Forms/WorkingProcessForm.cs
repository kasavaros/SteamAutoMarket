﻿namespace SteamAutoMarket.CustomElements.Forms
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    using SteamAutoMarket.CustomElements.Utils;
    using SteamAutoMarket.Utils;

    public partial class WorkingProcessForm : Form
    {
        private static Thread workingThread;

        private bool invokedFromStopButton;

        public WorkingProcessForm()
        {
            this.InitializeComponent();
        }

        public static void InitProcess(Action process)
        {
            Dispatcher.AsMainForm(() => { Program.WorkingProcessForm = new WorkingProcessForm(); });
            Program.WorkingProcessForm.StartProcess(process);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AppendWorkingProcessInfo(string message)
        {
            try
            {
                Dispatcher.AsWorkingProcessForm(
                    () =>
                        {
                            this.ClearLogBox();

                            this.AppendLog(Logger.GetCurrentDate() + @" - " + message + Environment.NewLine);

                            Logger.Working(message);
                        });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        public void AppendLog(string text)
        {
            try
            {
                var autoScroll = this.ScrollCheckBox.Checked;

                if (autoScroll)
                {
                    this.LogTextBox.AppendText(text);
                }
                else
                {
                    var selectionStart = this.LogTextBox.SelectionStart;
                    var selectionLength = this.LogTextBox.SelectionLength;
                    this.LogTextBox.Text += text;
                    this.LogTextBox.SelectionStart = selectionStart;
                    this.LogTextBox.SelectionLength = selectionLength;
                }
            }
            catch (ObjectDisposedException)
            {
                // ignored
            }
        }

        private void ClearLogBox()
        {
            if (this.LogTextBox.Lines.Length <= 1000)
            {
                return;
            }

            var list = this.LogTextBox.Lines.ToList();
            list.RemoveRange(0, 500);
            this.LogTextBox.Lines = list.ToArray();
        }

        private void WorkingProcessFormLoad(object sender, EventArgs e)
        {
            this.AppendWorkingProcessInfo("Working process started.");
        }

        private void DeactivateForm()
        {
            Dispatcher.AsWorkingProcessForm(this.Close);
        }

        private void StopWorkingProcessButtonClick(object sender, EventArgs e)
        {
            this.invokedFromStopButton = true;
            Dispatcher.AsMainForm(
                () =>
                    {
                        workingThread.Abort();
                        this.DeactivateForm();
                    });
        }

        private void WorkingProcessFormFormClosing(object sender, FormClosingEventArgs e)
        {
            // if invoked from [X] button
            if (this.invokedFromStopButton == false)
            {
                this.StopWorkingProcessButtonClick(sender, e);
            }
        }

        private void StartProcess(Action process)
        {
            Dispatcher.AsMainForm(
                () =>
                    {
                        this.Show();
                        workingThread = new Thread(
                            () =>
                                {
                                    process();
                                    this.DeactivateForm();
                                });
                        workingThread.Start();
                    });
        }
    }
}