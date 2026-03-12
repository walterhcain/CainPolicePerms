using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace walterhcain.BootlegPolicePlugin
{
    public class CheckCommand : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>();
            }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }

        public string Help
        {
            get
            {
                return "This checks if the player inventory is empty.";
            }
        }

        public string Name
        {
            get
            {
                return "check";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "check" };
            }
        }

        public string Syntax
        {
            get
            {
                return "/check <player>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;

                //Checks that there is only a character name
                if (command.Length == 0)
                {
                    UnityEngine.RaycastHit hit;
                    bool found = false;
                    if (UnityEngine.Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out hit, 5, RayMasks.PLAYER_INTERACT))
                    {
                        UnturnedPlayer[] players = GetUnturnedPlayerInRadius(player.Position, BootlegPolicePlugin.Instance.Configuration.Instance.radius);
                        foreach (UnturnedPlayer ups in players)
                        {
                            if (ups.CSteamID != player.CSteamID)
                            {
                                if (ups.Position == hit.transform.position)
                                {
                                    
                                    bool isfull = false;
                                    found = true;
                                    for (byte p = 0; p < (PlayerInventory.PAGES - 1); p++)
                                    {
                                        byte itemc = player.Player.inventory.getItemCount(p);
                                        if (itemc > 0)
                                        {
                                            isfull = true;
                                            break;
                                        }
                                    }

                                    if (!isfull)
                                    {
                                        UnturnedChat.Say(caller, "They have nothing in their inventory.");
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, "They have something in their inventory.");
                                    }
                                }
                            }
                        }
                        if (!found)
                        {
                            UnturnedChat.Say(player, "No player found. Try again.");
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(player, "No player in range.");
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, "Improper Parameters", Color.red);
                }

            }
            else
            {
                Rocket.Core.Logging.Logger.Log("Only players can execute this command.");
            }
        }
        //Uses a Steam ID to check if player is online
        private bool CheckPlayer(CSteamID plr)
        {
            bool flag = false;
            foreach (SteamPlayer sp in Provider.clients)
            {
                if (sp.playerID.steamID == plr)
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static UnturnedPlayer[] GetUnturnedPlayerInRadius(UnityEngine.Vector3 center, float radius)
        {
            List<Player> playerList = new List<Player>();
            PlayerTool.getPlayersInRadius(center, radius, playerList);
            Rocket.Core.Logging.Logger.Log(playerList.Count.ToString());
            var result = new UnturnedPlayer[playerList.Count];
            for (int i = 0; i < playerList.Count; i++)
            {
                result[i] = UnturnedPlayer.FromPlayer(playerList[i]);
            }
            return result;
        }
    }
}