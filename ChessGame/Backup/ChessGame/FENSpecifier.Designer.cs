namespace ChessGame
{
    partial class FENSpecifier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnToClipboard = new System.Windows.Forms.Button();
            this.btnToFile = new System.Windows.Forms.Button();
            this.pnlOutput = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.btnFromFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.pnlOutput.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(15, 121);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(96, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(12, 54);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(272, 20);
            this.txtInput.TabIndex = 2;
            this.txtInput.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please enter a FEN-formatted value";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnToClipboard
            // 
            this.btnToClipboard.Location = new System.Drawing.Point(3, 4);
            this.btnToClipboard.Name = "btnToClipboard";
            this.btnToClipboard.Size = new System.Drawing.Size(75, 23);
            this.btnToClipboard.TabIndex = 4;
            this.btnToClipboard.Text = "To Clipboard";
            this.btnToClipboard.UseVisualStyleBackColor = true;
            this.btnToClipboard.Click += new System.EventHandler(this.btnToClipboard_Click);
            // 
            // btnToFile
            // 
            this.btnToFile.Location = new System.Drawing.Point(84, 4);
            this.btnToFile.Name = "btnToFile";
            this.btnToFile.Size = new System.Drawing.Size(75, 23);
            this.btnToFile.TabIndex = 5;
            this.btnToFile.Text = "To File";
            this.btnToFile.UseVisualStyleBackColor = true;
            this.btnToFile.Click += new System.EventHandler(this.btnToFile_Click);
            // 
            // pnlOutput
            // 
            this.pnlOutput.Controls.Add(this.panel2);
            this.pnlOutput.Controls.Add(this.btnToClipboard);
            this.pnlOutput.Controls.Add(this.btnToFile);
            this.pnlOutput.Location = new System.Drawing.Point(12, 81);
            this.pnlOutput.Name = "pnlOutput";
            this.pnlOutput.Size = new System.Drawing.Size(176, 31);
            this.pnlOutput.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 57);
            this.panel2.TabIndex = 6;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.btnFromFile);
            this.pnlInput.Location = new System.Drawing.Point(194, 81);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(86, 32);
            this.pnlInput.TabIndex = 7;
            // 
            // btnFromFile
            // 
            this.btnFromFile.Location = new System.Drawing.Point(3, 5);
            this.btnFromFile.Name = "btnFromFile";
            this.btnFromFile.Size = new System.Drawing.Size(75, 23);
            this.btnFromFile.TabIndex = 8;
            this.btnFromFile.Text = "From File";
            this.btnFromFile.UseVisualStyleBackColor = true;
            this.btnFromFile.Click += new System.EventHandler(this.btnFromFile_Click);
            // 
            // FENSpecifier
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(298, 153);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.pnlOutput);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInput);
            this.Name = "FENSpecifier";
            this.Text = "FENSpecifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FENSpecifier_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.pnlOutput.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnToFile;
        private System.Windows.Forms.Button btnToClipboard;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Button btnFromFile;
        private System.Windows.Forms.Panel pnlOutput;
        private System.Windows.Forms.Panel panel2;
    }
}