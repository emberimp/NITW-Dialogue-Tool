namespace NITW_Dialogue_Tool
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSetup = new System.Windows.Forms.Button();
            this.txtNITWpath = new System.Windows.Forms.TextBox();
            this.btnDebugMode = new System.Windows.Forms.Button();
            this.btnResetSA8 = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnWatcher = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.labelPath = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labelWatcher = new System.Windows.Forms.Label();
            this.tabFiles = new System.Windows.Forms.TabPage();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.columnArchive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnEdited = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnLastModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOpen = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnWrite = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnReset = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnFiller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageMore = new System.Windows.Forms.TabPage();
            this.btnFindEditorPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEditorPath = new System.Windows.Forms.TextBox();
            this.cbEditor = new System.Windows.Forms.CheckBox();
            this.btnFindNITWPath = new System.Windows.Forms.Button();
            this.labelDisableDebugMode = new System.Windows.Forms.Label();
            this.btnDisableDebugMode = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTest = new System.Windows.Forms.TextBox();
            this.linkLabelDebugMode = new System.Windows.Forms.LinkLabel();
            this.labelRestore = new System.Windows.Forms.Label();
            this.labelDebugMode = new System.Windows.Forms.Label();
            this.labelSetup = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelEmberimp = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.tabPageMore.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(8, 149);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(156, 23);
            this.btnSetup.TabIndex = 0;
            this.btnSetup.Text = "Run Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // txtNITWpath
            // 
            this.txtNITWpath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNITWpath.Location = new System.Drawing.Point(8, 23);
            this.txtNITWpath.Name = "txtNITWpath";
            this.txtNITWpath.Size = new System.Drawing.Size(706, 20);
            this.txtNITWpath.TabIndex = 1;
            // 
            // btnDebugMode
            // 
            this.btnDebugMode.Location = new System.Drawing.Point(8, 178);
            this.btnDebugMode.Name = "btnDebugMode";
            this.btnDebugMode.Size = new System.Drawing.Size(156, 23);
            this.btnDebugMode.TabIndex = 2;
            this.btnDebugMode.Text = "Enable Debug Mode";
            this.btnDebugMode.UseVisualStyleBackColor = true;
            this.btnDebugMode.Click += new System.EventHandler(this.btnDebugMode_Click);
            // 
            // btnResetSA8
            // 
            this.btnResetSA8.Location = new System.Drawing.Point(294, 44);
            this.btnResetSA8.Name = "btnResetSA8";
            this.btnResetSA8.Size = new System.Drawing.Size(136, 23);
            this.btnResetSA8.TabIndex = 3;
            this.btnResetSA8.Text = "reset sharedassets8";
            this.btnResetSA8.UseVisualStyleBackColor = true;
            this.btnResetSA8.Click += new System.EventHandler(this.btnResetSA8_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(103, 44);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(114, 23);
            this.btnWrite.TabIndex = 4;
            this.btnWrite.Text = "write Busstation.yarn";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(6, 44);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(91, 23);
            this.btnTest.TabIndex = 7;
            this.btnTest.Text = "read txtbox file";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(8, 35);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(838, 308);
            this.txtLog.TabIndex = 8;
            // 
            // btnWatcher
            // 
            this.btnWatcher.Location = new System.Drawing.Point(6, 6);
            this.btnWatcher.Name = "btnWatcher";
            this.btnWatcher.Size = new System.Drawing.Size(156, 23);
            this.btnWatcher.TabIndex = 9;
            this.btnWatcher.Text = "Enable File Watcher";
            this.btnWatcher.UseVisualStyleBackColor = true;
            this.btnWatcher.Click += new System.EventHandler(this.btnWatcher_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(8, 236);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(156, 23);
            this.btnRestore.TabIndex = 10;
            this.btnRestore.Text = "Restore All Files";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(6, 7);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(141, 13);
            this.labelPath.TabIndex = 11;
            this.labelPath.Text = "Night in the Woods directory";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMain);
            this.tabControl1.Controls.Add(this.tabFiles);
            this.tabControl1.Controls.Add(this.tabPageMore);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.MinimumSize = new System.Drawing.Size(763, 382);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(806, 382);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.linkLabel1);
            this.tabPageMain.Controls.Add(this.labelWatcher);
            this.tabPageMain.Controls.Add(this.txtLog);
            this.tabPageMain.Controls.Add(this.btnWatcher);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(798, 356);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(599, 11);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(247, 13);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "nightinthewoods.gamepedia.com/Editing_Dialogue";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked_1);
            // 
            // labelWatcher
            // 
            this.labelWatcher.BackColor = System.Drawing.Color.Tomato;
            this.labelWatcher.Location = new System.Drawing.Point(168, 6);
            this.labelWatcher.Name = "labelWatcher";
            this.labelWatcher.Size = new System.Drawing.Size(111, 23);
            this.labelWatcher.TabIndex = 10;
            this.labelWatcher.Text = "File Watcher Disabled";
            this.labelWatcher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabFiles
            // 
            this.tabFiles.Controls.Add(this.dgvFiles);
            this.tabFiles.Location = new System.Drawing.Point(4, 22);
            this.tabFiles.Name = "tabFiles";
            this.tabFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabFiles.Size = new System.Drawing.Size(798, 356);
            this.tabFiles.TabIndex = 2;
            this.tabFiles.Text = "Files";
            this.tabFiles.UseVisualStyleBackColor = true;
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dgvFiles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnArchive,
            this.columnFile,
            this.columnEdited,
            this.columnLastModified,
            this.columnOpen,
            this.columnWrite,
            this.columnReset,
            this.columnFiller});
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFiles.Location = new System.Drawing.Point(3, 3);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.Size = new System.Drawing.Size(792, 350);
            this.dgvFiles.TabIndex = 0;
            this.dgvFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFiles_CellContentClick_1);
            // 
            // columnArchive
            // 
            this.columnArchive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnArchive.HeaderText = "SA";
            this.columnArchive.Name = "columnArchive";
            this.columnArchive.ReadOnly = true;
            this.columnArchive.ToolTipText = "Where the file was found: sharedassetsX.assets";
            this.columnArchive.Width = 46;
            // 
            // columnFile
            // 
            this.columnFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnFile.HeaderText = "File Name";
            this.columnFile.Name = "columnFile";
            this.columnFile.ReadOnly = true;
            this.columnFile.Width = 79;
            // 
            // columnEdited
            // 
            this.columnEdited.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnEdited.HeaderText = "Edited";
            this.columnEdited.Name = "columnEdited";
            this.columnEdited.ReadOnly = true;
            this.columnEdited.Width = 43;
            // 
            // columnLastModified
            // 
            this.columnLastModified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnLastModified.HeaderText = "Last Modified";
            this.columnLastModified.Name = "columnLastModified";
            this.columnLastModified.ReadOnly = true;
            this.columnLastModified.Width = 95;
            // 
            // columnOpen
            // 
            this.columnOpen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnOpen.HeaderText = "Open";
            this.columnOpen.Name = "columnOpen";
            this.columnOpen.Width = 39;
            // 
            // columnWrite
            // 
            this.columnWrite.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnWrite.HeaderText = "Write";
            this.columnWrite.Name = "columnWrite";
            this.columnWrite.Width = 38;
            // 
            // columnReset
            // 
            this.columnReset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnReset.HeaderText = "Reset";
            this.columnReset.Name = "columnReset";
            this.columnReset.Width = 41;
            // 
            // columnFiller
            // 
            this.columnFiller.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnFiller.HeaderText = "File Name again";
            this.columnFiller.Name = "columnFiller";
            this.columnFiller.ReadOnly = true;
            // 
            // tabPageMore
            // 
            this.tabPageMore.Controls.Add(this.btnFindEditorPath);
            this.tabPageMore.Controls.Add(this.label1);
            this.tabPageMore.Controls.Add(this.txtEditorPath);
            this.tabPageMore.Controls.Add(this.cbEditor);
            this.tabPageMore.Controls.Add(this.btnFindNITWPath);
            this.tabPageMore.Controls.Add(this.labelDisableDebugMode);
            this.tabPageMore.Controls.Add(this.btnDisableDebugMode);
            this.tabPageMore.Controls.Add(this.groupBox1);
            this.tabPageMore.Controls.Add(this.linkLabelDebugMode);
            this.tabPageMore.Controls.Add(this.labelRestore);
            this.tabPageMore.Controls.Add(this.labelDebugMode);
            this.tabPageMore.Controls.Add(this.labelSetup);
            this.tabPageMore.Controls.Add(this.labelVersion);
            this.tabPageMore.Controls.Add(this.labelEmberimp);
            this.tabPageMore.Controls.Add(this.btnSaveSettings);
            this.tabPageMore.Controls.Add(this.labelPath);
            this.tabPageMore.Controls.Add(this.txtNITWpath);
            this.tabPageMore.Controls.Add(this.btnDebugMode);
            this.tabPageMore.Controls.Add(this.btnRestore);
            this.tabPageMore.Controls.Add(this.btnSetup);
            this.tabPageMore.Location = new System.Drawing.Point(4, 22);
            this.tabPageMore.Name = "tabPageMore";
            this.tabPageMore.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMore.Size = new System.Drawing.Size(798, 356);
            this.tabPageMore.TabIndex = 1;
            this.tabPageMore.Text = "More";
            this.tabPageMore.UseVisualStyleBackColor = true;
            // 
            // btnFindEditorPath
            // 
            this.btnFindEditorPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindEditorPath.Location = new System.Drawing.Point(720, 61);
            this.btnFindEditorPath.Name = "btnFindEditorPath";
            this.btnFindEditorPath.Size = new System.Drawing.Size(67, 23);
            this.btnFindEditorPath.TabIndex = 27;
            this.btnFindEditorPath.Text = "...";
            this.btnFindEditorPath.UseVisualStyleBackColor = true;
            this.btnFindEditorPath.Click += new System.EventHandler(this.btnFindEditorPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Text editor";
            // 
            // txtEditorPath
            // 
            this.txtEditorPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEditorPath.Location = new System.Drawing.Point(8, 62);
            this.txtEditorPath.Name = "txtEditorPath";
            this.txtEditorPath.Size = new System.Drawing.Size(706, 20);
            this.txtEditorPath.TabIndex = 25;
            // 
            // cbEditor
            // 
            this.cbEditor.Location = new System.Drawing.Point(9, 86);
            this.cbEditor.Name = "cbEditor";
            this.cbEditor.Size = new System.Drawing.Size(116, 17);
            this.cbEditor.TabIndex = 0;
            this.cbEditor.Text = "Use built-in editor";
            this.cbEditor.UseVisualStyleBackColor = true;
            this.cbEditor.CheckedChanged += new System.EventHandler(this.cbEditor_CheckedChanged);
            // 
            // btnFindNITWPath
            // 
            this.btnFindNITWPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNITWPath.Location = new System.Drawing.Point(720, 21);
            this.btnFindNITWPath.Name = "btnFindNITWPath";
            this.btnFindNITWPath.Size = new System.Drawing.Size(67, 23);
            this.btnFindNITWPath.TabIndex = 23;
            this.btnFindNITWPath.Text = "...";
            this.btnFindNITWPath.UseVisualStyleBackColor = true;
            this.btnFindNITWPath.Click += new System.EventHandler(this.btnFindNITWPath_Click);
            // 
            // labelDisableDebugMode
            // 
            this.labelDisableDebugMode.AutoSize = true;
            this.labelDisableDebugMode.Location = new System.Drawing.Point(170, 212);
            this.labelDisableDebugMode.Name = "labelDisableDebugMode";
            this.labelDisableDebugMode.Size = new System.Drawing.Size(176, 13);
            this.labelDisableDebugMode.TabIndex = 22;
            this.labelDisableDebugMode.Text = "Restores the original UnityEngine.dll";
            // 
            // btnDisableDebugMode
            // 
            this.btnDisableDebugMode.Location = new System.Drawing.Point(8, 207);
            this.btnDisableDebugMode.Name = "btnDisableDebugMode";
            this.btnDisableDebugMode.Size = new System.Drawing.Size(156, 23);
            this.btnDisableDebugMode.TabIndex = 21;
            this.btnDisableDebugMode.Text = "Disable Debug Mode";
            this.btnDisableDebugMode.UseVisualStyleBackColor = true;
            this.btnDisableDebugMode.Click += new System.EventHandler(this.btnDisableDebugMode_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTest);
            this.groupBox1.Controls.Add(this.btnWrite);
            this.groupBox1.Controls.Add(this.btnResetSA8);
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Location = new System.Drawing.Point(10, 265);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(604, 75);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Testing";
            this.groupBox1.Visible = false;
            // 
            // txtTest
            // 
            this.txtTest.Location = new System.Drawing.Point(6, 18);
            this.txtTest.Name = "txtTest";
            this.txtTest.Size = new System.Drawing.Size(587, 20);
            this.txtTest.TabIndex = 18;
            this.txtTest.Text = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Night in the Woods\\Night in the Woo" +
    "ds_Data\\sharedassets14.assets";
            // 
            // linkLabelDebugMode
            // 
            this.linkLabelDebugMode.AutoSize = true;
            this.linkLabelDebugMode.Location = new System.Drawing.Point(519, 183);
            this.linkLabelDebugMode.Name = "linkLabelDebugMode";
            this.linkLabelDebugMode.Size = new System.Drawing.Size(232, 13);
            this.linkLabelDebugMode.TabIndex = 19;
            this.linkLabelDebugMode.TabStop = true;
            this.linkLabelDebugMode.Text = "nightinthewoods.gamepedia.com/Debug_Mode";
            this.linkLabelDebugMode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labelRestore
            // 
            this.labelRestore.AutoSize = true;
            this.labelRestore.Location = new System.Drawing.Point(170, 241);
            this.labelRestore.Name = "labelRestore";
            this.labelRestore.Size = new System.Drawing.Size(358, 13);
            this.labelRestore.TabIndex = 17;
            this.labelRestore.Text = "Changes all yarn files back to their original content. All changes will be lost!";
            // 
            // labelDebugMode
            // 
            this.labelDebugMode.AutoSize = true;
            this.labelDebugMode.Location = new System.Drawing.Point(170, 183);
            this.labelDebugMode.Name = "labelDebugMode";
            this.labelDebugMode.Size = new System.Drawing.Size(350, 13);
            this.labelDebugMode.TabIndex = 16;
            this.labelDebugMode.Text = "Overwrites UnityEngine.dll in the NITW folder to enable the debug mode.";
            // 
            // labelSetup
            // 
            this.labelSetup.AutoSize = true;
            this.labelSetup.Location = new System.Drawing.Point(170, 154);
            this.labelSetup.Name = "labelSetup";
            this.labelSetup.Size = new System.Drawing.Size(482, 13);
            this.labelSetup.TabIndex = 15;
            this.labelSetup.Text = "Searches all sharedassets.assets files for yarn dialogue files. Run this if it\'s " +
    "your first time with this tool.";
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersion.Location = new System.Drawing.Point(667, 325);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(125, 13);
            this.labelVersion.TabIndex = 14;
            this.labelVersion.Text = "v0.0.0";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEmberimp
            // 
            this.labelEmberimp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEmberimp.Location = new System.Drawing.Point(664, 338);
            this.labelEmberimp.Name = "labelEmberimp";
            this.labelEmberimp.Size = new System.Drawing.Size(128, 13);
            this.labelEmberimp.TabIndex = 13;
            this.labelEmberimp.Text = "by emberimp";
            this.labelEmberimp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSaveSettings.Location = new System.Drawing.Point(8, 109);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(156, 23);
            this.btnSaveSettings.TabIndex = 12;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 382);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(822, 421);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NITW Dialogue Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.tabPageMore.ResumeLayout(false);
            this.tabPageMore.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.TextBox txtNITWpath;
        private System.Windows.Forms.Button btnDebugMode;
        private System.Windows.Forms.Button btnResetSA8;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnWatcher;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageMore;
        private System.Windows.Forms.Label labelEmberimp;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelRestore;
        private System.Windows.Forms.Label labelDebugMode;
        private System.Windows.Forms.Label labelSetup;
        private System.Windows.Forms.TextBox txtTest;
        private System.Windows.Forms.LinkLabel linkLabelDebugMode;
        private System.Windows.Forms.Label labelWatcher;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TabPage tabFiles;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArchive;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFile;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnEdited;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLastModified;
        private System.Windows.Forms.DataGridViewButtonColumn columnOpen;
        private System.Windows.Forms.DataGridViewButtonColumn columnWrite;
        private System.Windows.Forms.DataGridViewButtonColumn columnReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFiller;
        private System.Windows.Forms.Label labelDisableDebugMode;
        private System.Windows.Forms.Button btnDisableDebugMode;
        private System.Windows.Forms.Button btnFindNITWPath;
        private System.Windows.Forms.Button btnFindEditorPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEditorPath;
        private System.Windows.Forms.CheckBox cbEditor;
    }
}

