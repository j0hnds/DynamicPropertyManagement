
using System;
using Gtk;

namespace ControlWrappers
{
    
    
    public class BaseTextViewControl : BaseControl
    {
        protected TextBuffer buffer;
        
        public BaseTextViewControl(Widget widget) : 
            base(widget)
        {
            this.buffer = ((TextView) widget).Buffer;
        }

        public void TagText(string tag, string text)
        {
            TextIter startIter = buffer.StartIter;
            TextIter endIter = buffer.EndIter;

            // Clear the buffer.
            buffer.Delete(ref startIter, ref endIter);

            buffer.InsertWithTagsByName(ref startIter, text, tag);
        }

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
