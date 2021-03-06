using System;
using Terramon.Players;
using Terraria;

namespace Terramon.Pokemon.FirstGeneration.Normal.Ivysaur
{
    public class Ivysaur : ParentPokemon
    {
        public override int EvolveCost => 16;

        public override Type EvolveTo => typeof(Venusaur.Venusaur);

        public override void SetDefaults()
        {
            base.SetDefaults();

            projectile.width = 36;
            projectile.height = 28;
			projectile.scale = 1.2f;
            // drawOriginOffsetY = -1;
        }
    }
}