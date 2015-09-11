using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ChessGame
{
    /*
     * A form which is used to display information on various help topics to the user
     * 
     * Links are clicked on the left side of the user interface, with the appropriate
     * help info displayed on the right. This info is streamed from text files before
     * it displayed to the user.
     * 
     * In an attempt to minimize streaming, files are read once, the first time they
     * are needed, and are then stored in memory if they need to be later referenced
     * during the life of this form.
     */
    public partial class HelpPlay : Form
    {
        // Array of stored data for help topics
        string[] files = new string[4];

        // Load the first file into memory and display it
        public HelpPlay()
        {
            InitializeComponent();
            this.lnkObj_LinkClicked(null, null);
        }

        /*
         * Display the first help topic, Objective, to the user
         * 
         * This help file is already loaded into memory on Form instantiation, so it only
         * has to be referenced, and checking to see if it's been read doesn't happen. The
         * label indicating the help topic currently displayed is changed and centered as 
         * well.
         */
        private void lnkObj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (files[0] == null)
                files[0] = Properties.Resources.Overview;
                //files[0] = readAndReturn(rootDir + "Overview.txt");

            lblTitle.Text = "Overview";
            lblTitle.Location = new Point((splitContainer1.Size.Width - splitContainer1.SplitterDistance + splitContainer1.SplitterWidth) / 2 - lblTitle.Size.Width / 2, lblTitle.Location.Y);
            rtbHelp.Text = files[0];
        }

        /*
         * Display the second help topic, Basic Movement, to the user
         * 
         * First, check to see if it's already been read into memory. If it hasn't, read it
         * in. After that, update the label and display the file's contents. Change and center
         * the Title.
         */
        private void lnkBasicMove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (files[1] == null)
                files[1] = Properties.Resources.BasicMovement;
                //files[1] = readAndReturn(rootDir + "BasicMovement.txt");

            lblTitle.Text = "Basic Piece Movement";

            lblTitle.Location = new Point((splitContainer1.Size.Width - splitContainer1.SplitterDistance + splitContainer1.SplitterWidth) / 2 - lblTitle.Size.Width / 2, lblTitle.Location.Y);
            rtbHelp.Text = files[1];
        }

        /*
         * Display the second help topic, Basic Tactics, to the user
         * 
         * First, check to see if it's already been read into memory. If it hasn't, read it
         * in. After that, update the label and display the file's contents. Change and center
         * the Title.
         */
        private void lnkTactis_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (files[2] == null)
                files[2] = Properties.Resources.BasicTactics;
                //files[2] = readAndReturn(rootDir + "Basic Tactics.txt");

            lblTitle.Text = "Basic Tactics";
            lblTitle.Location = new Point((splitContainer1.Size.Width - splitContainer1.SplitterDistance + splitContainer1.SplitterWidth) / 2 - lblTitle.Size.Width / 2, lblTitle.Location.Y);
            rtbHelp.Text = files[2];
        }

        /*
         * Display the second help topic, Advanced Movement, to the user
         * 
         * First, check to see if it's already been read into memory. If it hasn't, read it
         * in. After that, update the label and display the file's contents. Change and center
         * the Title.
         */
        private void lnkAdvMove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (files[3] == null)
                files[3] = Properties.Resources.AdvMovement;
                //files[3] = readAndReturn(rootDir + "AdvMovement.txt");

            lblTitle.Text = "Advanced Movement";
            lblTitle.Location = new Point((splitContainer1.Size.Width - splitContainer1.SplitterDistance + splitContainer1.SplitterWidth) / 2 - lblTitle.Size.Width / 2, lblTitle.Location.Y);
            rtbHelp.Text = files[3];
        }


        /*
         * Read in the passed through file into a string, close the stream, and then return
         * the string to the calling function.
         */
        //private string readAndReturn(string textFile)
        //{
        //    StreamReader reader = null;

        //    try
        //    {
        //        reader = File.OpenText(textFile);
        //        return reader.ReadToEnd();
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //    finally { reader.Close(); }
        //}
    }
}