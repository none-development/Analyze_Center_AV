using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Analyze_Center_AV.GenerellSystems;
using DiscordRPC;

namespace Analyze_Center_AV.DesignFeatures
{
    internal class DiscordRPCM
    {
        public DiscordRpcClient client;
        public void MainDiscord()
        {
            client = new DiscordRpcClient("");

            client.OnReady += (sender, e) =>
            {
                GenerateData.DiscordName = e.User.Username;
                GenerateData.DiscordID = e.User.ID.ToString(); 
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            client.Initialize();
      
            client.SetPresence(new RichPresence()
            {
                Details = "Analyze Center AV",
                State = "https://as.mba",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "1zu1",
                    LargeImageText = "https://as.mba/",
                    SmallImageKey = "as",
                    SmallImageText = "Analyze Center AV"
                },
                Buttons = new Button[]
                {
                    new Button(){ Label = "Webpage", Url = "https://as.mba/" },
                    new Button(){ Label = "Discord", Url = "https://discord.gg/HYeZrKdRhm" }
                }
            });
     
        }


    }
}
