namespace JeuxDuPendu
{
    partial class ConsultStat
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
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblGagne = new System.Windows.Forms.Label();
            this.lblPerdu = new System.Windows.Forms.Label();
            this.lblPourcentage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitre
            // 
            this.lblTitre.AutoSize = true;
            this.lblTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitre.Location = new System.Drawing.Point(12, 25);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(93, 17);
            this.lblTitre.TabIndex = 0;
            this.lblTitre.Text = "Statistiques";
            // 
            // lblGagne
            // 
            this.lblGagne.AutoSize = true;
            this.lblGagne.Location = new System.Drawing.Point(196, 87);
            this.lblGagne.Name = "lblGagne";
            this.lblGagne.Size = new System.Drawing.Size(35, 13);
            this.lblGagne.TabIndex = 1;
            this.lblGagne.Text = "label2";
            // 
            // lblPerdu
            // 
            this.lblPerdu.AutoSize = true;
            this.lblPerdu.Location = new System.Drawing.Point(196, 131);
            this.lblPerdu.Name = "lblPerdu";
            this.lblPerdu.Size = new System.Drawing.Size(35, 13);
            this.lblPerdu.TabIndex = 2;
            this.lblPerdu.Text = "label3";
            // 
            // lblPourcentage
            // 
            this.lblPourcentage.AutoSize = true;
            this.lblPourcentage.Location = new System.Drawing.Point(196, 173);
            this.lblPourcentage.Name = "lblPourcentage";
            this.lblPourcentage.Size = new System.Drawing.Size(35, 13);
            this.lblPourcentage.TabIndex = 3;
            this.lblPourcentage.Text = "label3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nombre de parties gagnées :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nombre de parties perdues :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pourcentage des parties gagnées :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Score :";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(196, 215);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(35, 13);
            this.lblScore.TabIndex = 7;
            this.lblScore.Text = "label3";
            // 
            // ConsultStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPourcentage);
            this.Controls.Add(this.lblPerdu);
            this.Controls.Add(this.lblGagne);
            this.Controls.Add(this.lblTitre);
            this.Name = "ConsultStat";
            this.Text = "ConsultStat";
            this.Load += new System.EventHandler(this.ConsultStat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblGagne;
        private System.Windows.Forms.Label lblPerdu;
        private System.Windows.Forms.Label lblPourcentage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblScore;
    }
}