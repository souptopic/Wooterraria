using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Wooting;

namespace Wooterraria
{
    public class Wooterraria : ModSystem
    {
        private bool playerHealed = false;

        public override void Load()
        {
            RGBControl.Initialize(Mod);
            if (RGBControl.IsInitialized)
            {
                Mod.Logger.Info("RGB Control Initialized.");

                // Subscribe to the ProcessExit event
                AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            }
            else
            {
                Mod.Logger.Error("RGB Control could not be initialized.");
                throw new Exception("RGB Control initialization failed. Mod will not load.");
            }
            Mod.Logger.Info("Wooterraria Loaded!");
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            RGBControl.Close();
            Mod.Logger.Info("RGB Restored.");
        }

        public override void PostUpdatePlayers()
        {
            Player player = Main.LocalPlayer;
            var healKey = WootingKey.GetKeyFromInput("QuickHeal");
            var wootingHealKey = WootingKey.ConvertToWootingKey(healKey);

            if (player.statLife < player.statLifeMax2 / 2)
            {
                if (!playerHealed)
                {
                    RGBControl.SetKey(wootingHealKey, 255, 0, 0, true);
                    playerHealed = true;
                }
            }
            else if (playerHealed)
            {
                RGBControl.ResetKey(wootingHealKey);
                playerHealed = false;
            }

            if (playerHealed && Main.keyState.IsKeyDown(healKey))
            {
                RGBControl.ResetKey(wootingHealKey);
                playerHealed = false;
            }
        }

        

        public override void Unload()
        {
            if (RGBControl.IsInitialized)
            {
                RGBControl.Close();
            }
            Mod.Logger.Info("Wooterraria Unloaded!");
        }
    }

    public class WooterrariaPlayer : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            RGBControl.ResetRGB();
        }
    }
}
