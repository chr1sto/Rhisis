using NLog;
using Rhisis.Core.Common;
using Rhisis.Core.Reflection;
using Rhisis.World.Game.Chat;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rhisis.World.Systems.Chat
{
    [System]
    public class ChatSystem : NotifiableSystemBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly IDictionary<string, Tuple<AuthorityType, Action<IPlayerEntity, string[]>>> ChatCommands = new Dictionary<string, Tuple<AuthorityType, Action<IPlayerEntity, string[]>>>();

        /// <inheritdoc />
        protected override WorldEntityType Type => WorldEntityType.Player;

        /// <summary>
        /// Creates a new <see cref="ChatSystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public ChatSystem(IContext context)
            : base(context)
        {
        }
        
        /// <inheritdoc />
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(e is ChatEventArgs chatEvent) || !(entity is IPlayerEntity player))
                return;
            
            if (!chatEvent.CheckArguments())
                return;

            if (chatEvent.Message.StartsWith("/"))
            {
                string commandName = chatEvent.Message.Split(' ').FirstOrDefault();
                string[] commandParameters = GetCommandParameters(chatEvent.Message, commandName);

                if (ChatCommands.TryGetValue(commandName, out var tuple))
                {
                    if (player.PlayerData.Authority >= tuple.Item1)
                        tuple.Item2.Invoke(player, commandParameters);
                    else
                        Logger.Warn($"{player.Object.Name} tried to use `{commandName}` command without privileges.");
                }
                else
                    Logger.Warn("Unknow chat command '{0}'", commandName);
            }
            else
            {
                WorldPacketFactory.SendChat(player, chatEvent.Message);
            }
        }

        /// <summary>
        /// Gets the command parameters.
        /// </summary>
        /// <param name="command">Command line</param>
        /// <param name="commandName">Command name</param>
        /// <returns></returns>
        private static string[] GetCommandParameters(string command, string commandName)
        {
            string commandParameters = command.Remove(0, commandName.Length);

            return Regex.Matches(commandParameters, @"[\""].+?[\""]|[^ ]+").Select(m => m.Value.Trim('"')).ToArray();
        }

        /// <summary>
        /// Initialize chat commands.
        /// </summary>
        public static void InitializeCommands()
        {
            IEnumerable<MethodInfo> chatCommandsMethods = ReflectionHelper.GetMethodsWithAttributes<ChatCommandAttribute>();

            foreach (MethodInfo chatMethod in chatCommandsMethods)
            {
                var action = chatMethod.CreateDelegate(typeof(Action<IPlayerEntity, string[]>)) as Action<IPlayerEntity, string[]>;
                IEnumerable<ChatCommandAttribute> chatComandAttributes = chatMethod.GetCustomAttributes<ChatCommandAttribute>();

                foreach (ChatCommandAttribute attribute in chatComandAttributes)
                {
                    if (!ChatCommands.ContainsKey(attribute.Command))
                    {
                        ChatCommands.Add(attribute.Command, new Tuple<AuthorityType, Action<IPlayerEntity, string[]>>(attribute.MinAuthorization, action));
                    }
                }
            }
        }
    }
}
