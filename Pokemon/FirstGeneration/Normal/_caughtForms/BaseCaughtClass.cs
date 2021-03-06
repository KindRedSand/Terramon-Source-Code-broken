﻿using System;
using System.Collections.Generic;
using System.IO;
using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terramon.Items.Pokeballs.Thrown;
using Terramon.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terramon.Pokemon.FirstGeneration.Normal.Charmander;
using Terramon.Pokemon.Moves;

namespace Terramon.Items.Pokeballs.Inventory
{
    public abstract class BaseCaughtClass : ModItem
    {
        /// <summary>
        /// I think this is not needed. I want  to store what mon are here
        /// we better need to store a type string <see cref="nameof(Charmander)"/>
        /// </summary>
        public int PokemonNPC;
        public string CapturedPokemon;
        public string PokemonName;
        public string SmallSpritePath;
        public int PartySlotNumber;
        public int Level = 1;
        public int Exp = 0;

        public List<BaseMove> Moves;

        public override bool CloneNewInstances => true;

        public override void SetDefaults()
        {
            item.damage = 20;

            item.width = 24;
            item.height = 24;

            item.useTime = 20;
            item.useStyle = 1;
            item.useAnimation = 20;

            item.UseSound = SoundID.Item2;
            item.accessory = false;
            item.shoot = 10;
            item.scale = 1f;

            item.noMelee = true;

            item.rare = 0;

            //I'l made a moves registry like i do it with mons after we done
            Moves = new List<BaseMove>(4);

            //Detour handle
            if (Main.netMode != NetmodeID.Server || det_CapturedPokemon == null)
                return;

            PokemonNPC = det_PokemonNPC;
            det_PokemonNPC = 0;
            PokemonName = det_PokemonName;
            det_PokemonName = null;
            SmallSpritePath = det_SmallSpritePath;
            det_SmallSpritePath = null;
            CapturedPokemon = det_CapturedPokemon;
            det_CapturedPokemon = null;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (SmallSpritePath == null )
            {
                var mon = TerramonMod.GetPokemon(CapturedPokemon);
                if (mon == null)
                    return true;
                SmallSpritePath = mon.IconName;
            }

            Texture2D pokemonTexture = ModContent.GetTexture(SmallSpritePath);
            Texture2D itemTexture = Main.itemTexture[item.type];
            spriteBatch.Draw(itemTexture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(pokemonTexture, position + itemTexture.Size() * Main.inventoryScale - new Vector2(4, 4), pokemonTexture.Frame(), drawColor, 0f, pokemonTexture.Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0);
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //if (player.HasBuff(mod.BuffType(PokemonName + "Buff")))
            //{
            //    player.ClearBuff(mod.BuffType(PokemonName + "Buff"));
            //    switch (Main.rand.Next(3))
            //    {
            //        case 0:
            //            CombatText.NewText(player.Hitbox, Color.White, PokemonName + ", switch out!\nCome back!", true, false);
            //            break;
            //        case 1:
            //            CombatText.NewText(player.Hitbox, Color.White, PokemonName + ", return!", true, false);
            //            break;
            //        default:
            //            CombatText.NewText(player.Hitbox, Color.White, "That's enough for now, " + PokemonName + "!", true, false);
            //            break;
            //    }
            //    return true;
            //}
            //else
            //    player.AddBuff(mod.BuffType(PokemonName + "Buff"), 2);
            //CombatText.NewText(player.Hitbox, Color.White, "Go! " + PokemonName + "!", true, false);
            //return true;

            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            if (!player.HasBuff(pokeBuff))
            {
                player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = PokemonName;
                modPlayer.ActivatePet(PokemonName);
                CombatText.NewText(player.Hitbox, Color.White, "Go! " + PokemonName + "!", true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
            }
            else
            {
                player.ClearBuff(pokeBuff);
                switch (Main.rand.Next(3))
                {
                    case 0:
                        CombatText.NewText(player.Hitbox, Color.White, modPlayer.ActivePetName + ", switch out!\nCome back!", true, false);
                        break;
                    case 1:
                        CombatText.NewText(player.Hitbox, Color.White, modPlayer.ActivePetName + ", return!", true, false);
                        break;
                    default:
                        CombatText.NewText(player.Hitbox, Color.White, "That's enough for now, " + modPlayer.ActivePetName + "!", true, false);
                        break;
                }
                modPlayer.ActivePetName = string.Empty;
            }



            return true;
        }

        public override bool CanRightClick()
        {
            //Just make it little clear
            return ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item.IsAir || ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item.IsAir || ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item.IsAir || ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item.IsAir;
        }

        public override void RightClick(Player player)
        {
            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item = item.Clone();
                item.TurnToAir();
            }
            else

            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item = item.Clone();
                item.TurnToAir();
            }
            else

            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item = item.Clone();
                item.TurnToAir();
            }
            else

            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item = item.Clone();
                item.TurnToAir();
            }
            else

            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item = item.Clone();
                item.TurnToAir();
            }
            else

            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item = item.Clone();
                item.TurnToAir();
            }
            else
            {
                Main.NewText("All Party Slots are full", 255, 240, 20, false);
                return;
            }
            ((TerramonMod)mod).PartySlots.UpdateUI(null);
        }

        public const string POKEBAL_PROPERTY = "PokebalType";
        public override TagCompound Save()
        {
            return new TagCompound
            {
                [nameof(PokemonNPC)] = PokemonNPC,
                [nameof(PokemonName)] = PokemonName,
                [nameof(SmallSpritePath)] = SmallSpritePath,

                [nameof(SmallSpritePath)] = SmallSpritePath, // what do i do here
                //v2
                [nameof(CapturedPokemon)] = CapturedPokemon,
                [nameof(Level)] = Level,
                [nameof(Exp)] = Exp,
                //Used to restore items in sidebarUI
                [POKEBAL_PROPERTY] = (byte)TerramonMod.PokeballFactory.GetEnum(this),
            };
        }
        public override void Load(TagCompound tag)
        {
            PokemonNPC = tag.GetInt(nameof(PokemonNPC));
            PokemonName = tag.GetString(nameof(PokemonName));
            SmallSpritePath = tag.GetString(nameof(SmallSpritePath));
            //v2
            CapturedPokemon = tag.ContainsKey(nameof(CapturedPokemon)) ? tag.GetString(nameof(CapturedPokemon)) : PokemonName;
            Level = tag.ContainsKey(nameof(Level)) ? tag.GetInt(nameof(Level)) : 1;
            Exp = tag.ContainsKey(nameof(Exp)) ? tag.GetInt(nameof(Exp)) : 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            for (int i = 0; i < tooltips.Count;)
            {
                if (tooltips[i].text.Contains("damage") || tooltips[i].text.Contains("knockback") || tooltips[i].text.Contains("critical strike") || tooltips[i].text.Contains("speed"))
                    tooltips.RemoveAt(i);
                else
                    i++;
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            //writer.Write(PokemonNPC);
            if (PokemonName != null)
            {
                writer.Write(true);
                writer.Write(PokemonName);
            }
            else
                writer.Write(false);

            if (SmallSpritePath != null)
            {
                writer.Write(true);
                writer.Write(SmallSpritePath);
            }
            else
                writer.Write(false);

            //v2 
            if (CapturedPokemon != null)
            {
                writer.Write(true);
                writer.Write(CapturedPokemon);
            }
            else
                writer.Write(false);
            writer.Write(Level);
            writer.Write(Exp);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            //PokemonNPC = reader.ReadInt32();
            if(reader.ReadBoolean())
                PokemonName = reader.ReadString();
            if(reader.ReadBoolean())
                SmallSpritePath = reader.ReadString();
            if (reader.ReadBoolean())
            {
                CapturedPokemon = reader.ReadString();
            }

            Level = reader.ReadInt32();
            Exp = reader.ReadInt32();
        }

        //TODO: Take rid with it
        #region Temp Server Detour
        [Obsolete]
        internal static void writeDetour_old(int id, string name, string icon)
        {
            det_PokemonNPC = id;
            det_PokemonName = name;
            det_SmallSpritePath = icon;
        }

        internal static void writeDetour(string type, string name, string icon, int lvl = 1)
        {
            det_CapturedPokemon = type;
            det_PokemonName = name;
            det_SmallSpritePath = icon;
            det_Lvl = lvl;
        }

        internal static string det_CapturedPokemon;
        internal static int det_PokemonNPC;
        internal static string det_PokemonName;
        internal static string det_SmallSpritePath;
        internal static int det_Lvl;

        #endregion

    }
}