﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL.Terraria.GameContent.UI;
using Terramon.Players;
using Terraria;
using Terraria.ModLoader;

namespace Terramon
{
    public class PokemonBuff : ModBuff
    {
        public virtual string ProjectileName { get; set; }
        protected string oldName = "";

        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
            DisplayName.SetDefault($"{ProjectileName}");
            Description.SetDefault($"A {ProjectileName} is following you around!");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (string.IsNullOrEmpty(modPlayer.ActivePetName))
            {
                player.DelBuff(buffIndex);
                return;
            }

            //if (oldName != modPlayer.ActivePetName)
            //{
            ProjectileName = modPlayer.ActivePetName;
            //    oldName = ProjectileName;

            //}

            player.buffTime[buffIndex] = 40000;

            modPlayer.ActivatePet(ProjectileName);

            var petProjectileNotSpawned = !(player.ownedProjectileCounts[mod.ProjectileType(ProjectileName)] > 0);

            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                modPlayer.ActivePetId = Projectile.NewProjectile(player.position.X + player.width / 2,
                    player.position.Y + player.height / 2, 0f, 0f, mod.ProjectileType(ProjectileName), 0, 0f,
                    player.whoAmI, 0f, 0f);
            }
        }


        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = $"A {ProjectileName} is following you around!";
            rare = 0;
        }
    }
}
