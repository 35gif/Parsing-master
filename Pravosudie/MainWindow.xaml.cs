using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Net;
using OpenQA.Selenium.Interactions;
using System.Reflection;

namespace Pravosudie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int j = 1;
        private Dictionary<string, string> _dict;
        public MainWindow()
        {
            InitializeComponent();
            _dict = new Dictionary<string, string>();
        }

        public async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(UrlTb.Text);
            Thread.Sleep(2000);
            //получение всех папок
            var folder = driver.FindElements(By.ClassName("el-table__row"));
            
            for (int i = 1; i < folder.Count; i++)
            {
                if (System.IO.Directory.Exists("D:\\C#\\Тесты\\Parsing-master\\Готовый результат\\" + i))
                {
                    if (System.IO.Directory.Exists("D:\\C#\\Тесты\\Parsing-master\\Готовый результат\\" + i + " (1)"))
                    {
                        j++;
                    }
                    i++;
                    continue;
                }
                var img = driver.FindElements(By.ClassName("el-table__row"));
                try
                {
                    img[i].Click();
                }
                catch
                {
                    Thread.Sleep(500);
                    img[i].Click();
                }
                System.IO.Directory.CreateDirectory("D:\\C#\\Тесты\\Parsing-master\\Готовый результат\\" + i);

                Thread.Sleep(1000);
                //получить количество изображений в папке
                var image = driver.FindElements(By.ClassName("img-mode-img"));
                Thread.Sleep(3000);
                if (image.Count == 0 || j > image.Count)
                {
                    i++;
                    driver.Navigate().Back();
                    Thread.Sleep(1000);
                    continue;
                }
                try
                {
                    Thread.Sleep(200);
                    image[j].Click();
                }
                catch
                {
                    try
                    {
                        Thread.Sleep(1000);
                        image[j].Click();
                    }
                    catch
                    {
                        Thread.Sleep(5000);
                        image[j].Click();
                    }
                }
                var count = driver.FindElements(By.ClassName("el-table__row"));
                for (j = 0; j < count.Count - 1; j++)
                {
                    using (var client = new WebClient())
                    {
                        var elem = driver.FindElement(By.TagName("img"));
                        Actions action = new Actions(driver);
                        action.MoveToElement(elem).Click().Build().Perform();
                        var src = image[j].GetAttribute("src");
                        client.DownloadFile(src, "D:\\C#\\Тесты\\Parsing-master\\Готовый результат\\" + i + "\\" + j + ".jpg");                        
                        Thread.Sleep(200);
                        var next = driver.FindElement(By.ClassName("layui-layer-imgnext"));
                        var back = driver.FindElement(By.ClassName("layui-layer-imgprev"));
                        action.MoveToElement(elem).Click().Build().Perform();
                        try
                        {
                            next.Click();
                        }
                        catch
                        {
                            try
                            {
                                action.MoveToElement(elem).Click().Build().Perform();
                                next.Click();
                            }
                            catch
                            {
                                //если уже имеется созданная папка с идентификатором folder[i] то создать папку с именем folder[i] + 1 и приравниваем i + 1
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(200);
                }

                driver.Navigate().GoToUrl(UrlTb.Text);
                Thread.Sleep(1000);
            }
            MessageBox.Show("Готово");
        }
    }
}

