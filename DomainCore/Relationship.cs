
using System;
using System.Text;
using System.Collections.Generic;

namespace DomainCore
{
    
    /// <summary>
    /// A collection relationship, where the objects in the relationship
    /// are considered to be the children of a parent domain object.
    /// </summary>
    public class CollectionRelationship : Relationship
    {
        /// <summary>
        /// The name of the relationship.
        /// </summary>
        private string name;
        /// <summary>
        /// The name of the domain objects that are stored in the relationship.
        /// </summary>
        private string domainName;
        /// <summary>
        /// The name of the domain attribute that holds the ID of the
        /// parent object (parent-child).
        /// </summary>
        private string parentIdAttribute;
        /// <summary>
        /// The actual collection of domain objects that represents the
        /// objects in the relationship.
        /// </summary>
        private List<Domain> domains;

        /// <summary>
        /// Constructs a new Collection relationship.
        /// </summary>
        /// <param name="domain">
        /// The domain object that owns the relationship (parent).
        /// </param>
        /// <param name="name">
        /// The name of the relationship.
        /// </param>
        /// <param name="domainName">
        /// The name of the domain objects stored in the relationship
        /// </param>
        /// <param name="parentIdAttribute">
        /// The name of the attribute on the child objects that hold the
        /// unique identifier of the parent.
        /// </param>
        public CollectionRelationship(Domain domain, 
                                      string name, 
                                      string domainName, 
                                      string parentIdAttribute)
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

        /// <summary>
        /// A delegate which indicates which of the domain objects in
        /// a collection should be removed.
        /// </summary>
        /// <param name="domain">
        /// The domain object to test for removal.
        /// </param>
        /// <returns>
        /// If <c>true</c>, the domain object should be removed from the
        /// collection.
        /// </returns>
        private bool RemoveNewObject(Domain domain)
        {
            return domain.NewObject;
        }

        /// <summary>
        /// Adds an object to the relationship.
        /// </summary>
        /// <param name="domain">
        /// The domain object to add to the relationship
        /// </param>
        public void AddObject(Domain domain)
        {
            domains.Add(domain);
        }

        /// <summary>
        /// Adds a new object to the relationship.
        /// </summary>
        /// <returns>
        /// The newly added domain object with the NewObject flag set to <c>true</c>.
        /// </returns>
        public Domain AddNewObject()
        {
            Domain domain = DomainFactory.Create(domainName);
            AddObject(domain);

            return domain;
        }

        /// <summary>
        /// Removes the specified domain object from the collection.
        /// </summary>
        /// <param name="domain">
        /// The domain object to remove from the collection.
        /// </param>
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


        /// <summary>
        /// Counts the number of objects in the relationship. Does not
        /// count objects that are marked for deletion.
        /// </summary>
        /// <returns>
        /// The number of objects in the collection.
        /// </returns>
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
        
        public void Revert()
        {
            // Eliminate all the new objects first
            domains.RemoveAll(RemoveNewObject);

            // Now revert all remaining domains.
            foreach (Domain domain in domains)
            {
                domain.Revert();
            }
        }
        #endregion


        /// <value>
        /// The name of the domain objects in this relationship.
        /// </value>
        public string DomainName
        {
            get { return domainName; }
        }
    }
}
