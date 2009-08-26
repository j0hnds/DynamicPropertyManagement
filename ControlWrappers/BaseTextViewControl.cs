
using System;
using Gtk;

namespace ControlWrappers
{

    /// <summary>
    /// A base class for all wrapped TextView controls.
    /// </summary>
    /// <remarks>
    /// Provides a set of methods that are generally useful for TextView controls
    /// in the context of templated text.
    /// </remarks>
    public abstract class BaseTextViewControl : BaseControl
    {
        /// <summary>
        /// The text buffer for the TextView control
        /// </summary>
        protected TextBuffer buffer;

        /// <summary>
        /// Constructs a new BaseTextViewControl.
        /// </summary>
        /// <param name="widget">
        /// The TextView widget to be wrapped.
        /// </param>
        public BaseTextViewControl(Widget widget) : 
            base(widget)
        {
            this.buffer = ((TextView) widget).Buffer;
        }

        /// <summary>
        /// Replaces the text currently in the control's buffer with the specified
        /// tagged text.
        /// </summary>
        /// <param name="tag">
        /// The tag to apply to the text in the control.
        /// </param>
        /// <param name="text">
        /// The text to be placed in the control with the specified tag.
        /// </param>
        public void TagText(string tag, string text)
        {
            TextIter startIter = buffer.StartIter;
            TextIter endIter = buffer.EndIter;

            // Clear the buffer.
            buffer.Delete(ref startIter, ref endIter);

            buffer.InsertWithTagsByName(ref startIter, text, tag);
        }

        /// <summary>
        /// Renders the specified text into the control's text buffer.
        /// </summary>
        /// <remarks>
        /// It is assumed that the supplied text string could contain embedded tag specifications
        /// which control the display of portions of the text. The markup for the tags is
        /// relatively straightforward: to tag text, simply provide the markup as follows:
        /// <code>untagged_text{{tag_name}}text_to_tag{{/}}untagged_text</code>
        /// where:
        /// <list type="bullet">
        /// <item></item>
        /// </list>
        /// </remarks>
        /// <param name="text">
        /// A <see cref="System.String"/>
        /// </param>
        public void Render(string text)
        {
            TextIter startIter = buffer.StartIter;
            TextIter endIter = buffer.EndIter;

            log.DebugFormat("Rendering text block [{0}]", text);
            
            // Clear the buffer.
            buffer.Delete(ref startIter, ref endIter);

            int startPos = 0;
            int i = text.IndexOf("{{", startPos);
            while (i >= 0)
            {
                string substr = text.Substring(startPos, i - startPos);
                log.DebugFormat("Leading text [{0}]", substr);
                if (substr.Length > 0)
                {
                    buffer.Insert(ref startIter, substr);
                }

                int endPos = text.IndexOf("}}", i);
                if (endPos < 0)
                {
                    throw new Exception(string.Format("Invalid tag template: {0}", text));
                }

                int eot = endPos + 1;

                string startTag = text.Substring(i, (eot + 1) - i);
                string tagName = startTag.Substring(2, startTag.Length - 4);
                log.DebugFormat("Tag name to apply [{0}]", tagName);

                int endTagPos = text.IndexOf("{{/}}", eot);
                if (endTagPos < 0)
                {
                    throw new Exception(string.Format("Unable to find end tag for tag ({0}): {1}", tagName, text));
                }

                string taggedText = text.Substring(eot + 1, endTagPos - (eot + 1));
                if (taggedText.Length > 0)
                {
                    buffer.InsertWithTagsByName(ref startIter, taggedText, tagName);
                }

                startPos = endTagPos + 5;

                i = text.IndexOf("{{", startPos);
            }

            string remainder = text.Substring(startPos);
            if (remainder.Length > 0)
            {
                buffer.Insert(ref startIter, remainder);
            }
        }
    }
}
