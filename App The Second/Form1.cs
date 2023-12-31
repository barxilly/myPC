﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace App_The_Second
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadPresets();
            version();
            IList<string> presetIlist = presetPresets.Items.Cast<string>().ToList();
            prePresets = new ReadOnlyCollection<string>(presetIlist).ToArray();

        }


        private void loadPresets()
        {
            // Check if presets.dll exists
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myPC/presets.dll")))
            {
                // Get info from presets.dll
                string[] lines = File.ReadAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
, "myPC/presets.dll"));
                // Presets are in format: NAME~CPU~GPU~RAM~RAMTYPE
                foreach (string line in lines)
                {
                    string[] preset = line.Split('~');
                    presetPresets.Items.Add(preset[0]);
                    int indextoadd = importedPresets.Length;
                    Array.Resize(ref importedPresets, importedPresets.Length + 1);
                    importedPresets[indextoadd] = preset;
                    anyImportedPresets = true;
                }
            }
        }

        private void version()
        {
            label9.Text = "v1.1.0";
        }
        private void enterer1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void enterer2(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                gsGO.PerformClick();
            }
        }
        private int calculateSpecScore(string CPU, string GPU, string RAM, string RAMType, string Storage)
        {
            int Score = (int)(((int.Parse(RAM) * 0.2) + (int.Parse(RAMType.Replace("DDR", "")) * 0.1) + (int.Parse(Storage) * 0.2) + (getCPUScore(CPU) * 0.3) + (getGPUScore(GPU) * 0.2))*10);
            int miniScore = Score / 100;
            return miniScore;
        }
        private int getCPUScore(string CPU)
        {
            try
            {
                Regex regex = new Regex(string.Format("{0}~(.*?)\r", CPU), RegexOptions.IgnoreCase);
                string file = cpuList;
                MatchCollection matches = regex.Matches(file);
                Match match = matches[0];
                GroupCollection group = match.Groups;
                return int.Parse(group[0].Value.Split('~')[1].Replace(",", ""));
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("CPU doesn't exist. Value set to 0.");
                return 0;
            }
        }

        private int getGPUScore(string GPU)
        {
            try
            {
                Regex regex = new Regex(string.Format("{0}~(.*?)\n", GPU), RegexOptions.IgnoreCase);
                string file = gpuList;
                MatchCollection matches = regex.Matches(file);
                Match match = matches[0];
                GroupCollection group = match.Groups;
                return int.Parse(group[0].Value.Split('~')[1].Replace(",", ""));
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("GPU doesn't exist. Value set to 0.");
                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Make Progress Bar Visible
            progressBar1.Visible = true;

            // RAM
            int RAM = int.Parse(textBox1.Text);
            int RAMType = int.Parse(snRAMType.Text.Last().ToString());

            // CPU
            string CPU = textBox2.Text;
            // Check if string is a number
            int m;
            bool isCPUNumeric = int.TryParse(CPU, out m);
            int CPUscore;
            if (isCPUNumeric)
            {
                int v = int.Parse(CPU);
                CPUscore = v;
            }
            else
            {
                CPUscore = getCPUScore(CPU);
            }   

            // GPU
            string GPU = textBox4.Text;
            // Check if string is a number
            int n;
            bool isNumeric = int.TryParse(GPU, out n);
            int GPUscore;
            if (isNumeric)
            {
                int v = int.Parse(GPU);
                GPUscore = v;
            }
            else
            {
                GPUscore = getGPUScore(GPU);
            }

            // Storage
            int GB = int.Parse(textBox3.Text);

            // Work out and display score
            int Score = calculateSpecScore(CPU, GPU, RAM.ToString(), RAMType.ToString(), GB.ToString());
            label5.Text = "Score: " + Score.ToString();

            // Set progress bar to 50%
            progressBar1.Value = 50;

            // Give a general "speed"
            if (Score > 1000)
            {
                label6.Text = "Overkill";
            }
            else if (Score > 300)
            {
                label6.Text = "Very Powerful";
            }
            else if (Score > 150)
            {
                label6.Text = "Powerful";
            }
            else if (Score > 90)
            {
                label6.Text = "Good";
            }
            else
            {
                label6.Text = "Slow";
            }

            // Hide progress bar and show labels
            progressBar1.Value = 100;
            label5.Visible = true;
            label6.Visible = true;
            progressBar1.Visible = false;
        }

        private Dictionary<string, string[]> getGames()
        {
            Dictionary<string, string[]> games = new Dictionary<string, string[]>();
            games.Add("Cyberpunk 2077", new string[] { "12", "4", "7254", "10072", "70" });
            games.Add("Grand Theft Auto V", new string[] { "8", "3", "4678", "3998", "150" });
            games.Add("Minecraft: Java Edition", new string[] { "8", "3", "2214", "343", "100" });
            games.Add("Minecraft: Bedrock Edition", new string[] { "4", "2", "987", "49", "1" });
            /// Add more games later
            return games;
        }

        private void gsGO_Click(object sender, EventArgs e)
        {
            try
            {
                string RAM = gsRAM.Text;
                string RAMType = gsRAMType.Text;

                // CPU
                string CPU = gsCPU.Text;
                // Check if string is a number
                int m;
                bool isCPUNumeric = int.TryParse(CPU, out m);
                if (isCPUNumeric)
                {
                }
                else
                {
                    try
                    {
                        Regex list = new Regex(string.Format("{0}~(.*?)\r", CPU), RegexOptions.IgnoreCase);
                        string file = cpuList;
                        MatchCollection listmatch = list.Matches(file);
                        Match match = listmatch[0];
                        GroupCollection group = match.Groups;
                        CPU = group[0].Value.Split('~')[1].Replace(",", "");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("CPU doesn't exist");
                    }   

                }

                // GPU
                string GPU = gsGPU.Text;
                // Check if string is a number
                int n;
                bool isNumeric = int.TryParse(GPU, out n);
                int GPUscore;
                if (isNumeric)
                {

                }
                else
                {
                    try
                    {
                        Regex GPUlist = new Regex(string.Format("{0}~(.*?)\n", GPU), RegexOptions.IgnoreCase);
                        string GPUfile = gpuList;
                        MatchCollection GPUlistmatch = GPUlist.Matches(GPUfile);
                        Match GPUmatch = GPUlistmatch[0];
                        GroupCollection GPUgroup = GPUmatch.Groups;
                        GPU = GPUgroup[0].Value.Split('~')[1].Replace(",", "");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("GPU doesn't exist");
                    }
                }

                string Storage = gsGB.Text;

                Dictionary<string, string[]> gamesRec = getGames();
                /// Add more games later
                foreach (KeyValuePair<string, string[]> game in gamesRec)
                {
                    string gameName = game.Key;
                    int gameRAM = int.Parse(game.Value[0]);
                    int gameRAMType = int.Parse(game.Value[1].Replace("DDR", ""));
                    int gameCPU = int.Parse(game.Value[2]);
                    int gameGPU = int.Parse(game.Value[3]);
                    int gameStorage = int.Parse(game.Value[4]);
                    int gameCompatibilityScore; // >100 is great, 100 is good, 70-100 is okay, 40-70 is playable, 10-40 is VERY slow, <10 is just unplayable
                    int gameRAMScore = 0;
                    int gameCPUScore = 0;
                    int gameGPUScore = 0;
                    if (gameRAM <= int.Parse(RAM))
                    {
                        gameRAMScore = (int.Parse(RAM) / gameRAM) * 100;
                    }
                    else
                    {
                        gameRAMScore = (int.Parse(RAM) / gameRAM) * 100;
                    }
                    if (gameCPU <= int.Parse(CPU))
                    {
                        gameCPUScore = (int.Parse(CPU) / gameCPU) * 100;
                    }
                    else
                    {
                        gameCPUScore = (int.Parse(CPU) / gameCPU) * 100;
                    }
                    if (gameGPU <= int.Parse(GPU))
                    {
                        gameGPUScore = (int.Parse(GPU) / gameGPU) * 100;
                    }
                    else
                    {
                        gameGPUScore = (int.Parse(GPU) / gameGPU) * 100;
                    }
                    gameCompatibilityScore = ((gameRAMScore * 2) + gameCPUScore + (gameGPUScore * 3)) / 6;
                    gsText.Text += gameName + " - " + gameCompatibilityScore + "%\r\n";
                }
                gsPanel.Visible = false;
                gsReset.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
            }
        }

        private void gsReset_Click(object sender, EventArgs e)
        {
            gsPanel.Visible = true;
            gsReset.Visible = false;
            gsText.Text = "";
            gsRAM.Text = "";
            gsRAMType.Text = "";
            gsCPU.Text = "";
            gsGPU.Text = "";
            gsGB.Text = "";
        }

        private string[] getPresetInfo(string presetname)
        {
            Dictionary<string, string[]> presets = new Dictionary<string, string[]>();
            // Presets in order: CPU, GPU, RAM, RAM Type
            presets.Add("PlayStation 5", new string[] { "AMD Ryzen 7 3700X", "GeForce RTX 2050", "16", "DDR4" });
            presets.Add("Apple Mac Pro 2023", new string[] { "Apple M2 Ultra 24 Core", "GeForce RTX 4080", "64", "DDR5" });
            if (anyImportedPresets)
            {
                foreach (string[] preset in importedPresets)
                {
                    string presetName = preset[0];
                    string[] presetValues = new string[] { preset[1], preset[2], preset[3], preset[4] };
                    presets.Add(presetName, presetValues);
                }
            }
            foreach (KeyValuePair<string, string[]> preset in presets)
            {
                string presetName = preset.Key;
                if (presetname == presetName)
                {
                    string[] presetValues = preset.Value;
                    return presetValues;
                }
            }
            return null;
        }

        private void presetPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            string chosenPreset = presetPresets.Text;
            string[] strings = getPresetInfo(chosenPreset);
            string CPU = strings[0];
            string GPU = strings[1];
            string RAM = strings[2];
            string RAMType = strings[3];
            string score = calculateSpecScore(CPU, GPU, RAM, RAMType, "1000").ToString();
            presetCPUlabel.Text = "CPU: " + CPU + " (Score: " + getCPUScore(CPU) + ")";
            presetGPUlabel.Text = "GPU: " + GPU + " (Score: " + getGPUScore(GPU) + ")";
            presetRAMlabel.Text = "RAM: " + RAM + "GB " + RAMType;
            presetScorelabel.Text = "Score: " + score;
            presetPanel.Visible = true;
            presetShowGamingInfo_CheckedChanged(sender, e);
        }

        private void presetShowGamingInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (presetShowGamingInfo.Checked)
            {
                presetGamingText.Text = "";
                presetGamingText.Visible = true;
                Dictionary<string, string[]> games = getGames();
                string[] presetData = getPresetInfo(presetPresets.Text);
                int presetCPU = getCPUScore(presetData[0]);
                int presetGPU = getGPUScore(presetData[1]);
                int presetRAM = int.Parse(presetData[2]);
                int presetRAMType = int.Parse(presetData[3].Replace("DDR", ""));
                foreach (KeyValuePair<string, string[]> game in games)
                {
                    string gameName = game.Key;
                    int gameRAM = int.Parse(game.Value[0]);
                    int gameRAMType = int.Parse(game.Value[1].Replace("DDR", ""));
                    int gameCPU = int.Parse(game.Value[2]);
                    int gameGPU = int.Parse(game.Value[3]);
                    int gameStorage = int.Parse(game.Value[4]);
                    int gameCompatibilityScore; // >100 is great, 100 is good, 70-100 is okay, 40-70 is playable, 10-40 is VERY slow, <10 is just unplayable
                    int gameRAMScore = 0;
                    int gameCPUScore = 0;
                    int gameGPUScore = 0;
                    if (gameRAM <= presetRAM)
                    {
                        gameRAMScore = (presetRAM / gameRAM) * 100;
                    }
                    else
                    {
                        gameRAMScore = (presetRAM / gameRAM) * 100;
                    }
                    if (gameCPU <= presetCPU)
                    {
                        gameCPUScore = (presetCPU / gameCPU) * 100;
                    }
                    else
                    {
                        gameCPUScore = (presetCPU / gameCPU) * 100;
                    }
                    if (gameGPU <= presetGPU)
                    {
                        gameGPUScore = (presetGPU / gameGPU) * 100;
                    }
                    else
                    {
                        gameGPUScore = (presetGPU / gameGPU) * 100;
                    }
                    gameCompatibilityScore = ((gameRAMScore * 2) + gameCPUScore + (gameGPUScore * 3)) / 6;
                    presetGamingText.Text += gameName + " - " + gameCompatibilityScore + "%\r\n";
                }
            }
            else
            {
                presetGamingText.Visible = false;
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            // Let user select a file with .pcspec extension
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PC Spec File|*.pcspec";
            openFileDialog1.Title = "Select a PC Spec File";
            openFileDialog1.ShowDialog();

            // If user selects a file
            async void importFile()
            {
                // Get data from file and save to presets.dll
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                foreach (string line in lines)
                {
                    string presetString = line + "\r\n";
                    try
                    {
                        File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    , "myPC/presets.dll"), presetString);
                    } catch (DirectoryNotFoundException)
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myPC"));
                        File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myPC/presets.dll"), presetString);
                    }

                    int indextoadd = importedPresets.Length;
                    Array.Resize(ref importedPresets, importedPresets.Length + 1);
                    importedPresets[indextoadd] = line.Split('~');

                    presetPresets.Items.Add(line.Split('~')[0]);

                    anyImportedPresets = true;
                }
            }

            if (openFileDialog1.FileName != "")
            {
                importFile();
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            // Give the PC a name
            string PCName = Interaction.InputBox("What would you like to name your PC?", "Name your PC", "My PC", 0, 0);
            // Export current specs to a .pcspec file in the format: CPU~GPU~RAM~RAMType
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PC Spec File|*.pcspec";
            saveFileDialog1.Title = "Save your PC Spec File";

            string RAM = textBox1.Text;
            string RAMType = snRAMType.Text;
            string CPU = textBox2.Text;
            string GPU = textBox4.Text;

            string line = PCName + "~" + CPU + "~" + GPU + "~" + RAM + "~" + RAMType;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, line);
            }
        }

        private void resetPresets_Click(object sender, EventArgs e)
        {
            // Delete presets.dll and close the program
            try
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
  , "myPC/presets.dll"));
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myPC"));
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myPC/presets.dll"));
            }
            presetPresets.Items.Clear();
            foreach (Object item in prePresets)
            {
                presetPresets.Items.Add(item);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Compare_Click(object sender, EventArgs e)
        {
            compareName.Text = "";
            compareCPU.Text = "";
            compareGPU.Text = "";
            compareRAM.Text = "";
            Object[] items = presetPresets.Items.Cast<Object>().ToArray();
            // Sort items by CPU score
            Array.Sort(items, delegate (Object item1, Object item2)
            {
                string[] info1 = getPresetInfo(item1.ToString());
                string[] info2 = getPresetInfo(item2.ToString());
                int score1 = getCPUScore(info1[0]);
                int score2 = getCPUScore(info2[0]);
                return score2.CompareTo(score1);
            });
            foreach (Object item in items)
            {
                string[] info = getPresetInfo(item.ToString());
                // "Preset Name [first 15 chars, if smaller than 15 chars, add spaces to make it 15 chars] - CPU [first 5 chars, if smaller than 5 chars, add spaces to make it 5 chars] - GPU [first 5 chars, if smaller than 5 chars, add spaces to make it 5 chars] - RAM [first 2 chars, if smaller than 2 chars, add spaces to make it 2 chars]GB RAMType [first 4 chars, if smaller than 4 chars, add spaces to make it 4 chars]"
                string presetName = item.ToString();
                string presetCPU = getCPUScore(info[0]).ToString();
                string presetGPU = getGPUScore(info[1]).ToString();
                string presetRAM = info[2];
                string presetRAMType = info[3];
                if (presetName.Length < 19)
                {
                    int spacesToAdd = 19 - presetName.Length;
                    for (int i = 0; i < spacesToAdd; i++)
                    {
                        presetName += " ";
                    }
                }
                else if (presetName.Length > 19)
                {
                    presetName = presetName.Substring(0, 19);
                }
                if (presetCPU.Length < 5)
                {
                    int spacesToAdd = 5 - presetCPU.Length;
                    for (int i = 0; i < spacesToAdd; i++)
                    {
                        presetCPU += " ";
                    }
                }
                else if (presetCPU.Length > 5)
                {
                    presetCPU = presetCPU.Substring(0, 5);
                }
                if (presetGPU.Length < 5)
                {
                    int spacesToAdd = 5 - presetGPU.Length;
                    for (int i = 0; i < spacesToAdd; i++)
                    {
                        presetGPU += " ";
                    }
                }
                else if (presetGPU.Length > 5)
                {
                    presetGPU = presetGPU.Substring(0, 5);
                }
                if (presetRAM.Length < 2)
                {
                    int zerosToAdd = 2 - presetRAM.Length;
                    // Add zeros to the start of the string
                    for (int i = 0; i < zerosToAdd; i++)
                    {
                        presetRAM = "0" + presetRAM;
                    }
                }
                else if (presetRAM.Length > 2)
                {
                    presetRAM = presetRAM.Substring(0, 2);
                }
                if (presetRAMType.Length < 4)
                {
                    int spacesToAdd = 4 - presetRAMType.Length;
                    for (int i = 0; i < spacesToAdd; i++)
                    {
                        presetRAMType += " ";
                    }
                }
                else if (presetRAMType.Length > 4)
                {
                    presetRAMType = presetRAMType.Substring(0, 4);
                }
                compareName.Text += presetName + "\r\n";
                compareCPU.Text += presetCPU + "\r\n";
                compareGPU.Text += presetGPU + "\r\n";
                compareRAM.Text += presetRAM + "GB " + presetRAMType + "\r\n";
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
