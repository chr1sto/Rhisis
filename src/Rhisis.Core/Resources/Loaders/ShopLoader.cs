using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Rhisis.Core.Structures.Game;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class ShopLoader : IGameResourceLoader
    {
        private readonly ILogger<ShopLoader> _logger;
        private readonly IDictionary<string, ShopData> _shopData;
        private readonly JsonLoadSettings _jsonSettings = new JsonLoadSettings { CommentHandling = CommentHandling.Ignore };

        /// <summary>
        /// Gets the shop data by the shop name.
        /// </summary>
        /// <param name="shopName">Shop name</param>
        /// <returns></returns>
        public ShopData this[string shopName] => this.GetShopData(shopName);

        /// <summary>
        /// Creates a new <see cref="ShopLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public ShopLoader(ILogger<ShopLoader> logger)
        {
            this._logger = logger;
            this._shopData = new Dictionary<string, ShopData>();
        }

        /// <inheritdoc />
        public void Load()
        {
            if (!Directory.Exists(GameResources.ShopsPath))
            {
                this._logger.LogWarning("Unable to load shops. Reason: cannot find '{0}' directory.", GameResources.ShopsPath);
                return;
            }

            string[] shopsFiles = Directory.GetFiles(GameResources.ShopsPath);

            foreach (string shopFile in shopsFiles)
            {
                string shopFileContent = File.ReadAllText(shopFile);
                var shopsParsed = JToken.Parse(shopFileContent, this._jsonSettings);

                if (shopsParsed.Type == JTokenType.Array)
                {
                    var shops = shopsParsed.ToObject<ShopData[]>();

                    foreach (ShopData shop in shops)
                        this.AddShop(shop);
                }
                else
                {
                    this.AddShop(shopsParsed.ToObject<ShopData>());
                }
            }

            this._logger.LogInformation("-> {0} shops loaded.", this._shopData.Count);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._shopData.Clear();
        }

        /// <summary>
        /// Gets the shop data by the shop name.
        /// </summary>
        /// <param name="shopName">Shop name</param>
        /// <returns></returns>
        public ShopData GetShopData(string shopName) => this._shopData.TryGetValue(shopName, out ShopData value) ? value : null;

        /// <summary>
        /// Adds a new shop.
        /// </summary>
        /// <param name="shop">Shop to add</param>
        private void AddShop(ShopData shop)
        {
            if (this._shopData.ContainsKey(shop.Name))
                this._logger.LogWarning(GameResources.ObjectIgnoredMessage, "Shop", shop.Name, "already declared");
            else
                this._shopData.Add(shop.Name, shop);
        }
    }
}
