namespace Photo_Organizer
{
    partial class Form1
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
            this.buttonSource = new System.Windows.Forms.Button();
            this.buttonDestination = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelDestination = new System.Windows.Forms.Label();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.labelMinPhotosPerFolder = new System.Windows.Forms.Label();
            this.textBoxMinPhotosPerFolder = new System.Windows.Forms.TextBox();
            this.textBoxMinPhotosDays = new System.Windows.Forms.TextBox();
            this.labelMinPhotosDays = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSource
            // 
            this.buttonSource.Location = new System.Drawing.Point(237, 32);
            this.buttonSource.Name = "buttonSource";
            this.buttonSource.Size = new System.Drawing.Size(36, 23);
            this.buttonSource.TabIndex = 0;
            this.buttonSource.Text = "...";
            this.buttonSource.UseVisualStyleBackColor = true;
            this.buttonSource.Click += new System.EventHandler(this.buttonSource_Click);
            // 
            // buttonDestination
            // 
            this.buttonDestination.Location = new System.Drawing.Point(237, 92);
            this.buttonDestination.Name = "buttonDestination";
            this.buttonDestination.Size = new System.Drawing.Size(36, 23);
            this.buttonDestination.TabIndex = 1;
            this.buttonDestination.Text = "...";
            this.buttonDestination.UseVisualStyleBackColor = true;
            this.buttonDestination.Click += new System.EventHandler(this.buttonDestination_Click);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(32, 13);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(128, 13);
            this.labelSource.TabIndex = 2;
            this.labelSource.Text = "Choose the photos folder:";
            // 
            // labelDestination
            // 
            this.labelDestination.AutoSize = true;
            this.labelDestination.Location = new System.Drawing.Point(35, 76);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(238, 13);
            this.labelDestination.TabIndex = 3;
            this.labelDestination.Text = "Choose the destination for the processed photos:";
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(35, 35);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(196, 20);
            this.textBoxSource.TabIndex = 4;
            this.textBoxSource.Text = "C:\\Users\\patrick.tedeschi\\Desktop\\Test";
            // 
            // textBoxDestination
            // 
            this.textBoxDestination.Location = new System.Drawing.Point(35, 94);
            this.textBoxDestination.Name = "textBoxDestination";
            this.textBoxDestination.Size = new System.Drawing.Size(196, 20);
            this.textBoxDestination.TabIndex = 5;
            this.textBoxDestination.Text = "C:\\Users\\patrick.tedeschi\\Desktop\\Result";
            // 
            // buttonProcess
            // 
            this.buttonProcess.Location = new System.Drawing.Point(35, 226);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonProcess.TabIndex = 6;
            this.buttonProcess.Text = "Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // labelMinPhotosPerFolder
            // 
            this.labelMinPhotosPerFolder.AutoSize = true;
            this.labelMinPhotosPerFolder.Location = new System.Drawing.Point(35, 134);
            this.labelMinPhotosPerFolder.Name = "labelMinPhotosPerFolder";
            this.labelMinPhotosPerFolder.Size = new System.Drawing.Size(130, 13);
            this.labelMinPhotosPerFolder.TabIndex = 7;
            this.labelMinPhotosPerFolder.Text = "Minimum photos per folder";
            // 
            // textBoxMinPhotosPerFolder
            // 
            this.textBoxMinPhotosPerFolder.Location = new System.Drawing.Point(35, 151);
            this.textBoxMinPhotosPerFolder.Name = "textBoxMinPhotosPerFolder";
            this.textBoxMinPhotosPerFolder.Size = new System.Drawing.Size(29, 20);
            this.textBoxMinPhotosPerFolder.TabIndex = 8;
            this.textBoxMinPhotosPerFolder.Text = "7";
            // 
            // textBoxMinPhotosDays
            // 
            this.textBoxMinPhotosDays.Location = new System.Drawing.Point(35, 191);
            this.textBoxMinPhotosDays.Name = "textBoxMinPhotosDays";
            this.textBoxMinPhotosDays.Size = new System.Drawing.Size(29, 20);
            this.textBoxMinPhotosDays.TabIndex = 10;
            this.textBoxMinPhotosDays.Text = "1";
            // 
            // labelMinPhotosDays
            // 
            this.labelMinPhotosDays.AutoSize = true;
            this.labelMinPhotosDays.Location = new System.Drawing.Point(35, 174);
            this.labelMinPhotosDays.Name = "labelMinPhotosDays";
            this.labelMinPhotosDays.Size = new System.Drawing.Size(152, 13);
            this.labelMinPhotosDays.TabIndex = 9;
            this.labelMinPhotosDays.Text = "Agragate folders minimum days";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 261);
            this.Controls.Add(this.textBoxMinPhotosDays);
            this.Controls.Add(this.labelMinPhotosDays);
            this.Controls.Add(this.textBoxMinPhotosPerFolder);
            this.Controls.Add(this.labelMinPhotosPerFolder);
            this.Controls.Add(this.buttonProcess);
            this.Controls.Add(this.textBoxDestination);
            this.Controls.Add(this.textBoxSource);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.buttonDestination);
            this.Controls.Add(this.buttonSource);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSource;
        private System.Windows.Forms.Button buttonDestination;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelDestination;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.Label labelMinPhotosPerFolder;
        private System.Windows.Forms.TextBox textBoxMinPhotosPerFolder;
        private System.Windows.Forms.TextBox textBoxMinPhotosDays;
        private System.Windows.Forms.Label labelMinPhotosDays;
    }
}

