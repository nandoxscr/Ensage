

namespace Vaper.Heroes
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.Common.Menu;
    using Ensage.SDK.Abilities.Items;
    using Ensage.SDK.Abilities.npc_dota_hero_pangolier;
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
    [ExportHero(HeroId.npc_dota_hero_pangolier)]
    public class Pangolier : BaseHero
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [ItemBinding]
        public item_abyssal_blade AbyssalBlade { get; private set; }

        [ItemBinding]
        public item_blink BlinkDagger { get; private set; }

        [ItemBinding]
        public item_mjollnir Mjollnir { get; private set; }

        [ItemBinding]
        public item_cyclone Euls { get; private set; }

        [ItemBinding]
        public item_diffusal_blade Diffu { get; private set; }

        [ItemBinding]
        public item_spirit_vessel spiritvessel { get; private set; }

        [ItemBinding]
        public item_crimson_guard Crimson { get; private set; }

        [ItemBinding]
        public item_pipe Pipe { get; private set; }

        [ItemBinding]
        public item_guardian_greaves GuardianGraves { get; private set; }

        [ItemBinding]
        public item_hood_of_defiance Hood { get; private set; }

        [ItemBinding]
        public pangolier_gyroshell ultStart { get; private set; }

        [ItemBinding]
        public pangolier_gyroshell_stop ultStop { get; private set; }

        [ItemBinding]
        public pangolier_shield_crash ShieldCrash { get; private set; }

        [ItemBinding]
        public pangolier_swashbuckle Swack { get; private set; }

        public MenuItem<bool> RollingMoveOnly { get; private set; }

        protected override ComboOrbwalkingMode GetComboOrbwalkingMode()
        {
            return new PangoOrbWalker(this);
        }
        protected override void OnActivate()
        {
            base.OnActivate();

            this.ultStart = this.Context.AbilityFactory.GetAbility<pangolier_gyroshell>();
            this.ultStop = this.Context.AbilityFactory.GetAbility<pangolier_gyroshell_stop>();
            this.ShieldCrash = this.Context.AbilityFactory.GetAbility<pangolier_shield_crash>();
            this.Swack = this.Context.AbilityFactory.GetAbility<pangolier_swashbuckle>();

            var factory = this.Menu.Hero.Factory;
            this.RollingMoveOnly = factory.Item("Rolling move only", true);
        }

        protected override void OnDeactivate()
        {

            base.OnDeactivate();
        }

        protected override async Task OnKillsteal(CancellationToken token)
        {
            if (Game.IsPaused || !this.Owner.IsAlive || !this.Swack.CanBeCasted || !this.ShieldCrash.CanBeCasted )
            {
                await Task.Delay(125, token);
                return;
            }

            if (this.Swack.CanBeCasted)
            {
                var killstealTarget = EntityManager<Hero>.Entities.FirstOrDefault(
                x => x.IsAlive
                     && (x.Team != this.Owner.Team)
                     && !x.IsIllusion
                     && this.Swack.CanHit(x)
                     && !x.IsLinkensProtected()
                     && (this.Swack.GetDamage(x) > x.Health));


                if (killstealTarget != null)
                {
                    if (this.Swack.UseAbility(killstealTarget))
                    {
                        var castDelay = this.Swack.GetCastDelay(killstealTarget);
                        await this.AwaitKillstealDelay(castDelay, token);
                    }
                }
            }
            await Task.Delay(125, token);
        }

    }
}