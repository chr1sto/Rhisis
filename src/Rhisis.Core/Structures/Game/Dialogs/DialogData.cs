using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game.Dialogs
{
    [DataContract]
    public class DialogData
    {
        /// <summary>
        /// Gets or sets the dialog name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dialog's oral text.
        /// </summary>
        [DataMember(Name = "oralText")]
        public string OralText { get; set; }

        /// <summary>
        /// Gets or sets the dialog's introduction text.
        /// </summary>
        [DataMember(Name = "introText")]
        public IEnumerable<string> IntroText { get; set; }

        /// <summary>
        /// Gets or sets the dialog's goodbye text.
        /// </summary>
        [DataMember(Name = "byeText")]
        public string ByeText { get; set; }

        /// <summary>
        /// Gets the dialog's links.
        /// </summary>
        [DataMember(Name = "links")]
        public List<DialogLink> Links { get; }

        /// <summary>
        /// Creates a new <see cref="DialogData"/> instance.
        /// </summary>
        public DialogData()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DialogData"/> instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="oralText"></param>
        /// <param name="introText"></param>
        /// <param name="byeText"></param>
        public DialogData(string name, string oralText, string introText, string byeText)
        {
            Name = name;
            OralText = oralText;
            IntroText = new[] { introText };
            ByeText = byeText;
            Links = new List<DialogLink>();
        }
    }
}