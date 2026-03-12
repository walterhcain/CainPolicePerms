using Rocket.API;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace walterhcain.BootlegPolicePlugin
{
    public class BootlegPolicePluginConfiguration : IRocketPluginConfiguration
    {

        public CSteamID GroupID;
        public float radius;
        public List<ushort> batons;
        public List<ushort> ramID;
        public void LoadDefaults()
        {
            
            GroupID = (CSteamID)103582791455441288;
            radius = 5;
            ramID = new List<ushort>()
			{
				49050
			};
            batons = new List<ushort>()
            {
                105,
                1023
            };
        }

    
    }
}

