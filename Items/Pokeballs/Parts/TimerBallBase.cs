using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Terramon.Items.Pokeballs.Parts
{
    public class TimerBallBase : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Timer Ball Base");
            Tooltip.SetDefault("A unique, iron base."
                + "\nCombine it with a button and a cap to create a Timer Ball.");
        }
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 999;
            item.value = 8000;
            item.rare = 0;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddIngredient(mod.ItemType("RedApricorn"));
            recipe.AddIngredient(mod.ItemType("WhiteApricorn"));
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(255, 163, 71);
                }
            }
        }
    }
}
