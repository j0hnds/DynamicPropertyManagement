
using System;
using System.Configuration;
using System.Collections.Generic;
using DomainCore;
using Antlr.StringTemplate;
using log4net;

namespace STUtils
{
    
    
    public class DomainRenderer
    {
        private static DomainRenderer instance = null;
        private const string BASE_TEMPLATES_GROUP = "Templates/BaseTemplates";
        private const string RELATIVE_CONFIG = "UseRelativeTemplatesLocation";
        private const string TEMPLATE_LOCATION_CONFIG = "TemplatesLocation";
        private const string DOMAIN_TEMPLATE_GROUP = "Domain";
        private const string BASE_TEMPLATE_FORMAT = "Templates/{0}{1}";

        private StringTemplateGroup baseTemplateGroup;

        private Dictionary<string,StringTemplateGroup> templateGroups;

        protected ILog log;
        
        private DomainRenderer()
        {
            log = LogManager.GetLogger(typeof(DomainRenderer));

            CommonGroupLoader loader = null;
            
            if (ConfigurationManager.AppSettings[RELATIVE_CONFIG].Equals("true"))
            {
                log.Info("Obtaining Templates from the Relative location");
                loader = new CommonGroupLoader(new ConsoleErrorListener());
            }
            else
            {
                string location = ConfigurationManager.AppSettings[TEMPLATE_LOCATION_CONFIG];
                log.InfoFormat("Obtaining Templates from this location: {0}", location);
                loader = new CommonGroupLoader(new ConsoleErrorListener(), location);
            }

            StringTemplateGroup.RegisterGroupLoader(loader);

            baseTemplateGroup = StringTemplateGroup.LoadGroup(BASE_TEMPLATES_GROUP);

            templateGroups = new Dictionary<string, StringTemplateGroup>();
            
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

        private StringTemplateGroup this[string domainName]
        {
            get
            {
                StringTemplateGroup stGroup = null;

                string templateName = string.Format("Templates/{0}", domainName);
                
                if (templateGroups.ContainsKey(templateName))
                {
                    log.DebugFormat("Re-using preloaded template: {0}", templateName);
                    stGroup = templateGroups[templateName];
                }
                else
                {
                    log.DebugFormat("Loading and caching template: {0}", templateName);
                    stGroup = StringTemplateGroup.LoadGroup(templateName, baseTemplateGroup);
                    templateGroups[templateName] = stGroup;
                }

                return stGroup;
            }
        }

        public string RenderObject(Domain domain, string type)
        {
            StringTemplateGroup stGroup = this[domain.GetType().Name];

            StringTemplate st = stGroup.GetInstanceOf(type);

            st.SetAttribute("domain", domain);

            return st.ToString();
        }
    }
}
