namespace JeuxDuPendu
{
    partial class dialogJoueur
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
            this.cboUtil = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radDifficile = new System.Windows.Forms.RadioButton();
            this.radMoyen = new System.Windows.Forms.RadioButton();
            this.radFacile = new System.Windows.Forms.RadioButton();
            this.btnJouer = new System.Windows.Forms.Button();
            this.btnQuitter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboUtil
            // 
            this.cboUtil.FormattingEnabled = true;
            this.cboUtil.Location = new System.Drawing.Point(13, 22);
            this.cboUtil.Name = "cboUtil";
            this.cboUtil.Size = new System.Drawing.Size(207, 21);
            this.cboUtil.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radDifficile);
            this.groupBox1.Controls.Add(this.radMoyen);
            this.groupBox1.Controls.Add(this.radFacile);
            this.groupBox1.Location = new System.Drawing.Point(13, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 107);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Difficulté";
            // 
            // radDifficile
            // 
            this.radDifficile.AutoSize = true;
            this.radDifficile.Location = new System.Drawing.Point(18, 75);
            this.radDifficile.Name = "radDifficile";
            this.radDifficile.Size = new System.Drawing.Size(59, 17);
            this.radDifficile.TabIndex = 2;
            this.radDifficile.Text = "Difficile";
            this.radDifficile.UseVisualStyleBackColor = true;
            // 
            // radMoyen
            // 
            this.radMoyen.AutoSize = true;
            this.radMoyen.Checked = true;
            this.radMoyen.Location = new System.Drawing.Point(18, 52);
            this.radMoyen.Name = "radMoyen";
            this.radMoyen.Size = new System.Drawing.Size(57, 17);
            this.radMoyen.TabIndex = 1;
            this.radMoyen.TabStop = true;
            this.radMoyen.Text = "Moyen";
            this.radMoyen.UseVisualStyleBackColor = true;
            // 
            // radFacile
            // 
            this.radFacile.AutoSize = true;
            this.radFacile.Location = new System.Drawing.Point(18, 31);
            this.radFacile.Name = "radFacile";
            this.radFacile.Size = new System.Drawing.Size(53, 17);
            this.radFacile.TabIndex = 0;
            this.radFacile.Text = "Facile";
            this.radFacile.UseVisualStyleBackColor = true;
            // 
            // btnJouer
            // 
            this.btnJouer.Location = new System.Drawing.Point(13, 214);
            this.btnJouer.Name = "btnJouer";
            this.btnJouer.Size = new System.Drawing.Size(75, 23);
            this.btnJouer.TabIndex = 3;
            this.btnJouer.Text = "Jouer";
            this.btnJouer.UseVisualStyleBackColor = true;
            this.btnJouer.Click += new System.EventHandler(this.btnJouer_Click);
            // 
            // btnQuitter
            // 
            this.btnQuitter.Location = new System.Drawing.Point(145, 214);
            this.btnQuitter.Name = "btnQuitter";
            this.btnQuitter.Size = new System.Drawing.Size(75, 23);
            this.btnQuitter.TabIndex = 4;
            this.btnQuitter.Text = "Quitter";
            this.btnQuitter.UseVisualStyleBackColor = true;
            this.btnQuitter.Click += new System.EventHandler(this.btnQuitter_Click);
            // 
            // dialogJoueur
            // 
            this.AcceptButton = this.btnJouer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnQuitter;
            this.ClientSize = new System.Drawing.Size(240, 249);
            this.Controls.Add(this.btnQuitter);
            this.Controls.Add(this.btnJouer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboUtil);
            this.Name = "dialogJoueur";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dialogJoueur";
            this.Load += new System.EventHandler(this.dialogJoueur_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboUtil;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radDifficile;
        private System.Windows.Forms.RadioButton radMoyen;
        private System.Windows.Forms.RadioButton radFacile;
        private System.Windows.Forms.Button btnJouer;
        private System.Windows.Forms.Button btnQuitter;
    }
}