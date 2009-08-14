
using System;
using System.Collections;
using System.Collections.Generic;
using DomainCore;

namespace ControlWrappers
{
    
    public delegate void ContextChangeHandler(string contextName, string itemName);
    
    public class DataContext
    {
        private string name;
        private Hashtable contents;
        
        public DataContext()
        {
            contents = new Hashtable();
        }

        public event ContextChangeHandler ContextChanged;

        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnContextChanged(null);
            }
        }

        public void AddObject(string name, object obj)
        {
            contents[name] = obj;
            OnContextChanged(name);
        }

        public void RemoveObject(string name)
        {
            if (contents.ContainsKey(name))
            {
                contents.Remove(name);
                OnContextChanged(name);
            }
        }

        public object this[string name]
        {
            get
            {
                return contents[name];
            }
            set
            {
                contents[name] = value;
            }
        }

        private void OnContextChanged(string item)
        {
            ContextChangeHandler handler = ContextChanged;

            if (handler != null)
            {
                handler(name, item);
            }
        }

        public List<Domain> GetCollection(string name)
        {
            return this[name] as List<Domain>;
        }

        public Domain GetDomain(string name)
        {
            return this[name] as Domain;
        }
    }
}
