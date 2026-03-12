using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;

namespace walterhcain.BootlegPolicePlugin
{
    public class BootlegPolicePlugin : RocketPlugin<BootlegPolicePluginConfiguration>
    {
        public static BootlegPolicePlugin Instance;
        private string version = "Version 1.0";
        

        protected override void Load()
        {
            Instance = this;
            U.Events.OnPlayerConnected += Police_OnPlayerConnected;
            U.Events.OnPlayerDisconnected += Police_OnPlayerDisconnect;
            DamageTool.damagePlayerRequested += CainDamage;
            BarricadeManager.onDamageBarricadeRequested += CainBarricadeDamage;
            Logger.Log("Cain's Bootleg Police Plugin has successfully loaded!", ConsoleColor.Yellow);
            Logger.Log("------------", ConsoleColor.Yellow);
            Logger.Log(version, ConsoleColor.Yellow);
            Logger.Log("------------", ConsoleColor.Yellow);
            Logger.Log("Special thanks to Valkian, Baron Von Bear, FinalChuckle and Revilia for assisting in testing", ConsoleColor.Yellow);
        
        }

        protected override void Unload()
        {
            BarricadeManager.onDamageBarricadeRequested -= CainBarricadeDamage;
            U.Events.OnPlayerConnected -= Police_OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= Police_OnPlayerDisconnect;
            DamageTool.damagePlayerRequested -= CainDamage;
            Logger.Log("Cain's Bootleg Police Plugin has successfully been unloaded!", ConsoleColor.Yellow);
        }

        private void CainBarricadeDamage(CSteamID instigatorSteamID, UnityEngine.Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if(damageOrigin == EDamageOrigin.Useable_Melee)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(instigatorSteamID);
                ItemAsset currentEquipped;

                currentEquipped = player.Player.equipment.asset;
                
                if (currentEquipped == null)
                {
                    return;
                }
                if (currentEquipped.type != EItemType.MELEE)
                {
                    return;
                }
                /*
                if (currentWeapon == null)
                {
                    return;
                }
                */
                if (Configuration.Instance.ramID.Contains(currentEquipped.id))
                {
                    shouldAllow = true;
                    Logger.Log(player.CharacterName + " successfully broke down a barricade at " + player.Position.x.ToString() + ", " + player.Position.y.ToString() + ", " + player.Position.z.ToString());
                }
            }
        }

        private void CainDamage(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            if(parameters.cause == EDeathCause.MELEE)
            {
                UnturnedPlayer killer = UnturnedPlayer.FromCSteamID(parameters.killer);
                if(killer != null)
                {
                    ItemMeleeAsset currentWeapon;
                    ItemAsset currentEquipped;

                    currentEquipped = killer.Player.equipment.asset;
                    if (currentEquipped == null)
                    {
                        return;
                    }
                    if (currentEquipped.type != EItemType.MELEE)
                    {
                        return;
                    }
                    currentWeapon = (ItemMeleeAsset)currentEquipped;
                    if (currentWeapon == null)
                    {
                        return;
                    }
                    if (Configuration.Instance.batons.Contains(currentWeapon.id))
                    {
                        parameters.player.life.breakLegs();
                        parameters.player.life.save();
                        Logger.Log(killer.CharacterName + " successfully broke " + UnturnedPlayer.FromPlayer(parameters.player).CharacterName + "'s legs");
                    }
                }
            }
        }

        private void Police_OnPlayerConnected(UnturnedPlayer player)
        {
           
            if (player.SteamGroupID == BootlegPolicePlugin.Instance.Configuration.Instance.GroupID)
            {
                bool found = false;
                foreach (RocketPermissionsGroup group in R.Permissions.GetGroups(new RocketPlayer(player.ToString()), true))
                {
                    Logger.Log("Checking Groups");
                    if (group.Id.ToLower() == "cadet")
                    {
                        if (!player.CharacterName.ToLower().Contains("cadet"))
                        {
                            UnturnedChat.Say(player, "Please add Cadet to the start of your public and private name");
                            RemoveGroup(player);
                        }
                        else
                        {
                            found = true;
                            R.Permissions.AddPlayerToGroup("cadet2", player);
                            Logger.Log(player.CharacterName + " has been given cadet perms!", ConsoleColor.Blue);
                            break;
                        }
                    }
                    else if (group.Id.ToLower() == "cop")
                    {
                        if (!player.CharacterName.ToLower().Contains("officer"))
                        {
                            UnturnedChat.Say(player, "Please add Officer to the start of your public and private name");
                            RemoveGroup(player);
                        }
                        else
                        {
                            found = true;
                            R.Permissions.AddPlayerToGroup("cop2", player);
                            Logger.Log(player.CharacterName + " has been given cop perms!", ConsoleColor.Blue);
                            break;
                        }
                    }
                    else if (group.Id.ToLower() == "deputy")
                    {
                        if (!player.CharacterName.ToLower().Contains("deputy"))
                        {
                            UnturnedChat.Say(player, "Please add Deputy to the start of your public and private name");
                            RemoveGroup(player);
                        }
                        else
                        {
                            found = true;
                            R.Permissions.AddPlayerToGroup("deputy2", player);
                            Logger.Log(player.CharacterName + " has been given deputy perms!", ConsoleColor.Blue);
                            break;
                        }
                    }
                    else if (group.Id.ToLower() == "deputychief")
                    {
                        if (!player.CharacterName.ToLower().Contains("deputy chief"))
                        {

                            UnturnedChat.Say(player, "Please add Deputy Chief to the start of your public and private name");
                            RemoveGroup(player);
                        }
                        else
                        {
                            found = true;
                            R.Permissions.AddPlayerToGroup("deputychief2", player);
                            Logger.Log(player.CharacterName + " has been given deputy chief perms!", ConsoleColor.Blue);
                            break;
                        }
                    }
                    else if (group.Id.ToLower() == "chief")
                    {
                        if (!player.CharacterName.ToLower().Contains("chief"))
                        {
                            UnturnedChat.Say(player, "Please add Chief to the start of your public and private name", UnityEngine.Color.blue);
                            RemoveGroup(player);
                        }
                        else
                        {
                            found = true;
                            R.Permissions.AddPlayerToGroup("chief2", player);
                            Logger.Log(player.CharacterName + " has been given chief perms!", ConsoleColor.Blue);
                            break;
                        }
                    }

                }
                if (!found)
                {
                    UnturnedChat.Say(player, "You have not yet had your perms added to you.");
                    RemoveGroup(player);
                }

            }
            else
            {
                RemoveGroup(player);
            }
        }

        private void Police_OnPlayerDisconnect(UnturnedPlayer player)
        {
            
            try
            {
                RemoveGroup(player);
            }
            catch
            {
                Logger.Log("Player does not have police perms");
            }
            

        }

        private void RemoveGroup(UnturnedPlayer player)
        {
            foreach (RocketPermissionsGroup group in R.Permissions.GetGroups(new RocketPlayer(player.CSteamID.ToString()), true))
            {
                if (group.Id.ToLower() == "cadet2")
                {
                    R.Permissions.RemovePlayerFromGroup("cadet2", player);
                    Logger.Log("Player has been removed from cadet perms!", ConsoleColor.Blue);
                }
                else if (group.Id.ToLower() == "cop2")
                {
                    R.Permissions.RemovePlayerFromGroup("cop2", player);
                    Logger.Log("Player has been removed from cop perms!", ConsoleColor.Blue);
                }
                else if (group.Id.ToLower() == "deputy2")
                {
                    R.Permissions.RemovePlayerFromGroup("deputy2", player);
                    Logger.Log("Player has been removed from deputy perms!", ConsoleColor.Blue);
                }
                else if (group.Id.ToLower() == "deputychief2")
                {
                    R.Permissions.RemovePlayerFromGroup("deputychief2", player);
                    Logger.Log("Player has been removed from deputychief perms!", ConsoleColor.Blue);
                }
                else if (group.Id.ToLower() == "chief2")
                {
                    R.Permissions.RemovePlayerFromGroup("chief2", player);
                    Logger.Log("Player has been removed from chief perms!", ConsoleColor.Blue);
                }
            }
        }
    }
}

