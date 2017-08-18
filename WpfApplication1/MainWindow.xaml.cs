using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Interop;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool USED_SENDKEY = false;
        #region Cath's Code (No Activate Window)
        const int WS_EX_NOACTIVATE = 0x08000000;
        const int GWL_EXSTYLE = -20;

        [DllImport("user32", SetLastError = true)]
        private extern static int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32", SetLastError = true)]
        private extern static int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewValue);
        #endregion

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

#if (USED_SENDKEY == true)
        Dictionary<string, string> keyMap = new Dictionary<string, string>();

        private void CreateDictionary()
        {
            keyMap.Add("K0", "{0}");
            keyMap.Add("K1", "{1}");
            keyMap.Add("K2", "{2}");
            keyMap.Add("K3", "{3}");
            keyMap.Add("K4", "{4}");
            keyMap.Add("K5", "{5}");
            keyMap.Add("K6", "{6}");
            keyMap.Add("K7", "{7}");
            keyMap.Add("K8", "{8}");
            keyMap.Add("K9", "{9}");
            keyMap.Add("KDot", "{.}");
            keyMap.Add("KBackSpace", "{BACKSPACE}");
        }

        private string FindInDictionary(String FindMe)
        {
            if (true == (keyMap.ContainsKey(FindMe)))
            {
                return keyMap[FindMe];
            }
            //else
            //{
            //    return "Not Found";
            //}
            return null;
        }
#else
        Dictionary<string, byte> keyMap = new Dictionary<string, byte>();

        private void CreateDictionary()
        {
            keyMap.Add("K0", (byte)Keys.NumPad0);
            keyMap.Add("K1", (byte)Keys.NumPad1);
            keyMap.Add("K2", (byte)Keys.NumPad2);
            keyMap.Add("K3", (byte)Keys.NumPad3);
            keyMap.Add("K4", (byte)Keys.NumPad4);
            keyMap.Add("K5", (byte)Keys.NumPad5);
            keyMap.Add("K6", (byte)Keys.NumPad6);
            keyMap.Add("K7", (byte)Keys.NumPad7);
            keyMap.Add("K8", (byte)Keys.NumPad8);
            keyMap.Add("K9", (byte)Keys.NumPad9);
            keyMap.Add("KDot", (byte)Keys.Decimal);
            keyMap.Add("KBackSpace", (byte)Keys.Back);
        }

        private byte FindInDictionary(String FindMe)
        {
            if (true == (keyMap.ContainsKey(FindMe)))
            {
                return keyMap[FindMe];
            }
            //else
            //{
            //    return "Not Found";
            //}
            return 0x00;
        }
#endif

        //定義數值
        public MainWindow()
        {
            InitializeComponent();
            CreateDictionary();

            //Click Function for Buttons
            foreach (System.Windows.Controls.Button Key in NumGroup.Children)
            {
                Key.Click += delegate
                {
                    IntPtr handle = FindWindow(null, "");
                    // Verify that Calculator is a running process.
                    if (handle == IntPtr.Zero)
                    {
                        return;
                    }
                    
                    foreach (var process in Process.GetProcessesByName("C01_Chatbot_WPF.exe"))
                    //foreach (var process in Process.GetProcessesByName("Notepad"))
                    {
                        
                        handle = process.MainWindowHandle;
                        SetForegroundWindow(handle);
                        
                        
                    }
#if (USED_SENDKEY == true)
                    string keyChar = FindInDictionary(Key.Name);
                    Console.WriteLine("key in " + Key.Name + " " + keyChar);
                    SendKeys.SendWait(keyChar);
#else
                    byte keyByte = FindInDictionary(Key.Name);
                    Console.WriteLine("key in " + Key.Name + " " + keyByte);
                    keybd_event(keyByte, 0, 0x0001, handle); // key_down
                    keybd_event(keyByte, 0, 0x0002, handle); // key_up
#endif
                };
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            int exstyle = GetWindowLong(wih.Handle, GWL_EXSTYLE);
            exstyle |= WS_EX_NOACTIVATE;
            SetWindowLong(wih.Handle, GWL_EXSTYLE, exstyle);
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Console.WriteLine("key down " + e.Key);
        }

        private void textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Console.WriteLine("key down " + e.Key);
        }
    }
}
