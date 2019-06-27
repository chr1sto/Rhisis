using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Game.Common
{
    public sealed class Delayer : IDisposable
    {
        private readonly Dictionary<Guid, DelayedAction> _delayedActions;

        private class DelayedAction
        {
            private readonly CancellationTokenSource _cancellationTokenSource;

            public TimeSpan DelayTime { get; }

            public Action Action { get; }

            public DelayedAction(TimeSpan delayTime, Action delayedAction)
            {
                this.DelayTime = delayTime;
                this.Action = delayedAction;
                this._cancellationTokenSource = new CancellationTokenSource();
            }

            public void Start()
            {
                Task.Delay(this.DelayTime, this._cancellationTokenSource.Token).ContinueWith(_ => this.Action());
            }

            public void Cancel()
            {
                this._cancellationTokenSource.Cancel(false);
            }
        }

        public Delayer()
        {
            this._delayedActions = new Dictionary<Guid, DelayedAction>();
        }

        public Guid DelayAction(TimeSpan delayTime, Action action)
        {
            var delayedActionId = Guid.NewGuid();
            var delayedAction = new DelayedAction(delayTime, action);

            this._delayedActions.Add(delayedActionId, delayedAction);

            delayedAction.Start();

            return delayedActionId;
        }

        public Guid DelayAction(int delaySeconds, Action delayedAction) 
            => this.DelayAction(TimeSpan.FromSeconds(delaySeconds), delayedAction);

        public void CancelAction(Guid delayedActionId)
        {
            if (this._delayedActions.TryGetValue(delayedActionId, out DelayedAction delayedAction))
            {
                delayedAction.Cancel();
            }
        }

        public void CancelAllActions()
        {
            foreach (KeyValuePair<Guid, DelayedAction> action in this._delayedActions)
            {
                action.Value.Cancel();
            }

            this._delayedActions.Clear();
        }

        public void Dispose()
        {
            this._delayedActions.Clear();
        }
    }
}
