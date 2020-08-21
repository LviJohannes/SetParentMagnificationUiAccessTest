using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetParentMagnificationUiAccessTest
{
    public partial class Form1 : Form
    {
        private IntPtr HWndMag = IntPtr.Zero;
        private MONITORINFO PrimaryMonitorInfo = new MONITORINFO();
        private RECT MagWindowRect = new RECT();
        private bool MagnificationInitialized = false;
        private Timer Timer = new Timer();
        private bool ShouldSetupMagnifier = true;

        private float Magnification = 1.0f;

        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            NativeMethods.SetWindowLong(Handle, 
                (int)GWL.GWL_EXSTYLE, 
                NativeMethods.GetWindowLong(Handle, (int)GWL.GWL_EXSTYLE) | (int)ExtendedWindowStyles.WS_EX_TOPMOST | (int)ExtendedWindowStyles.WS_EX_LAYERED | (int)ExtendedWindowStyles.WS_EX_TRANSPARENT);

            PrimaryMonitorInfo.cbSize = Marshal.SizeOf(PrimaryMonitorInfo);

            var primaryMonitor = NativeMethods.MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
            NativeMethods.GetMonitorInfo(primaryMonitor, ref PrimaryMonitorInfo);

            MagnificationInitialized = NativeMethods.MagInitialize();

            MagnificationPanel.SizeChanged += MagnificationPanel_SizeChanged;

            if (MagnificationInitialized)
            {
                Timer.Interval = 10;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }

        private void MagnificationPanel_SizeChanged(object sender, EventArgs e)
        {
            UpdatePanelSizes();

            if (MagnificationInitialized && ShouldSetupMagnifier)
            {
                ShouldSetupMagnifier = false;

                SetupMagnifier();

                DockApplication();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateMaginifier();
        }


        private void UpdateMaginifier()
        {
            UpdatePanelSizes();

            if (!MagnificationInitialized || HWndMag == IntPtr.Zero)
            {
                return;
            }

            RECT sourceRect = new RECT();
            POINT mousePoint = new POINT(0, 0);

            NativeMethods.GetCursorPos(ref mousePoint);

            var extraWidth = 0;
            var extraHeight = 0;

            int width = (int)((MagWindowRect.right + extraWidth) / Magnification);
            int height = (int)((MagWindowRect.bottom + extraHeight) / Magnification);

            var xFactor = mousePoint.x - (PrimaryMonitorInfo.rcMonitor.left + (width / 2));
            var yFactor = mousePoint.y - (PrimaryMonitorInfo.rcMonitor.top + (height / 2));

            sourceRect.left = PrimaryMonitorInfo.rcMonitor.left + xFactor;
            sourceRect.top = PrimaryMonitorInfo.rcMonitor.top + yFactor;

            if (sourceRect.left < PrimaryMonitorInfo.rcMonitor.left)
            {
                sourceRect.left = PrimaryMonitorInfo.rcMonitor.left;
            }

            if (sourceRect.left > PrimaryMonitorInfo.rcMonitor.right - width)
            {
                sourceRect.left = PrimaryMonitorInfo.rcMonitor.right - width;
            }

            sourceRect.right = sourceRect.left + width;

            if (sourceRect.top < PrimaryMonitorInfo.rcMonitor.top)
            {
                sourceRect.top = PrimaryMonitorInfo.rcMonitor.top;
            }

            if (sourceRect.top > PrimaryMonitorInfo.rcMonitor.bottom - height)
            {
                sourceRect.top = PrimaryMonitorInfo.rcMonitor.bottom - height;
            }

            sourceRect.bottom = sourceRect.top + height;

            NativeMethods.MagSetWindowSource(HWndMag, sourceRect);

            NativeMethods.SetWindowPos(this.Handle, new IntPtr(0), 0, 0, 0, 0,
                (int)SetWindowPosFlags.SWP_NOACTIVATE |
                (int)SetWindowPosFlags.SWP_NOZORDER |
                (int)SetWindowPosFlags.SWP_NOREDRAW |
                (int)SetWindowPosFlags.SWP_NOMOVE |
                (int)SetWindowPosFlags.SWP_NOSIZE);

            NativeMethods.InvalidateRect(HWndMag, IntPtr.Zero, true);
        }

        private void SetupMagnifier()
        {
            DestroyMagnifier();

            var hInst = NativeMethods.GetModuleHandle(null);

            NativeMethods.GetClientRect(MagnificationPanel.Handle, ref MagWindowRect);

            HWndMag = NativeMethods.CreateWindow((int)ExtendedWindowStyles.WS_EX_STATICEDGE, NativeMethods.WC_MAGNIFIER,
                    "MagnificationWindow", (int)WindowStyles.WS_CHILD | (int)MagnifierStyle.MS_SHOWMAGNIFIEDCURSOR | (int)WindowStyles.WS_VISIBLE,
                    MagWindowRect.left, MagWindowRect.top, MagWindowRect.right, MagWindowRect.bottom, MagnificationPanel.Handle, IntPtr.Zero, hInst, IntPtr.Zero);

            if (HWndMag == IntPtr.Zero)
            {
                return;
            }

            var matrix = new Transformation(Magnification);

            NativeMethods.MagSetWindowTransform(HWndMag, ref matrix);
        }

        private void DestroyMagnifier()
        {
            if (HWndMag != null || HWndMag != IntPtr.Zero)
            {
                NativeMethods.DestroyWindow(HWndMag);
            }
        }

        private void UpdatePanelSizes()
        {
            DockedWindowPanel.Width = 400;
            DockedWindowPanel.Height = PrimaryMonitorInfo.rcMonitor.bottom;

            MagnificationPanel.Width = PrimaryMonitorInfo.rcMonitor.right - 400;
            MagnificationPanel.Height = PrimaryMonitorInfo.rcMonitor.bottom;
        }

        private Process DockedProcess = null;
        private async void DockApplication()
        {
            if (DockedProcess != null)
            {
                return;
            }

            await Task.Delay(500);

            var processStartInfo = new ProcessStartInfo("notepad.exe")
            {
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Maximized,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            DockedProcess = Process.Start(processStartInfo);

            DockedProcess.WaitForInputIdle();

            NativeMethods.SetParent(DockedProcess.MainWindowHandle, DockedWindowPanel.Handle);

             NativeMethods.SetWindowLong(DockedProcess.MainWindowHandle,
                 (int)GWL.GWL_STYLE,
                 NativeMethods.GetWindowLong(DockedProcess.MainWindowHandle, (int)GWL.GWL_STYLE) & (int)~WindowStyles.WS_CAPTION & (int)~WindowStyles.WS_THICKFRAME);

             NativeMethods.SetWindowPos(DockedProcess.MainWindowHandle, IntPtr.Zero, 0, 0, DockedWindowPanel.Width, DockedWindowPanel.Height, (int)SetWindowPosFlags.SWP_NOZORDER | (int)SetWindowPosFlags.SWP_NOACTIVATE);
        }
    }
}
