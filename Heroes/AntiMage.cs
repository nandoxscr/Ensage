

namespace Vaper.Heroes
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.Common.Menu;
    using Ensage.SDK.Abilities.Items;
    using Ensage.SDK.Abilities.npc_dota_hero_antimage;
    using Ensage.SDK.Extensions;
    using Ensage.SDK.Helpers;
    using Ensage.SDK.Inventory.Metadata;
    using Ensage.SDK.Menu;

    using log4net;

    using PlaySharp.Toolkit.Helper.Annotations;
    using PlaySharp.Toolkit.Logging;



    using Vaper.OrbwalkingModes.Combo;

    using Color = System.Drawing.Color;

    [PublicAPI]
    [ExportHero(HeroId.npc_dota_hero_antimage)]
    public class AntiMage : BaseHero
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [ItemBinding]
        public item_abyssal_blade AbyssalBlade { get; private set; }

        public antimage_blink BLink { get; private set; }

        [ItemBinding]

        public antimage_mana_break manabreak { get; private set; }

        public antimage_counterspell CounterSpell { get; private set; }

        [ItemBinding]
        public item_manta Manta { get; private set; }

        [ItemBinding]

        public antimage_mana_void ManaVoid { get; private set; }

        protected override ComboOrbwalkingMode GetComboOrbwalkingMode()
        {
            return new AntiMageOrbWalker(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            this.manabreak = this.Context.AbilityFactory.GetAbility<antimage_mana_break>();
            this.CounterSpell = this.Context.AbilityFactory.GetAbility<antimage_counterspell>();
            this.BLink = this.Context.AbilityFactory.GetAbility<antimage_blink>();
            this.ManaVoid = this.Context.AbilityFactory.GetAbility<antimage_mana_void>();

        }

        protected override void OnDeactivate()
        {

            base.OnDeactivate();
        }

        protected override async Task OnKillsteal(CancellationToken token)
        {
            if (Game.IsPaused || !this.Owner.IsAlive || !this.ManaVoid.CanBeCasted)
            {
                await Task.Delay(125, token);
                return;
            }

            var killstealTarget = EntityManager<Hero>.Entities.FirstOrDefault(
                x => x.IsAlive
                     && (x.Team != this.Owner.Team)
                     && !x.IsIllusion
                     && this.ManaVoid.CanHit(x)
                     && !x.IsLinkensProtected()
                     && (this.ManaVoid.GetDamage(x) > x.Health));

            if (killstealTarget != null)
            {
                if (this.ManaVoid.UseAbility(killstealTarget))
                {
                    var castDelay = this.ManaVoid.GetCastDelay(killstealTarget);
                    await this.AwaitKillstealDelay(castDelay, token);
                }
            }

            await Task.Delay(125, token);
        } 

    }
}
