
using System;
using System.Configuration;
using DomainCore;
using Antlr.StringTemplate;
using log4net;

namespace PropertyManager
{
    
    
    public class DomainRenderer
    {
        private static DomainRenderer instance = null;
        private const string RELATIVE_CONFIG = "UseRelativeTemplatesLocation";
        private const string TEMPLATE_LOCATION_CONFIG = "TemplatesLocation";
        private const string DOMAIN_TEMPLATE_GROUP = "Domain";
        private const string BASE_TEMPLATE_FORMAT = "Templates/{0}{1}";

        private StringTemplateGroup stGroup;

        protected ILog log;
        
        private DomainRenderer()
        {
            log = LogManager.GetLogger(typeof(DomainRenderer));
            
            if (ConfigurationManager.AppSettings[RELATIVE_CONFIG].Equals("true"))
            {
                stGroup = new StringTemplateGroup(DOMAIN_TEMPLATE_GROUP);
            }
            else
            {
                string location = ConfigurationManager.AppSettings[TEMPLATE_LOCATION_CONFIG];
                stGroup = new StringTemplateGroup(DOMAIN_TEMPLATE_GROUP, location);
            }
            
        }

        public static DomainRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DomainRenderer();
                }

                return instance;
            }
        }

        public static string Render(Domain domain, string type)
        {
            return Instance.RenderObject(domain, type);
        }

        private string GetTemplateName(Domain domain, string type)
        {
            string domainName = domain.GetType().Name;

            return string.Format(BASE_TEMPLATE_FORMAT, domainName, type);
        }

        public string RenderObject(Domain domain, string type)
        {
            string templateName = GetTemplateName(domain, type);
            log.DebugFormat("Loading StringTemplate: {0}", templateName);
            StringTemplate st = stGroup.GetInstanceOf(templateName);

            st.SetAttribute("domain", domain);

            return st.ToString();
        }
    }
}
