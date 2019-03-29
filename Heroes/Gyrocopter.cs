namespace Vaper.Heroes
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.SDK.Abilities.Items;
    using Ensage.SDK.Abilities.npc_dota_hero_gyrocopter;
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
    [ExportHero(HeroId.npc_dota_hero_gyrocopter)]
    public class Gyrocopter : BaseHero
    {
        protected override ComboOrbwalkingMode GetComboOrbwalkingMode()
        {
            return new GyrocopterOrbWalker(this);
        }

    }
}
