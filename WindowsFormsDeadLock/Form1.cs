using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsDeadLock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var blahTask = SomeAsyncClass.GetStringAsync1();
            Debug.WriteLine("Form1.button1_click - calling blahTask.Result");
            var result = blahTask.Result;
            Debug.WriteLine($"Form1.button1_Click: {result}");
            label1.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var blahTask = SomeAsyncClass.GetStringAsync2();
            Debug.WriteLine("Form1.button2_click - calling blahTask.Result");
            var result = blahTask.Result;
            Debug.WriteLine($"Form1.button2_Click: {result}");
            label1.Text = result;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Form1.button3_click - calling await SomeAsyncClass.GetStringAsync1");
            var blahText = await SomeAsyncClass.GetStringAsync1();
            Debug.WriteLine($"Form1.button3_Click: {blahText}; because btn3 is async");
            label1.Text = blahText;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Form1.button4_click - calling SomeAsyncClass.GetStringAsync1");
            var blahTask = SomeAsyncClass.GetStringAsync1();
            Debug.WriteLine("blahTask.ConfigureAwait(false)");
            blahTask.ConfigureAwait(false);
            Debug.WriteLine("var result = blahTask.Result;");
            var result = blahTask.Result;
            Debug.WriteLine($"Form1.button4_Click: {result}");
            label1.Text = result;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Form1.button5_click - calling await SomeAsyncClass.GetStringAsync1");
            var blahText = await SomeAsyncClass.GetStringAsync1().ConfigureAwait(false);
            Debug.WriteLine($"Form1.button5_Click: {blahText}; because btn5 is async");
            label1.Text = blahText;
        }
    }
}
