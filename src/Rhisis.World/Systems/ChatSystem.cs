using Rhisis.Core.IO;
using Rhisis.Core.Reflection;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Chat;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rhisis.World.Systems
{
    [System]
    public class ChatSystem : NotifiableSystemBase
    {
        private static readonly IDictionary<string, Action<IPlayerEntity, string[]>> ChatCommands = new Dictionary<string, Action<IPlayerEntity, string[]>>();

        /// <summary>
        /// Gets the <see cref="ChatSystem"/> match filte.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <summary>
        /// Creates a new <see cref="ChatSystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public ChatSystem(IContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the <see cref="ChatSystem"/> logic.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        public override void Execute(IEntity entity, EventArgs e)
        {
            var chatEvent = e as ChatEventArgs;

            if (string.IsNullOrEmpty(chatEvent?.Message))
                return;
            
            var player = entity as IPlayerEntity;

            if (chatEvent.Message.StartsWith("/"))
            {
                string commandName = chatEvent.Message.Split(' ').FirstOrDefault();
                string[] commandParameters = this.GetCommandParameters(chatEvent.Message, commandName);

                if (ChatCommands.ContainsKey(commandName))
                    ChatCommands[commandName].Invoke(player, commandParameters);
                else
                    Logger.Warning("Unknow chat command '{0}'", commandName);
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
        private string[] GetCommandParameters(string command, string commandName)
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
                        ChatCommands.Add(attribute.Command, action);
                    }
                }
            }
        }
    }
}
