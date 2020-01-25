using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Game.Common
{
    /// <summary>
    /// Delay actions in time.
    /// </summary>
    public sealed class Delayer : IDisposable
    {
        private readonly Dictionary<Guid, DelayedAction> _delayedActions;

        /// <summary>
        /// Represents a delayed action.
        /// </summary>
        private class DelayedAction : IDisposable
        {
            private readonly CancellationTokenSource _cancellationTokenSource;

            /// <summary>
            /// Gets the delay time before executing the action.
            /// </summary>
            public TimeSpan DelayTime { get; }

            /// <summary>
            /// Actions to execute.
            /// </summary>
            public Action Action { get; }

            /// <summary>
            /// Creates a new <see cref="DelayedAction"/> instance.
            /// </summary>
            /// <param name="delayTime">Delay time before executing the action.</param>
            /// <param name="delayedAction">Action to execute once delay has passed.</param>
            public DelayedAction(TimeSpan delayTime, Action delayedAction)
            {
                DelayTime = delayTime;
                Action = delayedAction;
                _cancellationTokenSource = new CancellationTokenSource();
            }

            /// <summary>
            /// Starts the delay then executes the action.
            /// </summary>
            public void Start()
            {
                Task.Delay(DelayTime, _cancellationTokenSource.Token).ContinueWith(_ =>
                {
                    if (!_cancellationTokenSource.IsCancellationRequested)
                        Action();
                });
            }

            /// <summary>
            /// Cancels the current action.
            /// </summary>
            public void Cancel() => _cancellationTokenSource.Cancel(false);

            /// <summary>
            /// Dispose the delayer action.
            /// </summary>
            public void Dispose() => Cancel();
        }

        /// <summary>
        /// Creates a new <see cref="Delayer"/> instance.
        /// </summary>
        public Delayer()
        {
            _delayedActions = new Dictionary<Guid, DelayedAction>();
        }

        /// <summary>
        /// Delay an action using a time stamp has delay.
        /// </summary>
        /// <param name="delayTime">Delay time before executing action.</param>
        /// <param name="action">Action to execute after delay has passed.</param>
        /// <returns>Delayed action unique Id.</returns>
        public Guid DelayAction(TimeSpan delayTime, Action action)
        {
            var delayedActionId = Guid.NewGuid();
            var delayedAction = new DelayedAction(delayTime, action);

            _delayedActions.Add(delayedActionId, delayedAction);

            delayedAction.Start();

            return delayedActionId;
        }

        /// <summary>
        /// Delay an action using seconds as time unit.
        /// </summary>
        /// <param name="delaySeconds">Delay time in seconds before executing action.</param>
        /// <param name="delayedAction">Action to execute after delay has passed.</param>
        /// <returns>Delayed action unique Id.</returns>
        public Guid DelayAction(int delaySeconds, Action delayedAction) 
            => DelayAction(TimeSpan.FromSeconds(delaySeconds), delayedAction);

        /// <summary>
        /// Cancels an action.
        /// </summary>
        /// <param name="delayedActionId">Delayed action id.</param>
        public void CancelAction(Guid delayedActionId)
        {
            if (_delayedActions.Remove(delayedActionId, out DelayedAction delayedAction))
            {
                delayedAction.Cancel();
            }
        }

        /// <summary>
        /// Cancel all actions.
        /// </summary>
        public void CancelAllActions()
        {
            foreach (KeyValuePair<Guid, DelayedAction> action in _delayedActions)
            {
                action.Value.Cancel();
            }

            _delayedActions.Clear();
        }

        /// <summary>
        /// Dispose the delayer instance.
        /// </summary>
        public void Dispose() => CancelAllActions();
    }
}
