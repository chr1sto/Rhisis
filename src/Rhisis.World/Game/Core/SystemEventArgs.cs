using System;

namespace Rhisis.World.Game.Core
{
    public abstract class SystemEventArgs : EventArgs
    {
        private readonly object[] _arguments;

        /// <summary>
        /// Creates a new <see cref="SystemEventArgs"/> instance.
        /// </summary>
        /// <param name="args">System arguments</param>
        protected SystemEventArgs(object[] args)
        {
            this._arguments = args;            
        }

        /// <summary>
        /// Checks the event arguments.
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckArguments();

        /// <summary>
        /// Gets an argument value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        protected T GetArgument<T>(int index)
        {
            if (index < 0 || index >= this._arguments.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            return (T)this._arguments[index];
        }
    }
}
