namespace TitleCleanerGui
{
    partial class Settings
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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.flowLayoutButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBoxTvdb = new System.Windows.Forms.CheckBox();
            this.textBoxOutputDir = new System.Windows.Forms.TextBox();
            this.labelOutput = new System.Windows.Forms.Label();
            this.checkBoxConfirmations = new System.Windows.Forms.CheckBox();
            this.textBoxCommonName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTvName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonDirectory = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelInputDirDesc = new System.Windows.Forms.Label();
            this.buttonInputDir = new System.Windows.Forms.Button();
            this.labelInputDir = new System.Windows.Forms.Label();
            this.textBoxInputDir = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.labelDirFiles = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxFileType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.groupBoxTv = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTvFolder = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBoxMovie = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxMovieFolder = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxMovieName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            this.flowLayoutButtonsPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxTv.SuspendLayout();
            this.groupBoxMovie.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.flowLayoutButtonsPanel);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 397);
            this.panelButtons.MaximumSize = new System.Drawing.Size(0, 30);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(573, 30);
            this.panelButtons.TabIndex = 0;
            // 
            // flowLayoutButtonsPanel
            // 
            this.flowLayoutButtonsPanel.Controls.Add(this.buttonCancel);
            this.flowLayoutButtonsPanel.Controls.Add(this.buttonOk);
            this.flowLayoutButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutButtonsPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutButtonsPanel.MaximumSize = new System.Drawing.Size(0, 30);
            this.flowLayoutButtonsPanel.Name = "flowLayoutButtonsPanel";
            this.flowLayoutButtonsPanel.Size = new System.Drawing.Size(573, 30);
            this.flowLayoutButtonsPanel.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(495, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 25);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(414, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 25);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // checkBoxTvdb
            // 
            this.checkBoxTvdb.AutoSize = true;
            this.checkBoxTvdb.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxTvdb.Location = new System.Drawing.Point(35, 70);
            this.checkBoxTvdb.Name = "checkBoxTvdb";
            this.checkBoxTvdb.Size = new System.Drawing.Size(76, 18);
            this.checkBoxTvdb.TabIndex = 2;
            this.checkBoxTvdb.Text = "Use TVDB";
            this.checkBoxTvdb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxTvdb.UseVisualStyleBackColor = true;
            this.checkBoxTvdb.CheckedChanged += new System.EventHandler(this.CheckBoxTvdbCheckedChanged);
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.Location = new System.Drawing.Point(98, 125);
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.ReadOnly = true;
            this.textBoxOutputDir.Size = new System.Drawing.Size(102, 20);
            this.textBoxOutputDir.TabIndex = 4;
            this.textBoxOutputDir.Text = "None";
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(7, 128);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(86, 14);
            this.labelOutput.TabIndex = 6;
            this.labelOutput.Text = "Output Directory";
            this.labelOutput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxConfirmations
            // 
            this.checkBoxConfirmations.AutoSize = true;
            this.checkBoxConfirmations.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxConfirmations.Location = new System.Drawing.Point(20, 151);
            this.checkBoxConfirmations.Name = "checkBoxConfirmations";
            this.checkBoxConfirmations.Size = new System.Drawing.Size(92, 18);
            this.checkBoxConfirmations.TabIndex = 7;
            this.checkBoxConfirmations.Text = "Confirmations";
            this.checkBoxConfirmations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxConfirmations.UseVisualStyleBackColor = true;
            this.checkBoxConfirmations.CheckedChanged += new System.EventHandler(this.CheckBoxConfirmationsCheckedChanged);
            // 
            // textBoxCommonName
            // 
            this.textBoxCommonName.Location = new System.Drawing.Point(98, 73);
            this.textBoxCommonName.Name = "textBoxCommonName";
            this.textBoxCommonName.Size = new System.Drawing.Size(127, 20);
            this.textBoxCommonName.TabIndex = 14;
            this.textBoxCommonName.TextChanged += new System.EventHandler(this.TextBoxCommonNameTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 14);
            this.label5.TabIndex = 13;
            this.label5.Text = "Common Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTvName
            // 
            this.textBoxTvName.Location = new System.Drawing.Point(98, 18);
            this.textBoxTvName.Name = "textBoxTvName";
            this.textBoxTvName.Size = new System.Drawing.Size(124, 20);
            this.textBoxTvName.TabIndex = 10;
            this.textBoxTvName.TextChanged += new System.EventHandler(this.TextBoxTvNameTextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 14);
            this.label3.TabIndex = 9;
            this.label3.Text = "TV Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonDirectory
            // 
            this.buttonDirectory.Location = new System.Drawing.Point(200, 124);
            this.buttonDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDirectory.Name = "buttonDirectory";
            this.buttonDirectory.Size = new System.Drawing.Size(25, 22);
            this.buttonDirectory.TabIndex = 9;
            this.buttonDirectory.Text = "...";
            this.buttonDirectory.UseVisualStyleBackColor = true;
            this.buttonDirectory.Click += new System.EventHandler(this.ButtonDirectoryClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelInputDirDesc);
            this.groupBox2.Controls.Add(this.buttonInputDir);
            this.groupBox2.Controls.Add(this.labelInputDir);
            this.groupBox2.Controls.Add(this.textBoxInputDir);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.labelDirFiles);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBoxCommonName);
            this.groupBox2.Controls.Add(this.buttonDirectory);
            this.groupBox2.Controls.Add(this.comboBoxFileType);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.checkBoxConfirmations);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.labelOutput);
            this.groupBox2.Controls.Add(this.comboBoxMode);
            this.groupBox2.Controls.Add(this.textBoxOutputDir);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(549, 178);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // labelInputDirDesc
            // 
            this.labelInputDirDesc.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInputDirDesc.Location = new System.Drawing.Point(231, 94);
            this.labelInputDirDesc.Name = "labelInputDirDesc";
            this.labelInputDirDesc.Size = new System.Drawing.Size(312, 30);
            this.labelInputDirDesc.TabIndex = 27;
            this.labelInputDirDesc.Text = "Directory to read files from.";
            this.labelInputDirDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonInputDir
            // 
            this.buttonInputDir.Location = new System.Drawing.Point(200, 98);
            this.buttonInputDir.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInputDir.Name = "buttonInputDir";
            this.buttonInputDir.Size = new System.Drawing.Size(25, 22);
            this.buttonInputDir.TabIndex = 26;
            this.buttonInputDir.Text = "...";
            this.buttonInputDir.UseVisualStyleBackColor = true;
            this.buttonInputDir.Click += new System.EventHandler(this.ButtonInputDirClick);
            // 
            // labelInputDir
            // 
            this.labelInputDir.Location = new System.Drawing.Point(6, 102);
            this.labelInputDir.Name = "labelInputDir";
            this.labelInputDir.Size = new System.Drawing.Size(87, 14);
            this.labelInputDir.TabIndex = 25;
            this.labelInputDir.Text = "Input Directory";
            this.labelInputDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxInputDir
            // 
            this.textBoxInputDir.Location = new System.Drawing.Point(98, 99);
            this.textBoxInputDir.Name = "textBoxInputDir";
            this.textBoxInputDir.ReadOnly = true;
            this.textBoxInputDir.Size = new System.Drawing.Size(102, 20);
            this.textBoxInputDir.TabIndex = 24;
            this.textBoxInputDir.Text = "Current Directory";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(231, 144);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(312, 30);
            this.label11.TabIndex = 23;
            this.label11.Text = "Ask for confirmations before performing actions.";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDirFiles
            // 
            this.labelDirFiles.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDirFiles.Location = new System.Drawing.Point(231, 120);
            this.labelDirFiles.Name = "labelDirFiles";
            this.labelDirFiles.Size = new System.Drawing.Size(312, 30);
            this.labelDirFiles.TabIndex = 22;
            this.labelDirFiles.Text = "Directory to move files to.";
            this.labelDirFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(231, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(312, 30);
            this.label9.TabIndex = 21;
            this.label9.Text = "Name to rename files to. Options: L=Location, O=OrigionalName, C=CleanedName, E=F" +
    "ileExtension, Y=FileYear (current if unknown), \\=EscapeNext";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(231, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(312, 30);
            this.label4.TabIndex = 20;
            this.label4.Text = "Automatic selects based on what the program thinks your files are (its not always" +
    " correct!),\r\nTV assumes all files are TV Shows, Movie assumes all files are Movi" +
    "es.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(231, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(312, 22);
            this.label1.TabIndex = 19;
            this.label1.Text = "Normal renames files, test runs through test cases to check what their outcomes w" +
    "ould be.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 14);
            this.label7.TabIndex = 18;
            this.label7.Text = "File Type";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxFileType
            // 
            this.comboBoxFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFileType.FormattingEnabled = true;
            this.comboBoxFileType.Items.AddRange(new object[] {
            "Automatic",
            "TV",
            "Movie"});
            this.comboBoxFileType.Location = new System.Drawing.Point(98, 45);
            this.comboBoxFileType.Name = "comboBoxFileType";
            this.comboBoxFileType.Size = new System.Drawing.Size(127, 22);
            this.comboBoxFileType.TabIndex = 17;
            this.comboBoxFileType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxFileTypeSelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 14);
            this.label6.TabIndex = 16;
            this.label6.Text = "Mode";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "Normal - Multiple Files",
            "Normal - Single File",
            "Test"});
            this.comboBoxMode.Location = new System.Drawing.Point(98, 18);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(127, 22);
            this.comboBoxMode.TabIndex = 15;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.ComboBoxModeSelectedIndexChanged);
            // 
            // groupBoxTv
            // 
            this.groupBoxTv.Controls.Add(this.label2);
            this.groupBoxTv.Controls.Add(this.textBoxTvFolder);
            this.groupBoxTv.Controls.Add(this.label10);
            this.groupBoxTv.Controls.Add(this.label13);
            this.groupBoxTv.Controls.Add(this.label12);
            this.groupBoxTv.Controls.Add(this.textBoxTvName);
            this.groupBoxTv.Controls.Add(this.label3);
            this.groupBoxTv.Controls.Add(this.checkBoxTvdb);
            this.groupBoxTv.Location = new System.Drawing.Point(12, 193);
            this.groupBoxTv.Name = "groupBoxTv";
            this.groupBoxTv.Size = new System.Drawing.Size(549, 99);
            this.groupBoxTv.TabIndex = 11;
            this.groupBoxTv.TabStop = false;
            this.groupBoxTv.Text = "TV";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(231, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(312, 30);
            this.label2.TabIndex = 27;
            this.label2.Text = "Directory to store moved TV files in. Special functions: |=Path Separator, ts(x)=" +
    "Name where x is options from TV Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxTvFolder
            // 
            this.textBoxTvFolder.Location = new System.Drawing.Point(98, 44);
            this.textBoxTvFolder.Name = "textBoxTvFolder";
            this.textBoxTvFolder.Size = new System.Drawing.Size(124, 20);
            this.textBoxTvFolder.TabIndex = 26;
            this.textBoxTvFolder.TextChanged += new System.EventHandler(this.TextBoxTvFolderTextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(39, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 14);
            this.label10.TabIndex = 25;
            this.label10.Text = "TV Folder";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(231, 63);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(312, 30);
            this.label13.TabIndex = 24;
            this.label13.Text = "Use the TVDB to get missing titles.";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(231, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(312, 30);
            this.label12.TabIndex = 23;
            this.label12.Text = "Same as Common Name with extra options: T=Title, N=Name, S=SeasonNum, e=EpisodeNu" +
    "ms";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxMovie
            // 
            this.groupBoxMovie.Controls.Add(this.label15);
            this.groupBoxMovie.Controls.Add(this.textBoxMovieFolder);
            this.groupBoxMovie.Controls.Add(this.label16);
            this.groupBoxMovie.Controls.Add(this.label14);
            this.groupBoxMovie.Controls.Add(this.textBoxMovieName);
            this.groupBoxMovie.Controls.Add(this.label8);
            this.groupBoxMovie.Location = new System.Drawing.Point(12, 295);
            this.groupBoxMovie.Name = "groupBoxMovie";
            this.groupBoxMovie.Size = new System.Drawing.Size(549, 73);
            this.groupBoxMovie.TabIndex = 12;
            this.groupBoxMovie.TabStop = false;
            this.groupBoxMovie.Text = "Movie";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(231, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(312, 30);
            this.label15.TabIndex = 28;
            this.label15.Text = "Directory to store moved Movie files in.  Special functions: |=Path Separator, ts" +
    "(x)=Name where x is options from Movie Name\r\n";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxMovieFolder
            // 
            this.textBoxMovieFolder.Location = new System.Drawing.Point(98, 43);
            this.textBoxMovieFolder.Name = "textBoxMovieFolder";
            this.textBoxMovieFolder.Size = new System.Drawing.Size(124, 20);
            this.textBoxMovieFolder.TabIndex = 27;
            this.textBoxMovieFolder.TextChanged += new System.EventHandler(this.TextBoxMovieFolderTextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(25, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(68, 14);
            this.label16.TabIndex = 26;
            this.label16.Text = "Movie Folder";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(231, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(312, 30);
            this.label14.TabIndex = 25;
            this.label14.Text = "Same as Common Name.";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxMovieName
            // 
            this.textBoxMovieName.Location = new System.Drawing.Point(98, 18);
            this.textBoxMovieName.Name = "textBoxMovieName";
            this.textBoxMovieName.Size = new System.Drawing.Size(124, 20);
            this.textBoxMovieName.TabIndex = 10;
            this.textBoxMovieName.TextChanged += new System.EventHandler(this.TextBoxMovieNameTextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 14);
            this.label8.TabIndex = 9;
            this.label8.Text = "Movie Name";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(573, 427);
            this.Controls.Add(this.groupBoxMovie);
            this.Controls.Add(this.groupBoxTv);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panelButtons);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.panelButtons.ResumeLayout(false);
            this.flowLayoutButtonsPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxTv.ResumeLayout(false);
            this.groupBoxTv.PerformLayout();
            this.groupBoxMovie.ResumeLayout(false);
            this.groupBoxMovie.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutButtonsPanel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBoxTvdb;
        private System.Windows.Forms.TextBox textBoxOutputDir;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.CheckBox checkBoxConfirmations;
        private System.Windows.Forms.TextBox textBoxCommonName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTvName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonDirectory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxFileType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.GroupBox groupBoxTv;
        private System.Windows.Forms.GroupBox groupBoxMovie;
        private System.Windows.Forms.TextBox textBoxMovieName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelDirFiles;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelInputDirDesc;
        private System.Windows.Forms.Button buttonInputDir;
        private System.Windows.Forms.Label labelInputDir;
        private System.Windows.Forms.TextBox textBoxInputDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTvFolder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxMovieFolder;
        private System.Windows.Forms.Label label16;
    }
}