using ServerEngine.Characters;
using ServerEngine.Commands.AccountCommands;
using ServerEngine.Commands.AnonymousCommands;
using ServerEngine.Commands.GameCommands;
using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands
{
    public static class CommandRunner
    {
        private static List<IAnonymousCommand> BuildInAnonymousCommands { get; set; }
        private static List<IAccountCommand> BuildInAccountCommands { get; set; }
        private static List<IGameCommand> BuiltInGameCommands { get; set; }

        static CommandRunner()
        {
            BuildInAnonymousCommands = new List<IAnonymousCommand>()
            {
                new CreateNewAccountCommand(),
                new LoginCommand()
            };

            BuildInAccountCommands = new List<IAccountCommand>()
            {
                new AutoLoadBestGameStateCommand(),
                new CreateNewGameStateCommand(),
                new LoadGameStateCommand(),
                new CreateNewPlayerCommand(),
                new UsePlayerCommand(),
                new SaveGameStateCommand(),
                //new UnloadGameStateCommand()
            };

            BuiltInGameCommands = new List<IGameCommand>()
            {
                new LookCommand(),
                new MoveCommand(),
                new InventoryCommand(),
                new GrabCommand(),
                new DropCommand(),
                new AttackCommand(),
                new StatsCommand(),
                new InteractCommand(),
                //new ShopCommand(), // TODO: this command needs to be redesigned into smaller commands like entershop, listshopitem, buyshopitem, etc
                new ChangePlayerNameCommand(),
            };
        }

        internal static bool TryRunCommandFromClient(string word, List<string> extraWords, AttachedClient executingClient)
        {
            // Built in anonymous commands
            var anonCommandToRun = BuildInAnonymousCommands
                .Where(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault(); // No permission check on anon commands
            if (anonCommandToRun != null)
            {
                anonCommandToRun.Execute(extraWords, executingClient);
                return true;
            }

            // Anything further requires an account attached to the client
            if (executingClient?.AttachedAccount == null)
            {
                return false;
            }

            // Try to run an account level command
            if (TryRunCommandFromAccount(word, extraWords, executingClient.AttachedAccount))
            {
                return true;
            }

            // Anything further requires a player logged into the game
            var playerCharacter = AttachedClients.GetClientFocusedCharacter(executingClient.TrackingId);
            if (playerCharacter == null)
            {
                return false;
            }

            if (!playerCharacter.CanTakeTurn())
            {
                var secondsLeft = playerCharacter.GetSecondsTillNextTurn();
                playerCharacter.SendDescriptiveTextDtoMessage($"You cannot take your next turn yet. ({secondsLeft} seconds left).");
                return true;
            }
            else
            {
                bool commandResult = TryRunCommandFromCharacter(word, extraWords, playerCharacter, executingClient.AttachedAccount.GetPermissions().Result);
                playerCharacter.TurnComplete();
                return commandResult;
            }
        }

        internal static bool TryRunCommandFromAccount(string word, List<string> extraWords, IAccountGrain executingAccount)
        {
            // Built in account commands
            var accountCommandToRun = BuildInAccountCommands
                .FirstOrDefault(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));

            // If the command was not found
            if (accountCommandToRun == null)
            {
                return false;
            }

            // If the command requires a certain permission to run
            if (accountCommandToRun.PermissionNeeded != null)
            {
                var hasPermission = executingAccount.HasPermission(accountCommandToRun.PermissionNeeded).Result;
                if (!hasPermission)
                {
                    return false;
                }
            }

            // All checks passed, run the command
            accountCommandToRun.Execute(extraWords, executingAccount);
            return true;
        }

        public static bool TryRunCommandFromCharacter(string word, List<string> extraWords, Character executingCharacter)
        {
            // Try to get an associated account if we can, but if we can't find an account (such as for NPCs) 
            // then continue with default permissions
            var permissionsToUse = new List<string>();
            var client = AttachedClients.GetCharacterFocusedClient(executingCharacter.TrackingId);
            if (client?.AttachedAccount != null)
            {
                permissionsToUse = client.AttachedAccount.GetPermissions().Result;
            }
            return TryRunCommandFromCharacter(word, extraWords, executingCharacter, permissionsToUse);
        }

        public static bool TryRunCommandFromCharacter(string word, List<string> extraWords, Character executingCharacter, List<string> accountPermissions)
        {
            if (accountPermissions == null)
            {
                return false;
            }

            // Built in game commands
            var gameCommandToRun = BuiltInGameCommands
                .Where(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault(c => c.PermissionNeeded == null || accountPermissions.Contains(c.PermissionNeeded, StringComparer.OrdinalIgnoreCase));
            if (gameCommandToRun != null)
            {
                gameCommandToRun.Execute(extraWords, executingCharacter);
                return true;
            }

            // Extensible game commands
            gameCommandToRun = GameState.CurrentGameState.GetGameCommand(word, accountPermissions);
            if (gameCommandToRun != null)
            {
                gameCommandToRun.Execute(extraWords, executingCharacter);
                return true;
            }

            return false;
        }
    }
}
