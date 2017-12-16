using Rhisis.Core.Helpers;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    public class Entity : IEntity, IDisposable, IEqualityComparer<IEntity>
    {
        private bool _disposedValue;

        public int Id { get; }

        public virtual WorldEntityType Type { get; }

        public IContext Context { get; }

        public ObjectComponent ObjectComponent { get; }

        internal Entity(IContext context)
        {
            this.Id = RandomHelper.GenerateUniqueId();
            this.Context = context;
            this.ObjectComponent = new ObjectComponent();
        }

        public bool Equals(IEntity x, IEntity y) => x.Id == y.Id;

        public int GetHashCode(IEntity obj)
        {
            int hashCode = obj.Id ^ (int)obj.Type;

            return hashCode.GetHashCode();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // TODO: delete resources
                }

                this._disposedValue = true;
            }
        }
    }
}
