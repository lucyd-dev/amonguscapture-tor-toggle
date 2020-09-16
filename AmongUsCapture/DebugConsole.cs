﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AmongUsCapture
{
    class DebugConsole
    {
        public void Run()
        {
            AllocConsole();
            GameMemReader.getInstance().GameStateChanged += GameStateChangedHandler;
            GameMemReader.getInstance().PlayerChanged += PlayerChangedHandler;

            while(true)
            {
                string command = Console.ReadLine();
                Console.WriteLine($"Received command: {command}");
                if (command == "list")
                {
                    Console.WriteLine("Player List:");
                    foreach (KeyValuePair<string, PlayerInfo> kvp in GameMemReader.getInstance().oldPlayerInfos) // thread safety ??
                    {
                        string playerName = kvp.Key;
                        PlayerInfo pi = kvp.Value;
                        Console.WriteLine(kvp.Key);
                    }
                }
            }
        }

        private void GameStateChangedHandler(object sender, GameStateChangedEventArgs e)
        {
            Console.WriteLine("State changed to " + e.NewState);
        }

        private void PlayerChangedHandler(object sender, PlayerChangedEventArgs e)
        {
            if (e.Name.Length != 0) // for some reason, sometimes there's events with players without a name
            {
                Console.WriteLine(e.Name + ": " + e.Action);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
