using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    public class DialogData
    {
        public const string PlayerNameText = "%PLAYERNAME%";

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
        public string IntroText { get; set; }

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
            this.Name = name;
            this.OralText = oralText;
            this.IntroText = introText;
            this.ByeText = byeText;
            this.Links = new List<DialogLink>();
        }
    }

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
        /// Gets or sets the dialog link text.
        /// </summary>
        /// <remarks>
        /// This text will appear once the client has clicked on the link title.
        /// </remarks>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Create an empty <see cref="DialogLink"/> instance.
        /// </summary>
        public DialogLink()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Create a <see cref="DialogLink"/> instance.
        /// </summary>
        /// <param name="id">Dialog link id</param>
        /// <param name="title">Dialog link title</param>
        /// <param name="text">Dialog link text</param>
        public DialogLink(string id, string title, string text)
        {
            this.Id = id;
            this.Title = title;
            this.Text = text;
        }
    }
}