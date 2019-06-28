using System;

namespace Rhisis.World.Game.Core.Systems
{
    public abstract class SystemEventArgs : EventArgs
    {
        /// <summary>
        /// Checks the event arguments.
        /// </summary>
        /// <returns></returns>
        public abstract bool GetCheckArguments();
    }
}
