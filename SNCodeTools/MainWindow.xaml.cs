using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SNCodeTools
{
    public partial class MainWindow : Window
    {
        //第一次SN
        private string SN_1 = "";

        //清除提示文本
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            //设置窗口居中显示
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //初始化定时器
            timer = new DispatcherTimer();
            //定时器在3秒后执行
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            //第一次输入提示
            txtPrompt.Text = "请输入机台SN：";
        }

        //定时器
        private void Timer_Tick(object sender, EventArgs e)
        {
            //清除消息
            txtResult.Text = "";
            pass.Text = "";
            fn.Text = "";
            fp.Text = "";
            //停止定时器
            timer.Stop();
        }

        //点击保存进行存储
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //获取第二个SN码
            string SN_2 = txtSNInput.Text.Trim();

            //保存文件并根据日期命名
            try
            {
                if (!string.IsNullOrEmpty(SN_1) && SN_2.Length == 3)
                {
                    string fileName = $"{DateTime.Now:yyyy-MM-dd}_SN.txt";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                    //使用append参数设置为true，以便在文件末尾追加内容(不覆盖之前的txt内容)
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(SN_1 + " " + SN_2); //将两个SN码写在同一行上，并使用空格隔开
                    }

                    pass.Text = "PASS";
                    fn.Text = $"fileName：{fileName} \n ";
                    fp.Text = $"filePath：{filePath} \n ";

                    SN_1 = ""; //重置第一个SN码
                    txtSNInput.Clear();
                    txtSNInput.Focus();

                    timer.Start();
                    Console.WriteLine("保存操作已执行"); //添加调试语句，确认保存操作已执行

                    //重置提示文本为"请输入机台SN："
                    txtPrompt.Text = "请输入机台SN：";
                }
                else
                {
                    txtResult.Text = "请输入有效的3位电板序号！";
                    txtResult.Foreground = Brushes.Red; //设置文本颜色为红色
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = $"ERROR：{ex.Message}";
                txtResult.Foreground = Brushes.Red; //设置文本颜色为红色
            }
        }

        //第一次按回车进行第二次输入
        private void txtSNInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string SN_2 = txtSNInput.Text.Trim();
                if (string.IsNullOrEmpty(SN_1))
                {
                    SN_1 = SN_2;
                    txtPrompt.Text = "请输入集电板序号：";
                    txtSNInput.Clear();
                }
                else
                {
                    if (SN_2.Length == 3)
                    {
                        Save_Click(sender, e);
                    }
                    else
                    {
                        txtResult.Text = "请输入有效的3位集电板序号！";
                        txtResult.Foreground = Brushes.Red; //设置文本颜色为红色
                        timer.Start();
                    }
                }
            }
        }
    }
}