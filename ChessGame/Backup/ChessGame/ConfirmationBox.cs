using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChessGame
{
    /*
     * A generic confirmation box class, inspired by the built-in MessageBox class.
     * 
     * The only exterally-callable method is ShowDialog(), which creates an instantiation
     * of this class, shows it in a Dialog box, and returns the boolean result of it.
     */
    public partial class ConfirmationBox : Form
    {
        /*
         * Variable dictating whether confirmation was given or not
         * 
         * Set to false by default so that if the box is closed without clicking "OK",
         * confirmation is registered as not being given.
         */
        bool okay = false;

        /*
         * Private constructor to ensure that the class can not be instantiated differently
         * than anticipated, which would result in a different implementation method.
         */
        private ConfirmationBox(string text, string title)
        {
            InitializeComponent();
            rtbDisplayText.Text = text;
            this.Text = title;
        }

        /*
         * Only callable method in class, it dispays the form in a dialog box and returns
         * the result to the caller.
         * 
         * The two parameters, text and title, allow customization on what question is
         * displayed, as well as what the title of the box is.
         */
        public static bool ShowDialog(string text, string title)
        {
            ConfirmationBox form = new ConfirmationBox(text, title);
            form.ShowDialog();
            return form.okay;
        }

        // Sets confirmation to true and closes the window
        private void btnOK_Click(object sender, EventArgs e)
        {
            okay = true;
            this.Close();
        }

        // Closes the window, retaining the default confirmation of false
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}