﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terramon.Players;
using Terramon.UI.SidebarParty;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace Terramon
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Party UI - Dark Mode")]
        [Tooltip("Set to true for Dark Mode, or false for Light Mode.")]
        public bool PartyUITheme;

        [DefaultValue(false)]
        [Label("Party UI - Auto Mode")]
        [Tooltip("When set to true, the sidebar will\nbecome Light Mode by day and Dark Mode by\nnight automatically.")]
        public bool PartyUIAutoMode;

        [DefaultValue(false)]
        [Label("Party UI - Reverse Auto Mode")]
        [Tooltip("When set to true, the sidebar will\nbecome Dark Mode by day and Light Mode by\nnight automatically.")]
        public bool PartyUIReverseAutoMode;

        [DefaultValue(true)]
        [Label("Show Help Button")]
        [Tooltip("When set to true, the help button will appear in the bottom left corner of the screen.")]
        public bool ShowHelpButton;
        public override void OnChanged()
        {
            TerramonMod.PartyUITheme = PartyUITheme;
            TerramonMod.PartyUIAutoMode = PartyUIAutoMode;
            TerramonMod.PartyUIReverseAutoMode = PartyUIReverseAutoMode;
            TerramonMod.ShowHelpButton = ShowHelpButton;

            UISidebar uISidebar = ModContent.GetInstance<TerramonMod>().UISidebar;
            if (uISidebar != null)
            {
                if (!TerramonMod.ShowHelpButton)
                {
                    uISidebar.RemoveChild(uISidebar.choose);
                }
                else
                {
                    uISidebar.Append(uISidebar.choose);
                }
            }
        }
    }
}