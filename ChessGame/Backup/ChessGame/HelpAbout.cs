using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChessGame
{
    /*
     * This form simply displays information about the program and it's
     * author. There is no user interaction involved aside from closing
     * the window again.
     */
    public partial class HelpAbout : Form
    {
        public HelpAbout()
        {
            InitializeComponent();
            AssemblyName asm = Assembly.GetExecutingAssembly().GetName();
            Version v = asm.Version;

            lblVersion.Text = v.ToString();
            lblRelease.Text = RetrieveLinkerTimestamp().ToShortDateString();
            lblName.Text = asm.Name;

            this.Text = "About "+ asm.Name;
        }

        private DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

    }
}