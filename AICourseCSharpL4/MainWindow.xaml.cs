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
using Newtonsoft.Json.Linq;

namespace AICourseCSharpL4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Baidu.Aip.Face.Face mClient;
        private IniFile mConfig;
        private Dictionary<string, string> mDictionary;
        private string m_imgPath1 = "";
        private string m_imgPath2 = "";

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
            mDictionary = new Dictionary<string, string>();
            mDictionary.Add("male", "女性");
            mDictionary.Add("female", "女性");
        }

        void initClient()
        {
            var APP_ID = mConfig.IniReadValue("Baidu", "APP_ID").ToString();
            var API_KEY = mConfig.IniReadValue("Baidu", "API_KEY").ToString();
            ;
            var SECRET_KEY = mConfig.IniReadValue("Baidu", "SECRET_KEY").ToString();
            Console.WriteLine(API_KEY);
            Console.WriteLine(SECRET_KEY);
            mClient = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            mClient.Timeout = 60000; // 修改超时时间
        }

        void initGui()
        {
        }

        void initDict()
        {
        }

        private void button_recongnition2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "选择识别语音文件";
                //fdlg.InitialDirectory = @"c:\";
                fdlg.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                fdlg.FilterIndex = 0;
                fdlg.RestoreDirectory = false;
                if (fdlg.ShowDialog() == true)
                {
                    m_imgPath2 = fdlg.FileName;
                }

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(m_imgPath2, UriKind.RelativeOrAbsolute);
                bi.EndInit();
                image_recongnition2.Source = bi;

                string image = Convert.ToBase64String(File.ReadAllBytes(m_imgPath2));


                // 调用人脸检测，可能会抛出网络等异常，请使用try/catch捕获
                // 如果有可选参数
                var options = new Dictionary<string, object>
                {
                    {"face_field", "beauty,age,expression,face_shape,gender,race"},
                };


                // 带参数调用人脸检测
                var result = mClient.Detect(image, "BASE64", options);
                var list_faceinfo = new List<string>();
                var faces = result["result"]["face_list"];
                foreach (var face in faces)
                {
                    var age = (int) face["age"];
                    list_faceinfo.Add("年龄：" + age);
                    var beauty = (string) face["beauty"];
                    list_faceinfo.Add("样貌评分：" + beauty);
                    var expression = (string) face["expression"]["type"];
                    var expression_pb = (string) face["expression"]["probability"];
                    list_faceinfo.Add("表情：" + expression + ",置信度：" + expression_pb);
                    var face_shape = (string) face["face_shape"]["type"];
                    var face_shape_pb = (string) face["face_shape"]["probability"];
                    list_faceinfo.Add("脸型：" + face_shape + ",置信度：" + face_shape_pb);
                    var gender = (string) face["gender"]["type"];
                    var gender_pb = (string) face["gender"]["probability"];
                    list_faceinfo.Add("性别：" + gender + ",置信度：" + gender_pb);
                    var race = (string) face["race"]["type"];
                    var race_pb = (string) face["race"]["probability"];
                    list_faceinfo.Add("种族：" + race + ",置信度：" + race_pb);
                }

                textBox_recongnition2.Text = string.Join("\n", list_faceinfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "错误");
            }
        }

        private void button_recongnition1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "选择识别语音文件";
                //fdlg.InitialDirectory = @"c:\";
                fdlg.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                fdlg.FilterIndex = 0;
                fdlg.RestoreDirectory = false;
                if (fdlg.ShowDialog() == true)
                {
                    m_imgPath1 = fdlg.FileName;
                }

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(m_imgPath1, UriKind.RelativeOrAbsolute);
                bi.EndInit();
                image_recongnition1.Source = bi;
                string image = Convert.ToBase64String(File.ReadAllBytes(m_imgPath1));


                // 调用人脸检测，可能会抛出网络等异常，请使用try/catch捕获
                // 如果有可选参数
                var options = new Dictionary<string, object>
                {
                    {"face_field", "beauty,age,expression,face_shape,gender,race"},
                };


                // 带参数调用人脸检测
                var result = mClient.Detect(image, "BASE64", options);

                var list_faceinfo = new List<string>();
                var faces = result["result"]["face_list"];
                foreach (var face in faces)
                {
                    var age = (int) face["age"];
                    list_faceinfo.Add("年龄：" + age);
                    var beauty = (string) face["beauty"];
                    list_faceinfo.Add("样貌评分：" + beauty);
                    var expression = (string) face["expression"]["type"];
                    var expression_pb = (string) face["expression"]["probability"];
                    list_faceinfo.Add("表情：" + expression + ",置信度：" + expression_pb);
                    var face_shape = (string) face["face_shape"]["type"];
                    var face_shape_pb = (string) face["face_shape"]["probability"];
                    list_faceinfo.Add("脸型：" + face_shape + ",置信度：" + face_shape_pb);
                    var gender = (string) face["gender"]["type"];
                    var gender_pb = (string) face["gender"]["probability"];
                    list_faceinfo.Add("性别：" + gender + ",置信度：" + gender_pb);
                    var race = (string) face["race"]["type"];
                    var race_pb = (string) face["race"]["probability"];
                    list_faceinfo.Add("种族：" + race + ",置信度：" + race_pb);
                }

                textBox_recongnition1.Text = string.Join("\n", list_faceinfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "错误");
            }
        }

        private void button_diff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var faces = new JArray
                {
                    new JObject
                    {
                        {"image", ReadImg(m_imgPath1)},
                        {"image_type", "BASE64"},
                        {"face_type", "LIVE"},
                        {"quality_control", "LOW"},
                        {"liveness_control", "NONE"},
                    },
                    new JObject
                    {
                        {"image", ReadImg(m_imgPath2)},
                        {"image_type", "BASE64"},
                        {"face_type", "LIVE"},
                        {"quality_control", "LOW"},
                        {"liveness_control", "NONE"},
                    }
                };

                var result = mClient.Match(faces);
                var score = (float) result["result"]["score"];
                var list_faceinfo = new List<string>();
                if (score > 50)
                {
                    list_faceinfo.Add("像");
                }
                else
                {
                    list_faceinfo.Add("不像");
                }

                list_faceinfo.Add(String.Format("得分：{0}", score));
                textBox_diff.Text = string.Join("\n", list_faceinfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "错误");
            }
        }

        public string ReadImg(string img)
        {
            return Convert.ToBase64String(File.ReadAllBytes(img));
        }
    }
}