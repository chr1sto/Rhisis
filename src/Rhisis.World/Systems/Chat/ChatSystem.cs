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
        internal sealed class ChatCommand
        {
            /// <summary>
            /// The method to invoke.
            /// </summary>
            public Action<IPlayerEntity, string[]> Method { get; }

            /// <summary>
            /// The Minimum Authorization for this command.
            /// </summary>
            public AuthorityType MinAuthorization { get; }

            /// <summary>
            /// Creates a new <see cref="ChatCommand"/> instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="minAuthorization"></param>
            public ChatCommand(Action<IPlayerEntity, string[]> method, AuthorityType minAuthorization)
            {
                Method = method;
                MinAuthorization = minAuthorization;
            }
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly IDictionary<string, ChatCommand> ChatCommands = new Dictionary<string, ChatCommand>();

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

                if (ChatCommands.TryGetValue(commandName, out var command))
                {
                    if (player.PlayerData.Authority >= command.MinAuthorization)
                        command.Method.Invoke(player, commandParameters);
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
                        ChatCommands.Add(attribute.Command, new ChatCommand(action, attribute.MinAuthorization));
                    }
                }
            }
        }
    }
}
