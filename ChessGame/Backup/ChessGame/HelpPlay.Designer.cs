namespace ChessGame
{
    partial class HelpPlay
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lnkAdvMove = new System.Windows.Forms.LinkLabel();
            this.lnkTactic = new System.Windows.Forms.LinkLabel();
            this.lnkObj = new System.Windows.Forms.LinkLabel();
            this.lnkBasicMove = new System.Windows.Forms.LinkLabel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lnkAdvMove);
            this.splitContainer1.Panel1.Controls.Add(this.lnkTactic);
            this.splitContainer1.Panel1.Controls.Add(this.lnkObj);
            this.splitContainer1.Panel1.Controls.Add(this.lnkBasicMove);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblTitle);
            this.splitContainer1.Panel2.Controls.Add(this.rtbHelp);
            this.splitContainer1.Size = new System.Drawing.Size(554, 474);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 0;
            // 
            // lnkAdvMove
            // 
            this.lnkAdvMove.AutoSize = true;
            this.lnkAdvMove.Location = new System.Drawing.Point(23, 185);
            this.lnkAdvMove.Name = "lnkAdvMove";
            this.lnkAdvMove.Size = new System.Drawing.Size(139, 13);
            this.lnkAdvMove.TabIndex = 3;
            this.lnkAdvMove.TabStop = true;
            this.lnkAdvMove.Text = "Advanced Piece Movement";
            this.lnkAdvMove.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdvMove_LinkClicked);
            // 
            // lnkTactic
            // 
            this.lnkTactic.AutoSize = true;
            this.lnkTactic.Location = new System.Drawing.Point(23, 138);
            this.lnkTactic.Name = "lnkTactic";
            this.lnkTactic.Size = new System.Drawing.Size(71, 13);
            this.lnkTactic.TabIndex = 2;
            this.lnkTactic.TabStop = true;
            this.lnkTactic.Text = "Basic Tactics";
            this.lnkTactic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTactis_LinkClicked);
            // 
            // lnkObj
            // 
            this.lnkObj.AutoSize = true;
            this.lnkObj.Location = new System.Drawing.Point(23, 52);
            this.lnkObj.Name = "lnkObj";
            this.lnkObj.Size = new System.Drawing.Size(52, 13);
            this.lnkObj.TabIndex = 1;
            this.lnkObj.TabStop = true;
            this.lnkObj.Text = "Overview";
            this.lnkObj.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkObj_LinkClicked);
            // 
            // lnkBasicMove
            // 
            this.lnkBasicMove.AutoSize = true;
            this.lnkBasicMove.Location = new System.Drawing.Point(23, 91);
            this.lnkBasicMove.Name = "lnkBasicMove";
            this.lnkBasicMove.Size = new System.Drawing.Size(116, 13);
            this.lnkBasicMove.TabIndex = 0;
            this.lnkBasicMove.TabStop = true;
            this.lnkBasicMove.Text = "Basic Piece Movement";
            this.lnkBasicMove.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBasicMove_LinkClicked);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(149, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 17);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Overview";
            // 
            // rtbHelp
            // 
            this.rtbHelp.Location = new System.Drawing.Point(4, 35);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.ReadOnly = true;
            this.rtbHelp.Size = new System.Drawing.Size(359, 436);
            this.rtbHelp.TabIndex = 0;
            this.rtbHelp.TabStop = false;
            this.rtbHelp.Text = "";
            // 
            // HelpPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 474);
            this.Controls.Add(this.splitContainer1);
            this.Name = "HelpPlay";
            this.Text = "How to Play Chess";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.LinkLabel lnkObj;
        private System.Windows.Forms.LinkLabel lnkBasicMove;
        private System.Windows.Forms.LinkLabel lnkTactic;
        private System.Windows.Forms.LinkLabel lnkAdvMove;
        private System.Windows.Forms.RichTextBox rtbHelp;
        private System.Windows.Forms.Label lblTitle;
    }
}