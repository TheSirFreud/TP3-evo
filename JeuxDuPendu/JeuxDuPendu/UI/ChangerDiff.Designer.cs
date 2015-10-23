namespace JeuxDuPendu
{
    partial class ChangerDiff
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
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.btnChanger = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radDifficile = new System.Windows.Forms.RadioButton();
            this.radMoyen = new System.Windows.Forms.RadioButton();
            this.radFacile = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnuler.Location = new System.Drawing.Point(93, 134);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Size = new System.Drawing.Size(75, 23);
            this.btnAnnuler.TabIndex = 7;
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.UseVisualStyleBackColor = true;
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // btnChanger
            // 
            this.btnChanger.Location = new System.Drawing.Point(12, 134);
            this.btnChanger.Name = "btnChanger";
            this.btnChanger.Size = new System.Drawing.Size(75, 23);
            this.btnChanger.TabIndex = 6;
            this.btnChanger.Text = "Confirmer";
            this.btnChanger.UseVisualStyleBackColor = true;
            this.btnChanger.Click += new System.EventHandler(this.btnChanger_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radDifficile);
            this.groupBox1.Controls.Add(this.radMoyen);
            this.groupBox1.Controls.Add(this.radFacile);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 107);
            this.groupBox1.TabIndex = 5;
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
            // ChangerDiff
            // 
            this.AcceptButton = this.btnChanger;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAnnuler;
            this.ClientSize = new System.Drawing.Size(176, 169);
            this.Controls.Add(this.btnAnnuler);
            this.Controls.Add(this.btnChanger);
            this.Controls.Add(this.groupBox1);
            this.Name = "ChangerDiff";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChangerDiff";
            this.Load += new System.EventHandler(this.ChangerDiff_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.Button btnChanger;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radDifficile;
        private System.Windows.Forms.RadioButton radMoyen;
        private System.Windows.Forms.RadioButton radFacile;
    }
}