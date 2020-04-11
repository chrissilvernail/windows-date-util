using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace WindowsDateUtilGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileInfo targetFile;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            
            if (fileDialog.ShowDialog()==true)
            {
                HandleNewTargetFile(new FileInfo(fileDialog.FileName));
            }
        }

        private void lbl_TargetFileName_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void lbl_TargetFileName_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                HandleNewTargetFile(new FileInfo(fileNames[0]));
            }
        }

        private void HandleNewTargetFile(FileInfo file)
        {
            targetFile = file;
            lbl_TargetFileName.Content = targetFile.FullName;
            var name = targetFile.Name;
            var directory = targetFile.Directory.Name;
            try
            {
                lbl_DateFromFile.Content = "";
                btn_UpdateFromFile.IsEnabled = false;
               var result = ParseDashDateFormat(name);
                if (result != DateTime.MinValue)
                {
                    lbl_DateFromFile.Content = result.ToString();
                    btn_UpdateFromFile.IsEnabled = true;
                }
            }
            catch { }
            try
            {
                lbl_DateFromParentDirectory.Content = "";
                var result = ParseDashDateFormat(directory);
                btn_UpdateFromParent.IsEnabled = false;
                if (result != DateTime.MinValue)
                {
                    lbl_DateFromParentDirectory.Content = result.ToString();
                    btn_UpdateFromParent.IsEnabled = true;
                }
            }
            catch { }

        }
        private DateTime ParseDashDateFormat(string inputString)
        {
            DateTime result=DateTime.MinValue;
            try
            {
                var year = Int16.Parse(inputString.Substring(0, 4));
                var month = Int16.Parse(inputString.Substring(5, 2));
                var day = Int16.Parse(inputString.Substring(8, 2));
                var dateFromFileName = new DateTime(year, month, day);
                result =  dateFromFileName.AddHours(16);
            }
            catch
            {
                
            }
            return result;
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            SetDateCreated(targetFile, ParseDashDateFormat(targetFile.Name));
            SetDateCreated(targetFile, ParseDashDateFormat(targetFile.Name));
        }

        private static void SetDateModified(FileInfo fileInfo, DateTime dateTime)
        {
            Console.WriteLine("Setting {0} [Date modified] to {1}", fileInfo.FullName, dateTime);
            fileInfo.LastWriteTime = dateTime;
        }
        private static void SetDateCreated(FileInfo fileInfo, DateTime dateTime)
        {
            Console.WriteLine("Setting {0} [Date created] to {1}", fileInfo.FullName, dateTime);
            fileInfo.CreationTime = dateTime;
        }

        private void btn_UpdateFromParent_Click(object sender, RoutedEventArgs e)
        {
            SetDateCreated(targetFile, ParseDashDateFormat(targetFile.Directory.Name));
        }
    }
}
