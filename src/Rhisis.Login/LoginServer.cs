using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Login.Client;
using Rhisis.Login.Packets;
using Rhisis.Network;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;
using System;
using System.Linq;

namespace Rhisis.Login
{
    public sealed class LoginServer : NetServer<LoginClient>, ILoginServer
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;
        private readonly ILogger<LoginServer> _logger;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        //protected override IPacketProcessor PacketProcessor { get; } = new FlyffPacketProcessor();

        /// <summary>
        /// Creates a new <see cref="LoginServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="loginConfiguration">Login server configuration.</param>
        /// <param name="iscConfiguration">ISC configuration.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public LoginServer(ILogger<LoginServer> logger, IOptions<LoginConfiguration> loginConfiguration, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._loginConfiguration = loginConfiguration.Value;
            this._serviceProvider = serviceProvider;
            this.PacketProcessor = new FlyffPacketProcessor();
            this.ServerConfiguration = new NetServerConfiguration(this._loginConfiguration.Host, 
                this._loginConfiguration.Port, 
                ClientBacklog, 
                ClientBufferSize);
        }

        /// <inheritdoc />
        protected override void OnAfterStart()
        {
            //TODO: Implement this log inside OnStarted method when will be available.
            this._logger.LogInformation($"{nameof(LoginServer)} is started and listen on {this.ServerConfiguration.Host}:{this.ServerConfiguration.Port}.");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(LoginClient client)
        {
            this._logger.LogInformation($"New client connected to {nameof(LoginServer)} from {client.Socket.RemoteEndPoint}.");

            client.Initialize(this, 
                this._serviceProvider.GetRequiredService<ILogger<LoginClient>>(), 
                this._serviceProvider.GetRequiredService<IHandlerInvoker>(),
                this._serviceProvider.GetRequiredService<ILoginPacketFactory>());
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(LoginClient client)
        {
            if (string.IsNullOrEmpty(client.Username))
                this._logger.LogInformation($"Unknwon client disconnected from {client.Socket.RemoteEndPoint}.");
            else
                this._logger.LogInformation($"Client '{client.Username}' disconnected from {client.Socket.RemoteEndPoint}.");
        }

        /// <inheritdoc />
        //protected override void OnError(Exception exception)
        //{
        //    this._logger.LogInformation($"{nameof(LoginServer)} socket error: {exception.Message}");
        //}

        /// <inheritdoc />
        public ILoginClient GetClientByUsername(string username)
            => this.Clients.FirstOrDefault(x =>
                x.IsConnected &&
                x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        /// <inheritdoc />
        public bool IsClientConnected(string username) => this.GetClientByUsername(username) != null;
    }
}