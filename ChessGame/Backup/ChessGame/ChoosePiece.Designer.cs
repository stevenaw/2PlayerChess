namespace ChessGame
{
    partial class ChoosePiece
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
            this.picQueen = new System.Windows.Forms.PictureBox();
            this.picRook = new System.Windows.Forms.PictureBox();
            this.picBishop = new System.Windows.Forms.PictureBox();
            this.picKnight = new System.Windows.Forms.PictureBox();
            this.radChooseQ = new System.Windows.Forms.RadioButton();
            this.radChooseR = new System.Windows.Forms.RadioButton();
            this.radChooseB = new System.Windows.Forms.RadioButton();
            this.radChooseN = new System.Windows.Forms.RadioButton();
            this.btnPromote = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.picQueen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBishop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKnight)).BeginInit();
            this.SuspendLayout();
            // 
            // picQueen
            // 
            this.helpProvider1.SetHelpString(this.picQueen, "A queen");
            this.picQueen.Image = global::ChessGame.Properties.Resources.queenW;
            this.picQueen.Location = new System.Drawing.Point(12, 12);
            this.picQueen.Name = "picQueen";
            this.helpProvider1.SetShowHelp(this.picQueen, true);
            this.picQueen.Size = new System.Drawing.Size(52, 52);
            this.picQueen.TabIndex = 0;
            this.picQueen.TabStop = false;
            this.picQueen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_Click);
            this.picQueen.MouseEnter += new System.EventHandler(this.SetFocus);
            // 
            // picRook
            // 
            this.helpProvider1.SetHelpString(this.picRook, "A rook");
            this.picRook.Image = global::ChessGame.Properties.Resources.rookW;
            this.picRook.Location = new System.Drawing.Point(70, 12);
            this.picRook.Name = "picRook";
            this.helpProvider1.SetShowHelp(this.picRook, true);
            this.picRook.Size = new System.Drawing.Size(52, 52);
            this.picRook.TabIndex = 1;
            this.picRook.TabStop = false;
            this.picRook.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_Click);
            // 
            // picBishop
            // 
            this.helpProvider1.SetHelpString(this.picBishop, "A bishop");
            this.picBishop.Image = global::ChessGame.Properties.Resources.bishopW;
            this.picBishop.Location = new System.Drawing.Point(128, 12);
            this.picBishop.Name = "picBishop";
            this.helpProvider1.SetShowHelp(this.picBishop, true);
            this.picBishop.Size = new System.Drawing.Size(52, 52);
            this.picBishop.TabIndex = 2;
            this.picBishop.TabStop = false;
            this.picBishop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_Click);
            // 
            // picKnight
            // 
            this.helpProvider1.SetHelpString(this.picKnight, "A knight");
            this.picKnight.Image = global::ChessGame.Properties.Resources.knightW;
            this.picKnight.Location = new System.Drawing.Point(186, 12);
            this.picKnight.Name = "picKnight";
            this.helpProvider1.SetShowHelp(this.picKnight, true);
            this.picKnight.Size = new System.Drawing.Size(52, 52);
            this.picKnight.TabIndex = 3;
            this.picKnight.TabStop = false;
            this.picKnight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_Click);
            // 
            // radChooseQ
            // 
            this.radChooseQ.AutoSize = true;
            this.radChooseQ.Location = new System.Drawing.Point(31, 71);
            this.radChooseQ.Name = "radChooseQ";
            this.radChooseQ.Size = new System.Drawing.Size(14, 13);
            this.radChooseQ.TabIndex = 5;
            this.radChooseQ.TabStop = true;
            this.radChooseQ.Tag = "";
            this.radChooseQ.UseVisualStyleBackColor = true;
            // 
            // radChooseR
            // 
            this.radChooseR.AutoSize = true;
            this.radChooseR.Location = new System.Drawing.Point(88, 70);
            this.radChooseR.Name = "radChooseR";
            this.radChooseR.Size = new System.Drawing.Size(14, 13);
            this.radChooseR.TabIndex = 6;
            this.radChooseR.TabStop = true;
            this.radChooseR.UseVisualStyleBackColor = true;
            // 
            // radChooseB
            // 
            this.radChooseB.AutoSize = true;
            this.radChooseB.Location = new System.Drawing.Point(148, 70);
            this.radChooseB.Name = "radChooseB";
            this.radChooseB.Size = new System.Drawing.Size(14, 13);
            this.radChooseB.TabIndex = 7;
            this.radChooseB.TabStop = true;
            this.radChooseB.UseVisualStyleBackColor = true;
            // 
            // radChooseN
            // 
            this.radChooseN.AutoSize = true;
            this.radChooseN.Location = new System.Drawing.Point(206, 70);
            this.radChooseN.Name = "radChooseN";
            this.radChooseN.Size = new System.Drawing.Size(14, 13);
            this.radChooseN.TabIndex = 8;
            this.radChooseN.TabStop = true;
            this.radChooseN.UseVisualStyleBackColor = true;
            // 
            // btnPromote
            // 
            this.btnPromote.Location = new System.Drawing.Point(87, 106);
            this.btnPromote.Name = "btnPromote";
            this.btnPromote.Size = new System.Drawing.Size(75, 23);
            this.btnPromote.TabIndex = 9;
            this.btnPromote.Text = "&Promote";
            this.btnPromote.UseVisualStyleBackColor = true;
            this.btnPromote.Click += new System.EventHandler(this.btnPromote_Click);
            // 
            // ChoosePiece
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 141);
            this.Controls.Add(this.btnPromote);
            this.Controls.Add(this.radChooseN);
            this.Controls.Add(this.radChooseB);
            this.Controls.Add(this.radChooseR);
            this.Controls.Add(this.radChooseQ);
            this.Controls.Add(this.picKnight);
            this.Controls.Add(this.picBishop);
            this.Controls.Add(this.picRook);
            this.Controls.Add(this.picQueen);
            this.Name = "ChoosePiece";
            this.Text = "Choose a Piece";
            this.Load += new System.EventHandler(this.ChoosePiece_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnsureSelected);
            ((System.ComponentModel.ISupportInitialize)(this.picQueen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBishop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKnight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picQueen;
        private System.Windows.Forms.PictureBox picRook;
        private System.Windows.Forms.PictureBox picBishop;
        private System.Windows.Forms.PictureBox picKnight;
        private System.Windows.Forms.RadioButton radChooseQ;
        private System.Windows.Forms.RadioButton radChooseR;
        private System.Windows.Forms.RadioButton radChooseB;
        private System.Windows.Forms.RadioButton radChooseN;
        private System.Windows.Forms.Button btnPromote;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}