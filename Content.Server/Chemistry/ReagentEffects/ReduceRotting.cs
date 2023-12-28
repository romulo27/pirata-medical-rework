using Content.Shared.Chemistry.Reagent;
using Content.Server.Medical;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Content.Shared.Atmos.Miasma;
using Content.Server.Atmos.Miasma;

namespace Content.Server.Chemistry.ReagentEffects
{
    /// <summary>
    /// Reduces the rotting accumulator on the patient, making them revivable.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class ReduceRotting : ReagentEffect
    {
        [DataField("rottingAmount")]
        public double RottingAmount = 10;

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-reduce-rotting",
                ("chance", Probability),
                ("time", RottingAmount));
        public override void Effect(ReagentEffectArgs args)
        {
            if (args.Scale != 1f)
                return;

            var rottingSys = args.EntityManager.EntitySysManager.GetEntitySystem<RottingSystem>();

            rottingSys.ReduceAccumulator(args.SolutionEntity, TimeSpan.FromSeconds(RottingAmount));
        }
    }
}
