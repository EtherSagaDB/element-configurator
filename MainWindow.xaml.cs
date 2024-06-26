﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.Unicode;

namespace element_configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Set an icon using code
            Uri iconUri = new Uri("pack://application:,,,/resource/icon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            InitializeComponent();
            TextBox[] content = {
                txtCommonChatColor,
                txtPartyChatColor,
                txtWhisperChatColor,
                txtAllianceChatColor,
                txtClanChatColor,
                txtWorldChatColor,
                txtSuperChatColor,
                txtBroadcastChatColor,
                txtOtherChatColor,
                txtDamageChatColor,
                txtSystemChatColor
            };
            readChatColors("./elementclient.exe", content);
        }

        public class chatColorOffsets
        {
            public int commonChatOffset = 0x0067C4BB;      // Common Chat		Default Color: ^FFFFFF 0x0067C4BC
            public int partyChatOffset = 0x0067C4CB;       // Party Chat		Default Color: ^00FF00 0x0067C4CC
            public int whisperChatOffset = 0x0067E983;     // Whisper Chat 	Default Color: ^FF7FFF 0x0067E984
            public int allianceChatOffset = 0x0067E993;    // Alliance Chat	Default Color: ^00FFFF
            public int clanChatOffset = 0x0067E943;        // Clan Chat		Default Color: ^14A9FF
            public int worldChatOffset = 0x0067E9A3;       // World Chat		Default Color: ^FFFF00
            public int superChatOffset = 0x0067E933;       // Super Chat		Default Color: ^A900C7
            public int broadcastChatOffset = 0x0067C4AB;   // Broadcast Chat	Default Color: ^FF0000 0x0067C4AC
            public int otherChatOffset = 0x0067E953;       // Other Chat		Default Color: ^B680FF
            public int damageChatOffset = 0x0067E973;      // Damage Chat		Default Color: ^FFAE00
            public int systemChatOffset = 0x0067E963;		// System Chat		Default Color: ^FF7F00
        }

        static void readChatColors(string file, TextBox[] content)
        {
            // player usable chats 
            var offsets = new chatColorOffsets();
            int[] chatOffsets = {
                offsets.commonChatOffset,
                offsets.partyChatOffset,
                offsets.whisperChatOffset,
                offsets.allianceChatOffset,
                offsets.clanChatOffset,
                offsets.worldChatOffset,
                offsets.superChatOffset,
                offsets.broadcastChatOffset,
                offsets.otherChatOffset,
                offsets.damageChatOffset,
                offsets.systemChatOffset
            };

            if (File.Exists(file))
            {
                using BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read));
                for (int i = 0; i < chatOffsets.Length; i++)
                {
                    reader.BaseStream.Seek(chatOffsets[i], SeekOrigin.Begin);
                    byte[] data = reader.ReadBytes(14);
                    string color = Encoding.BigEndianUnicode.GetString(data);
                    Trace.WriteLine(color);
                    content[i].Text = color;
                }
            } 
            else
            {
                return;
            }
        }

        static void createBackup(string file)
        {
            DateTime dateTime = DateTime.Now;
            Directory.CreateDirectory("./configurator");

            foreach (var fi in new DirectoryInfo("./configurator").GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(5))
                fi.Delete();

            File.Copy(file, $"./configurator/{file}-{dateTime.ToString("yyyyMMdd-HHmm")}.bak");
        }

        static void SetColor(string file, int offset, string color)
        {
            using BinaryWriter writer = new BinaryWriter(File.Open(file, FileMode.Open, FileAccess.ReadWrite));
            writer.Seek(offset, SeekOrigin.Begin); //move your cursor to the position
            writer.Write(Encoding.BigEndianUnicode.GetBytes(color)); //write new color
            Trace.WriteLine("Writing: " + file + " with " + color + " at " + offset);
        }

        private static void SetDefaultChatColors()
        {
            // player usable chats 
            var offsets = new chatColorOffsets();
            int[] chatOffsets = {
                offsets.commonChatOffset,
                offsets.partyChatOffset,
                offsets.whisperChatOffset,
                offsets.allianceChatOffset,
                offsets.clanChatOffset,
                offsets.worldChatOffset,
                offsets.superChatOffset,
                offsets.broadcastChatOffset,
                offsets.otherChatOffset,
                offsets.damageChatOffset,
                offsets.systemChatOffset
            };
            string[] chatColors = { "^FFFFFF", "^00FF00", "^FF7FFF", "^00FFFF", "^14A9FF", "^FFFF00", "^A900C7", "^FF0000", "^B680FF", "^FFAE00", "^FF7F00"};

            createBackup("./elementclient.exe");

            for (int i = 0; i < chatOffsets.Length; i++)
            {
                SetColor("./elementclient.exe", chatOffsets[i], chatColors[i]);
            }
        }

        private void BtnSetDefaultColors_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultChatColors();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string[] content = { 
                txtCommonChatColor.Text, 
                txtPartyChatColor.Text, 
                txtWhisperChatColor.Text,
                txtAllianceChatColor.Text,
                txtClanChatColor.Text,
                txtWorldChatColor.Text,
                txtSuperChatColor.Text,
                txtBroadcastChatColor.Text,
                txtOtherChatColor.Text,
                txtDamageChatColor.Text,
                txtSystemChatColor.Text
            };

            // player usable chats 
            var offsets = new chatColorOffsets();
            int[] chatOffsets = {
                offsets.commonChatOffset,
                offsets.partyChatOffset,
                offsets.whisperChatOffset,
                offsets.allianceChatOffset,
                offsets.clanChatOffset,
                offsets.worldChatOffset,
                offsets.superChatOffset,
                offsets.broadcastChatOffset,
                offsets.otherChatOffset,
                offsets.damageChatOffset,
                offsets.systemChatOffset
            };

            createBackup("./elementclient.exe");
            for (int i = 0; i < chatOffsets.Length; i++)
            {
                SetColor("./elementclient.exe", chatOffsets[i], content[i]);
            }
        }
    }
}
