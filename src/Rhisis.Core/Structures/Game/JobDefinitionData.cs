using Rhisis.Core.Data;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    public class JobDefinitionData
    {
        [DataMember]
        public DefineJob.Job? Parent { get; set; }

        [DataMember]
        public DefineJob.JobType Type { get; set;}
    }
}
