using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Core
{
    public class Context
    {
        private static readonly Lazy<Context> lazyInstance = new Lazy<Context>(() => new Context());

        public static Context Instance => lazyInstance.Value;

        private Context()
        {
        }

        public IEntity CreateEntity()
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddSystem(ISystem system)
        {
            throw new NotImplementedException();
        }

        public void DeleteSystem(ISystem system)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
