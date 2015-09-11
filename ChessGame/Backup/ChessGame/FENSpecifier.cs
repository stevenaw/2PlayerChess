using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ChessGameLib;

namespace ChessGame
{
    public partial class FENSpecifier : Form
    {
        bool validEntry = true;
        SerializationModes mode;

        /*
         * A dialog-box which is used to save game data (PGN or FEN format)
         */
        FileDialog saveDiag = new SaveFileDialog();
        FileDialog openDiag = new OpenFileDialog();

        public FENSpecifier(SerializationModes mode, string startValue)
        {
            InitializeComponent();
            this.saveDiag.Filter = Main.FILETYPE_FEN;
            this.openDiag.Filter = Main.FILETYPE_FEN;
            this.setMode(mode);
            this.Result = startValue;
        }

        public FENSpecifier(SerializationModes mode, Board b)
        {
            InitializeComponent();
            this.saveDiag.Filter = Main.FILETYPE_FEN;
            this.openDiag.Filter = Main.FILETYPE_FEN;
            this.setMode(mode);

            if (b != null)
                this.Result = new FENSerializer().Serialize(b);
        }

        public FENSpecifier(SerializationModes mode)
            : this(mode, String.Empty)
        {
        }

        public void setMode(SerializationModes mode)
        {
            this.mode = mode;

            switch (mode)
            {
                case SerializationModes.Input:
                    pnlInput.Enabled = true;
                    txtInput.ReadOnly = false;
                    pnlOutput.Enabled = false;
                    break;
                case SerializationModes.Output:
                default:
                    pnlInput.Enabled = false;
                    pnlOutput.Enabled = true;
                    txtInput.ReadOnly = true;
                    break;
            }
        }

        public string Result
        {
            get
            {
                return txtInput.Text;
            }
            private set
            {
                txtInput.Text = value;
                txtInput.Focus();
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            // Check if valid FEN string
            FENSerializer serializer = new FENSerializer();
            validEntry = serializer.IsValidSerialization(txtInput.Text) || String.IsNullOrEmpty(txtInput.Text);
            //GameSerializer serializer = new GameSerializer();
            //validEntry = String.IsNullOrEmpty(serializer.IsValidFEN(txtInput.Text));
            e.Cancel = !validEntry;

            if (!validEntry)
                errorProvider1.SetError(txtInput, "Not a valid FEN string");
            else
                errorProvider1.SetError(txtInput, "");
        }
        
        private void FENSpecifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.DialogResult = validEntry ? DialogResult.OK : DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
        }
        
        private void btnFromFile_Click(object sender, EventArgs e)
        {
            // Prompt for where to save file
            if (this.openDiag.ShowDialog() == DialogResult.OK)
            {
                // Open stream and serializer
                StreamReader stream = new StreamReader(new FileStream(this.openDiag.FileName, FileMode.OpenOrCreate, FileAccess.Read));
                FENSerializer s = new FENSerializer();

                string result = String.Empty;

                try
                {
                    // Generate string and write to file
                    result = stream.ReadLine();

                    if (s.IsValidSerialization(result))
                        this.Result = result;
                }
                catch (Exception ex) // May be FEN Exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        private void btnToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.txtInput.Text);
        }

        private void btnToFile_Click(object sender, EventArgs e)
        {
            // Prompt for where to save file
            if (this.saveDiag.ShowDialog() == DialogResult.OK)
            {
                // Open stream and serializer
                StreamWriter writer = new StreamWriter(new FileStream(this.saveDiag.FileName, FileMode.OpenOrCreate, FileAccess.Write));
                FENSerializer s = new FENSerializer();

                try
                {
                    // Format, write and close
                    writer.Write(this.Result);
                    writer.Flush();
                }
                catch (Exception ex) // May be FEN Exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
    }
}