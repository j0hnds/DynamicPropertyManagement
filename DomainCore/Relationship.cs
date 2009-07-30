
using System;
using System.Text;
using System.Collections.Generic;

namespace DomainCore
{
    
    
    public class CollectionRelationship : Relationship
    {
        private string name;
        private string domainName;
        private string parentIdAttribute;
        private List<Domain> domains;
        
        public CollectionRelationship(Domain domain, string name, string domainName, string parentIdAttribute)
        {
            this.name = name;
            this.domainName = domainName;
            this.parentIdAttribute = parentIdAttribute;

            domains = new List<Domain>();

            if (domain != null)
            {
                domain.AddRelationship(this);
            }
        }

        public void AddObject(Domain domain)
        {
            domains.Add(domain);
        }

        public Domain AddNewObject()
        {
            Domain domain = DomainFactory.Create(domainName);
            AddObject(domain);

            return domain;
        }

        public void RemoveObject(Domain domain)
        {
            if (domains.IndexOf(domain) >= 0)
            {
                if (domain.NewObject)
                {
                    domains.Remove(domain);
                }
                else
                {
                    domain.ForDelete = true;
                }
            }
        }


        public int CountObjects()
        {
            int count = 0;

            foreach (Domain domain in domains)
            {
                if (! domain.ForDelete)
                {
                    count++;
                }
            }

            return count;
        }

        #region Relationship implementation
        public void Save (object parentId)
        {
            foreach (Domain domain in domains)
            {
                domain.SetValue(this.parentIdAttribute, parentId);
                domain.Save();
            }
        }
        
        public string SaveSQL (object parentId)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Domain domain in domains)
            {
                domain.SetValue(this.parentIdAttribute, parentId);
                if (sb.Length > 0)
                {
                    sb.Append(";");
                }
                sb.Append(domain.SaveSQL());
            }

            return sb.ToString();
        }
        public List<Domain> CollectedObjects
        {
            get
            {
                List<Domain> objects = new List<Domain>();
                foreach (Domain dom in domains)
                {
                    if (! dom.ForDelete)
                    {
                        objects.Add(dom);
                    }
                }
    
                return objects;
            }
        }
        public string Name
        {
            get { return name; }
        }

        public bool Dirty
        {
            get
            {
                bool dirty = false;

                foreach (Domain domain in domains)
                {
                    if (domain.Dirty)
                    {
                        dirty = true;
                        break;
                    }
                }

                return dirty;
            }
        }
        
        #endregion




        public string DomainName
        {
            get { return domainName; }
        }
    }
}
