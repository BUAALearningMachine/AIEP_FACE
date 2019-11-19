using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace AICourseCSharpL4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Baidu.Aip.Face.Face mClient;
        private IniFile mConfig;
        public MainWindow()
        {
            InitializeComponent();
            init();
        }

        void init()
        {
            initConfig();
            initClient();
        }

        void initConfig()
        {
            mConfig = new IniFile("./cfg.ini");
        }

        void initClient()
        { 
            var APP_ID = mConfig.IniReadValue("Baidu", "APP_ID").ToString(); 
            var API_KEY = mConfig.IniReadValue("Baidu", "API_KEY").ToString();;
            var SECRET_KEY = mConfig.IniReadValue("Baidu", "SECRET_KEY").ToString();
            Console.WriteLine(API_KEY);
            Console.WriteLine(SECRET_KEY);
            mClient = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            mClient.Timeout = 60000;  // 修改超时时间
        }

        void initGui()
        {

        }

        void initDict()
        {

        }

        private void button_recongnition2_Click(object sender, RoutedEventArgs e)
        {
            string pathImg = "";
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "选择识别语音文件";
            //fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            fdlg.FilterIndex = 0;
            fdlg.RestoreDirectory = false;
            if (fdlg.ShowDialog() == true)
            {
                pathImg = fdlg.FileName;
            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(pathImg, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            image_recongnition2.Source = bi;

            string image = Convert.ToBase64String(File.ReadAllBytes(pathImg));


            // 调用人脸检测，可能会抛出网络等异常，请使用try/catch捕获
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"face_field", "beauty,age,expression,face_shape,gender,race"},
            };


            // 带参数调用人脸检测
            var result = mClient.Detect(image, "BASE64", options);
            textBox_recongnition2.Text = result.ToString();

        }

        private void button_recongnition1_Click(object sender, RoutedEventArgs e)
        {
            string pathImg = "";
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "选择识别语音文件";
            //fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            fdlg.FilterIndex = 0;
            fdlg.RestoreDirectory = false;
            if (fdlg.ShowDialog() == true)
            {
                 pathImg = fdlg.FileName;
            }
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(pathImg, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            image_recongnition1.Source = bi;
            string image = Convert.ToBase64String(File.ReadAllBytes(pathImg));


            // 调用人脸检测，可能会抛出网络等异常，请使用try/catch捕获
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"face_field", "beauty,age,expression,face_shape,gender,race"},
            };


            // 带参数调用人脸检测
            var result = mClient.Detect(image,"BASE64",options);
            textBox_recongnition1.Text = result.ToString();
        }

        private void button_diff_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
