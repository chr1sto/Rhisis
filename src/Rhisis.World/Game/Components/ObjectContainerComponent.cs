using Sylver.Network.Data;
using System.Collections.Generic;

namespace Rhisis.World.Game.Components
{
    public class ObjectContainerComponent<TObject>
        where TObject : class
    {
        protected List<TObject> _objects;

        /// <summary>
        /// Gets the maximum capacity of the container.
        /// </summary>
        public int MaxCapacity { get; }

        /// <summary>
        /// Gets the objects collection of the container.
        /// </summary>
        public IReadOnlyList<TObject> Objects => _objects;

        /// <summary>
        /// Gets the amount of object in the container.
        /// </summary>
        public virtual int Count { get; }

        /// <summary>
        /// Creates a new <see cref="ObjectContainerComponent{TObject}"/> instance.
        /// </summary>
        /// <param name="maxCapacity">Maximum capacity.</param>
        protected ObjectContainerComponent(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
        }

        /// <summary>
        /// Serialize the object container.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        public virtual void Serialize(INetPacketStream packet)
        {
            // Nothing to do here.
        }

        /// <summary>
        /// Checks if the given slot is available.
        /// </summary>
        /// <param name="slotIndex">Slot index.</param>
        /// <returns>True if the slot is available, false otherwise.</returns>
        public virtual bool IsSlotAvailable(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxCapacity)
            {
                return false;
            }

            return _objects[slotIndex] == null;
        }

        /// <summary>
        /// Adds a <typeparamref name="TObject"/> at a given slot.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        /// <param name="slotIndex">Target slot index.</param>
        /// <returns>True if the object has been added; false otherwise.</returns>
        public bool Add(TObject obj, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxCapacity)
            {
                return false;
            }

            _objects[slotIndex] = obj;

            return true;
        }

        /// <summary>
        /// Removes a <typeparamref name="TObject"/> from a given slot index.
        /// </summary>
        /// <param name="slotIndex">Target slot index.</param>
        /// <returns>True if the object has been removed; false otherwise.</returns>
        public bool RemoveAt(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxCapacity)
            {
                return false;
            }

            _objects[slotIndex] = null;

            return true;
        }
    }
}