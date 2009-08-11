
using System;
using Gtk;
using DomainCore;

namespace PropertyManager
{
    
    
    public partial class BufferDisplayDlg : Gtk.Dialog
    {

        private MonospacedTextViewControl textview;
        
        public BufferDisplayDlg()
        {
            this.Build();

            textview = new MonospacedTextViewControl(textViewMain);
        }

        public bool DoModal(Window parentWindow, Domain domain)
        {
            bool ok = false;

            TransientFor = parentWindow;

            string sql = domain.SaveSQL();

            textview.TagText("Monospaced", sql);

            int response = Run();
            if (response == Gtk.ResponseType.Ok.value__)
            {
                ok = true;
            }

            Destroy();

            return ok;
        }
    }
}
