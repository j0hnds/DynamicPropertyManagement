
using System;
using System.Collections.Generic;
using Gtk;
using DomainCore;

namespace ControlWrappers
{
    
    public class BoundDialog : Gtk.Dialog, ContextContainer
    {
        private List<BaseBoundControl> boundControls = new List<BaseBoundControl>();

        protected List<BaseBoundControl> BoundControls
        {
            get { return boundControls; }
        }

        public void AddBoundControl(BaseBoundControl boundControl)
        {
            boundControls.Add(boundControl);
        }

        public DataContext GetContext(string name)
        {
            return Data[name] as DataContext;
        }
        

        protected void ContextChangeHandler(string contextName, string itemName)
        {
            // This is where we would make the call the reload all the
            // data bound controls.
        }

        public void SetContext(DataContext context)
        {
            Data[context.Name] = context;

            context.ContextChanged += ContextChangeHandler;

            // TODO: Seems like we should load the controls here.
        }

        protected void DomainToControls(Widget w)
        {
            if (w is BoundControl)
            {
                ((BoundControl) w).SetFromContext();
            }
            else if (w is Container)
            {
                foreach (Widget widget in ((Container) w).AllChildren)
                {
                    DomainToControls(widget);
                }
            }
                
        }

        protected void DomainToControls()
        {
            DomainToControls(Child);
        }

        public virtual bool DoModal(Window parentWindow, Domain domain)
        {
            bool ok = false;

            TransientFor = parentWindow;

            DomainToControls(domain);

            int response = Run();
            if (response == Gtk.ResponseType.Ok.value__)
            {
                ok = true;
                ControlsToDomain(domain);
            }

            Destroy();

            return ok;
        }

        protected virtual void DomainToControls(Domain domain)
        {
            foreach (BaseBoundControl bbc in boundControls)
            {
                bbc.SetControlValue(domain);
            }
        }

        protected virtual void ControlsToDomain(Domain domain)
        {
            foreach (BaseBoundControl bbc in boundControls)
            {
                bbc.GetControlValue(domain);
            }
        }
    }
    
}
