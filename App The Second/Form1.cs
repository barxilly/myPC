﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace App_The_Second
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int calculateSpecScore(string CPU, string GPU, string RAM, string RAMType, string Storage)
        {
            int Score = (int.Parse(RAM) * 300) + (getCPUScore(CPU) * 3) + (getGPUScore(GPU) * 4) + int.Parse(RAMType.Replace("DDR", ""));
            int miniScore = Score / 100;
            return miniScore;
        }
        private int getCPUScore(string CPU)
        {
            try
            {
                Regex regex = new Regex(string.Format("{0}~(.*?)\r", CPU), RegexOptions.IgnoreCase);
                string file = File.ReadAllText(@"../../CPU.list");
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
                string file = File.ReadAllText(@"../../GPU.list");
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
            if (Score > 200)
            {
                label6.Text = "Overkill";
            }
            else if (Score > 100)
            {
                label6.Text = "Very Powerful";
            }
            else if (Score > 70)
            {
                label6.Text = "Powerful";
            }
            else if (Score > 40)
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
            games.Add("Minecraft: Java Edition", new string[] { "8", "3", "2214", "343", "100" });
            games.Add("Grand Theft Auto V", new string[] { "8", "3", "4678", "3998", "150" });
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
                        string file = File.ReadAllText(@"../../CPU.list");
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
                        string GPUfile = File.ReadAllText(@"../../GPU.list");
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
            presets.Add("Asus Nitro 5 2019 8GB", new string[] { "Intel Core i5-9300H", "GeForce GTX 1650", "8", "DDR4" });
            presets.Add("Lenovo ThinkPad X270", new string[] { "Intel Core i5-7200U", "Intel HD Graphics 620", "8", "DDR4" });
            presets.Add("PlayStation 5", new string[] { "AMD Ryzen 7 3700X", "GeForce RTX 2050", "16", "DDR4" });
            // Add more presets later
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
    }
}
