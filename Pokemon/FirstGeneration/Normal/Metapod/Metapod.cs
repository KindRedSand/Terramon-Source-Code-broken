using System;
using Terramon.Players;
using Terraria;

namespace Terramon.Pokemon.FirstGeneration.Normal.Metapod
{
    public class Metapod : ParentPokemon
    {
        public override int EvolveCost => 20;

        public override Type EvolveTo => typeof(Butterfree.Butterfree);

        public override void SetDefaults()
        {
            base.SetDefaults();

            projectile.width = 32;
            projectile.height = 32;
            drawOriginOffsetY = -14;
        }
    }
}