namespace BusinessTourHack
{
    partial class BusinessTourHack
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.MoneyAdd = new System.Windows.Forms.Button();
            this.MoneyTrackBar = new System.Windows.Forms.TrackBar();
            this.PlayersListBox = new System.Windows.Forms.ListBox();
            this.MoneyRemove = new System.Windows.Forms.Button();
            this.MoneyAmount = new System.Windows.Forms.NumericUpDown();
            this.AboutButton = new System.Windows.Forms.Button();
            this.UpdateAddress = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.MoneyTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoneyAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(13, 5);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(217, 20);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.Text = "Hack status...";
            this.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MoneyAdd
            // 
            this.MoneyAdd.Location = new System.Drawing.Point(127, 162);
            this.MoneyAdd.Name = "MoneyAdd";
            this.MoneyAdd.Size = new System.Drawing.Size(102, 23);
            this.MoneyAdd.TabIndex = 0;
            this.MoneyAdd.Text = "Add Money";
            this.MoneyAdd.UseVisualStyleBackColor = true;
            this.MoneyAdd.Click += new System.EventHandler(this.MoneyAdd_Click);
            // 
            // MoneyTrackBar
            // 
            this.MoneyTrackBar.Location = new System.Drawing.Point(12, 93);
            this.MoneyTrackBar.Minimum = 1;
            this.MoneyTrackBar.Name = "MoneyTrackBar";
            this.MoneyTrackBar.Size = new System.Drawing.Size(217, 45);
            this.MoneyTrackBar.TabIndex = 1;
            this.MoneyTrackBar.Value = 1;
            this.MoneyTrackBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // PlayersListBox
            // 
            this.PlayersListBox.FormattingEnabled = true;
            this.PlayersListBox.Items.AddRange(new object[] {
            "Bottom Right Player ",
            "Bottom Left Player",
            "Top Left Player",
            "Top Right Player"});
            this.PlayersListBox.Location = new System.Drawing.Point(14, 31);
            this.PlayersListBox.Name = "PlayersListBox";
            this.PlayersListBox.Size = new System.Drawing.Size(216, 56);
            this.PlayersListBox.TabIndex = 2;
            this.PlayersListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // MoneyRemove
            // 
            this.MoneyRemove.Location = new System.Drawing.Point(127, 188);
            this.MoneyRemove.Name = "MoneyRemove";
            this.MoneyRemove.Size = new System.Drawing.Size(102, 23);
            this.MoneyRemove.TabIndex = 3;
            this.MoneyRemove.Text = "Remove Money";
            this.MoneyRemove.UseVisualStyleBackColor = true;
            this.MoneyRemove.Click += new System.EventHandler(this.MoneyRemove_Click);
            // 
            // MoneyAmount
            // 
            this.MoneyAmount.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MoneyAmount.Location = new System.Drawing.Point(14, 162);
            this.MoneyAmount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.MoneyAmount.Name = "MoneyAmount";
            this.MoneyAmount.Size = new System.Drawing.Size(107, 20);
            this.MoneyAmount.TabIndex = 4;
            // 
            // AboutButton
            // 
            this.AboutButton.Location = new System.Drawing.Point(14, 188);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(107, 23);
            this.AboutButton.TabIndex = 5;
            this.AboutButton.Text = "About ";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // UpdateAddress
            // 
            this.UpdateAddress.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UpdateAddress_DoWork);
            // 
            // BusinessTourHack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 223);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.MoneyAmount);
            this.Controls.Add(this.MoneyRemove);
            this.Controls.Add(this.PlayersListBox);
            this.Controls.Add(this.MoneyTrackBar);
            this.Controls.Add(this.MoneyAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BusinessTourHack";
            this.Text = "Business Tour Hack";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MoneyTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoneyAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.Button MoneyAdd;
        private System.Windows.Forms.TrackBar MoneyTrackBar;
        private System.Windows.Forms.ListBox PlayersListBox;
        private System.Windows.Forms.Button MoneyRemove;
        private System.Windows.Forms.NumericUpDown MoneyAmount;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.TextBox txtStatus;
        private System.ComponentModel.BackgroundWorker UpdateAddress;
    }
}

