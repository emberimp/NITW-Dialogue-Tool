using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace NITW_Dialogue_Tool
{
    public partial class Form1 : Form
    {
        string version = "1.0.0";

        yarnDictionary rootz = JsonUtil.loadYarnDictionary();
        FileSystemWatcher watcher = new FileSystemWatcher();
        static TextBox txtLogStatic;
        bool setupRunning = false;

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        public void HideCaret(object sender, EventArgs e)
        {
            HideCaret(txtLog.Handle);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtLogStatic = txtLog;
            Log("Initialized", true);
            labelVersion.Text = "v" + version;
            CheckForIllegalCrossThreadCalls = false; //hm

            string yarnDir = AppDomain.CurrentDomain.BaseDirectory + @"\yarn files";
            if (!Directory.Exists(yarnDir))
            {
                Directory.CreateDirectory(yarnDir);
            }
            watcher.Path = AppDomain.CurrentDomain.BaseDirectory + @"\yarn files";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.yarn.txt*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = false;

            //hide caret in log textbox
            txtLog.GotFocus += HideCaret;

            //first time stuff
            if (String.IsNullOrEmpty(rootz.nitwPath))
            {
                Form1.Log("If this is your first time running this tool click \"Run Setup\" on the \"More\" tab.", true);
                txtNITWpath.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Steam\steamapps\common\Night in the Woods";
            }
            else
            {
                txtNITWpath.Text = rootz.nitwPath;
            }

            //init file tab
            fillDgvFiles(rootz.yarnFiles);

            //register dgvFiles click event
            dgvFiles.CellContentClick += dgvFiles_CellContentClick;

        }
        public static void Log(string message, bool twoNewlines = false)
        {
            txtLogStatic.AppendText("[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + message + "\r\n");

            if (twoNewlines)
            {
                txtLogStatic.AppendText("\r\n");
            }
        }

        private void dgvFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                string buttonText = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string yarnFileName = senderGrid.Rows[e.RowIndex].Cells["columnFile"].Value.ToString();
                if (buttonText.Contains("Open"))
                {
                    string yarnFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\yarn files\" + yarnFileName;
                    Process.Start(yarnFilePath);
                    dgvFiles.ClearSelection();
                }
                else if (buttonText.Contains("Write"))
                {
                    UnityAssetFile.write(yarnFileName.Replace(".txt", ""), ref rootz);
                    
                    updateDgvFilesRow(e.RowIndex);
                }
                else if (buttonText.Contains("Reset"))
                {
                    //tabControl1.SelectedIndex = 0;
                    watcher.EnableRaisingEvents = false;
                    updateWatcherButton();
                    tabControl1.Update();

                    restoryYarn(rootz.yarnFiles[yarnFileName.Replace(".txt", "")], true);
                    
                    updateDgvFilesRow(e.RowIndex);
                }
            }
        }

        private void fillDgvFiles(Dictionary<string,yarnFile> yarnFiles)
        {
            dgvFiles.Rows.Clear();

            foreach (KeyValuePair<string, yarnFile> yarn in yarnFiles)
            {
                int archiveNumber = Convert.ToInt32(Path.GetFileNameWithoutExtension(yarn.Value.assetsFilePath).Replace("sharedassets", ""));
                dgvFiles.Rows.Add(archiveNumber, yarn.Key + ".txt", yarn.Value.edited, yarn.Value.lastModified, "Open in editor", "Write to assets file", "Reset content", yarn.Key + ".txt");
            }

            if (dgvFiles.Rows.Count > 0)
            {
                btnRestore.Enabled = true;
            }
            else
            {
                dgvFiles.Rows.Add(0, "Please \"Run Setup\" on the \"More\" tab");
                btnRestore.Enabled = false;
            }
        }

        private void updateDgvFilesRow(int rowIndex)
        {
            string yarnKey = dgvFiles.Rows[rowIndex].Cells["columnFile"].Value.ToString().Replace(".txt", "");
            yarnFile yarn = rootz.yarnFiles[yarnKey];
            int archiveNumber = Convert.ToInt32(Path.GetFileNameWithoutExtension(yarn.assetsFilePath).Replace("sharedassets", ""));
            dgvFiles.Rows[rowIndex].SetValues(archiveNumber, yarnKey + ".txt", yarn.edited, yarn.lastModified);
        }

        private void setup(string nitwfolder)
        {
            setupRunning = true;
            Log("Setup started", true);

            string path = Path.Combine(nitwfolder + @"\Night in the Woods_Data");
            DirectoryInfo d = new DirectoryInfo(path);

            if (!d.Exists)
            {
                Log("Error: NITW folder not found", true);
                return;
            }

            string[] files = Directory.GetFiles(path, "sharedassets*.assets");
            Array.Sort(files, CompareNumeric.sortFunc);

            foreach (string file in files)
            {
                loadAssetFile(file, true);
            }
            JsonUtil.saveYarnDictionary(rootz);

            fillDgvFiles(rootz.yarnFiles);

            Log("Setup done");
            Log("Enable the file watcher and edit the files in the \"yarn files\" folder, any changes will be written to the sharedassets files automatically");
            Log("For more info see nightinthewoods.gamepedia.com/Editing_Dialogue", true);
            setupRunning = false;
        }
        
       

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (!setupRunning)
            {
                watcher.EnableRaisingEvents = false;
                System.Timers.Timer fwt = new System.Timers.Timer();
                fwt.Interval = 5;
                fwt.Elapsed += delegate {
                    watcher.EnableRaisingEvents = true;
                    fwt.Stop();
                };
                fwt.Start();

                string yarnFile = e.Name.Replace(".txt", "");

                Log("Detected change to " + e.Name, true);

                UnityAssetFile.write(yarnFile, ref rootz);

                DataGridViewRow row = dgvFiles.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["columnFile"].Value.ToString().Equals(e.Name))
                    .First();                

                updateDgvFilesRow(row.Index);
            }            
        }

        private void loadAssetFile(string filepath, bool setupMode = false)
        {
            Dictionary<string, yarnFile> yarnFiles = UnityAssetFile.read(filepath);

            foreach(KeyValuePair<string, yarnFile> entry in yarnFiles)
            {

                yarnFile yf = entry.Value;

                if (yf.yarnFileName.Length > 0)
                {
                    if (rootz.yarnFiles.ContainsKey(yf.yarnFileName))
                    {
                        rootz.yarnFiles[yf.yarnFileName] = yf;
                    }
                    else
                    {
                        rootz.yarnFiles.Add(yf.yarnFileName, yf);
                    }

                    if (!setupMode)
                    {
                        JsonUtil.saveYarnDictionary(rootz);
                    }
                }

            }
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {            
            //disable file watcher
            watcher.EnableRaisingEvents = false;
            updateWatcherButton();

            btnSetup.Enabled = false;
            btnWatcher.Enabled = false;

            tabControl1.SelectedIndex = 0;

            tabControl1.Update();

            rootz.nitwPath = txtNITWpath.Text;
            setup(txtNITWpath.Text);

            btnSetup.Enabled = true;
            btnWatcher.Enabled = true;
        }

        private void btnWrite_Click(object sender, EventArgs e) //for testing
        {
            btnWrite.Enabled = false;
            UnityAssetFile.write("BusStation.yarn", ref rootz);
            btnWrite.Enabled = true;
        }
        
        private void btnDebugMode_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes(Path.Combine(txtNITWpath.Text + @"\Night in the Woods_Data\Managed", "UnityEngine.dll"), Properties.Resources.UnityEngine_ForceDebugMode);

            tabControl1.SelectedIndex = 0;
            tabControl1.Update();

            Log("Modified UnityEngine.dll copied to NITW folder");
            Log("Press ` (back quote) ingame to show the debug menu");
            Log("Press TAB ingame to speed up the game");
            Log("See nightinthewoods.gamepedia.com/Debug_Mode for more info", true);
        }

        private void btnDisableDebugMode_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes(Path.Combine(txtNITWpath.Text + @"\Night in the Woods_Data\Managed", "UnityEngine.dll"), Properties.Resources.UnityEngine_Original);

            tabControl1.SelectedIndex = 0;
            tabControl1.Update();

            Log("Unmodified UnityEngine.dll copied to NITW folder", true);
        }

        private void btnResetSA8_Click(object sender, EventArgs e) //for testing
        {
            File.Copy(Path.Combine(@"D:\Program Files (x86)\Steam\steamapps\common\Night in the Woods\Night in the Woods_Data\#sharedassets", "sharedassets8_2.assets"), 
                Path.Combine(@"D:\Program Files (x86)\Steam\steamapps\common\Night in the Woods\Night in the Woods_Data", "sharedassets8.assets"), true);
        }
        
        private void btnTest_Click(object sender, EventArgs e) //for testing
        {
            loadAssetFile(txtTest.Text);
        }

        private void btnWatcher_Click(object sender, EventArgs e)
        {
            watcher.EnableRaisingEvents = !watcher.EnableRaisingEvents;

            updateWatcherButton();
        }

        private void updateWatcherButton()
        {
            if (watcher.EnableRaisingEvents)
            {
                Log("File watcher enabled", true);
                labelWatcher.Text = "File Watcher Enabled";
                labelWatcher.BackColor = Color.LightGreen;
                btnWatcher.Text = "Disable File Watcher";
            }
            else
            {
                Log("File watcher disabled", true);
                labelWatcher.Text = "File Watcher Disabled";
                labelWatcher.BackColor = Color.Tomato;
                btnWatcher.Text = "Enable File Watcher";
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            rootz.nitwPath = txtNITWpath.Text;
            JsonUtil.saveYarnDictionary(rootz);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("This will overwrite all .yarn.txt files! \r\nAre you sure?","Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tabControl1.SelectedIndex = 0;
                watcher.EnableRaisingEvents = false;
                updateWatcherButton();
                tabControl1.Update();

                Log("Restoring original dialogue content...", true);

                Dictionary<string, yarnFile> tempDic = new Dictionary<string, yarnFile>(rootz.yarnFiles);

                foreach (KeyValuePair<string, yarnFile> entry in tempDic)
                {
                    restoryYarn(entry.Value);
                }

                fillDgvFiles(rootz.yarnFiles);

                Log("Restoring original dialogue content done", true);
            }
        }

        private void restoryYarn(yarnFile f, bool log = false)
        {
            if (log)
            {
                Log("Restoring original dialogue content of " + f.yarnFileName + ".txt...", true);
            }

            string yarnDir = AppDomain.CurrentDomain.BaseDirectory + @"\yarn files";
            if (!Directory.Exists(yarnDir))
            {
                Directory.CreateDirectory(yarnDir);
            }

            string yarnPath = Path.Combine(yarnDir, f.yarnFileName) + ".txt";
            File.WriteAllText(yarnPath, f.originalContent);

            UnityAssetFile.write(f.yarnFileName, ref rootz);

            f.lastModified = File.GetLastWriteTime(yarnPath);
            f.edited = false;

            JsonUtil.saveYarnDictionary(rootz);

            if (log)
            {
                Log("Restoring original dialogue content of " + f.yarnFileName + ".txt done", true);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://nightinthewoods.gamepedia.com/Debug_Mode");
        }
        
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://nightinthewoods.gamepedia.com/Editing_Dialogue");
        }
    }

    class yarnDictionary
    {
        [JsonProperty("nitwPath")]
        public string nitwPath { get; set; }

        [JsonProperty("gregg_rootz_ok")]
        public Dictionary<string, yarnFile> yarnFiles { get; set; }
    }

    public class yarnFile
    {
        [JsonProperty("yarnFileName")]
        public string yarnFileName { get; set; }

        [JsonProperty("file")]
        public string assetsFilePath { get; set; }

        [JsonProperty("index")]
        public long index { get; set; }

        [JsonProperty("offset")]
        public uint offset { get; set; }

        [JsonProperty("length")]
        public uint length { get; set; }

        [JsonProperty("yarnPath")]
        public string yarnPath { get; set; }

        [JsonProperty("objectDataOffset")]
        public uint objectDataOffset { get; set; }

        [JsonProperty("objectInfoCount")]
        public int objectInfoCount { get; set; }

        [JsonProperty("objectInfoPosition")]
        public long objectInfoPosition { get; set; }

        [JsonProperty("lastModified")]
        public DateTime lastModified { get; set; }

        [JsonProperty("edited")]
        public bool edited { get; set; }

        [JsonProperty("originalContent")]
        public string originalContent { get; set; }

        public yarnFile()
        {
            this.yarnFileName = "";
        }

        public yarnFile(string assetsFilePathArg, long indexArg, uint offsetArg, uint lengthArg, string yarnFileNameArg, string originalContentArg, string yarnPathArg, DateTime lastModifiedArg, 
            uint objectDataOffsetArg, int objectInfoCountArg, long objectInfoPositionArg)
        {
            this.assetsFilePath = assetsFilePathArg;
            this.index = indexArg;
            this.offset = offsetArg;
            this.length = lengthArg;
            this.yarnFileName = yarnFileNameArg;
            this.yarnPath = yarnPathArg;
            this.originalContent = originalContentArg;
            this.lastModified = lastModifiedArg;
            this.edited = false;
            this.objectDataOffset = objectDataOffsetArg;
            this.objectInfoCount = objectInfoCountArg;
            this.objectInfoPosition = objectInfoPositionArg;
        }
    }
}

/*
 * The yarn code to unlock all achievements is:
 * 
 * <<unlockAchievement Act_1_Complete>>
 * <<unlockAchievement Act_2_Complete>>
 * <<unlockAchievement Act_3_Complete>>
 * <<unlockAchievement Act_4_Complete>>
 * <<unlockAchievement Astral_Secrets>>
 * <<unlockAchievement At_The_End_Of_Everything>>
 * <<unlockAchievement Bass_Ackwards>>
 * <<unlockAchievement Bea_FQ3_Recipe>>
 * <<unlockAchievement Best_Available_Friend>>
 * <<unlockAchievement Clock_Quest>>
 * <<unlockAchievement Crusty>>
 * <<unlockAchievement Demonpower>>
 * <<unlockAchievement Dusk_Stargazer>>
 * <<unlockAchievement Finish_Birdland_Quest>>
 * <<unlockAchievement Gregg_FQ3_Hit_Monster_in_Head>>
 * <<unlockAchievement Gregg_FQ3_Win_Knife_Fight>>
 * <<unlockAchievement He's_From_Somewhere>>
 * <<unlockAchievement Hold_Onto_Anything>>
 * <<unlockAchievement Horrorshow>>
 * <<unlockAchievement Jenny’s_Field>>
 * <<unlockAchievement Kill_Arm_Cockroach>>
 * <<unlockAchievement Let_It_Be_A_Legend>>
 * <<unlockAchievement Maestro>>
 * <<unlockAchievement Make_It_Last>>
 * <<unlockAchievement Miracle_Rats>>
 * <<unlockAchievement Mother_Of_Vermin>>
 * <<unlockAchievement Palecat>>
 * <<unlockAchievement Poets_of_Possum_Springs>>
 * <<unlockAchievement RABIES!>>
 * <<unlockAchievement Seriously>>
 * <<unlockAchievement Thryy_Wyrd_Tyyns>>
 * 
 * But that's no fun :P
 * 
 */
