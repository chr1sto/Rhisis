using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Job
{
    /// <summary>
    /// Describes the job system.
    /// </summary>
    public interface IJobSystem
    {
        /// <summary>
        /// Changes a player's job.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="newJob">New player job.</param>
        void ChangeJob(IPlayerEntity player, DefineJob.Job newJob);
    }
}
