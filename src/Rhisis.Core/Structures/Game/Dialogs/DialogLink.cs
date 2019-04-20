using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game.Dialogs
{
    [DataContract]
    public class DialogLink
    {
        /// <summary>
        /// Gets or sets the dialog link id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the dialog link title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the dialog link texts.
        /// </summary>
        /// <remarks>
        /// This text will appear once the client has clicked on the link title.
        /// </remarks>
        [DataMember(Name = "texts")]
        public IList<string> Texts { get; set; }

        /// <summary>
        /// Create an empty <see cref="DialogLink"/> instance.
        /// </summary>
        public DialogLink()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Create a <see cref="DialogLink"/> instance.
        /// </summary>
        /// <param name="id">Dialog link id</param>
        /// <param name="title">Dialog link title</param>
        /// <param name="text">Dialog link text</param>
        public DialogLink(string id, string title)
        {
            this.Id = id;
            this.Title = title;
            this.Texts = new List<string>();
        }
    }
}