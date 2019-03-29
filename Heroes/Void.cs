// <copyright file="PhantomAssassin.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Vaper.Heroes
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.SDK.Abilities.Items;
    using Ensage.SDK.Abilities.npc_dota_hero_faceless_void;
    using Ensage.SDK.Extensions;
    using Ensage.SDK.Helpers;
    using Ensage.SDK.Inventory.Metadata;
    using Ensage.SDK.Menu;
    using Ensage.SDK.Utils;

    using PlaySharp.Toolkit.Helper.Annotations;

    using SharpDX;

    using Vaper.OrbwalkingModes;
    using Vaper.OrbwalkingModes.Combo;
    using Vaper.OrbwalkingModes.Harras;

    using Color = System.Drawing.Color;

    [PublicAPI]
    [ExportHero(HeroId.npc_dota_hero_faceless_void)]
    public class Void : BaseHero
    {

        [ItemBinding]
        public item_mjollnir Mjollnir { get; private set; }

        [ItemBinding]
        public item_blink BlinkDagger { get; private set; }

        [ItemBinding]
        public item_manta Manta { get; private set; }

        [ItemBinding]
        public item_satanic Satanic { get; private set; }

        [ItemBinding]
        public item_black_king_bar BKB { get; private set; }

        [ItemBinding]
        public item_bloodthorn Blod { get; private set; }

        [ItemBinding]
        public faceless_void_chronosphere Chrono { get; private set; }

        [ItemBinding]
        public faceless_void_time_dilation Dilation { get; private set; }

        [ItemBinding]
        public faceless_void_time_walk TimeWalk { get; private set; }



        protected override ComboOrbwalkingMode GetComboOrbwalkingMode()
        {
            return new VoidOrbWalker(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();


            this.Chrono = this.Context.AbilityFactory.GetAbility<faceless_void_chronosphere>();
            this.Dilation = this.Context.AbilityFactory.GetAbility<faceless_void_time_dilation>();
            this.TimeWalk = this.Context.AbilityFactory.GetAbility<faceless_void_time_walk>();

        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

    }
}
