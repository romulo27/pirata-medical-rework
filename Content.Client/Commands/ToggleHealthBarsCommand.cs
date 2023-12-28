using Content.Shared.Overlays;
using Robust.Client.Player;
using Robust.Shared.Console;
using System.Linq;

namespace Content.Client.Commands;

public sealed class ToggleHealthBarsCommand : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;

    public string Command => "togglehealthbars";
    public string Description => "Toggles a health bar above mobs.";
    public string Help => $"Usage: {Command} [<DamageContainerId>]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var player = _playerManager.LocalPlayer;
        if (player == null)
        {
            shell.WriteLine("You aren't a player.");
            return;
        }

        var playerEntity = player?.ControlledEntity;
        if (playerEntity == null)
        {
            shell.WriteLine("You do not have an attached entity.");
            return;
        }

        if (!_entityManager.HasComponent<ShowHealthBarsComponent>(playerEntity))
        {
            using var showHealthBarsComponent = _entityManager.AddComponentUninitialized<ShowHealthBarsComponent>(playerEntity.Value);
            showHealthBarsComponent.Comp.DamageContainers = args.ToList();

            shell.WriteLine($"Enabled health overlay for DamageContainers: {string.Join(", ", args)}.");
            return;
        }
        else
        {
            _entityManager.RemoveComponentDeferred<ShowHealthBarsComponent>(playerEntity.Value);
            shell.WriteLine("Disabled health overlay.");
        }

        return;
    }
}
