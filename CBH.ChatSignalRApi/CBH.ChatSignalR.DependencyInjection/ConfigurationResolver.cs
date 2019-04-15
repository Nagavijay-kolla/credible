using System;
using System.Reflection;
using CBH.ChatSignalR.Domain;
using Microsoft.Extensions.Configuration;

namespace CBH.ChatSignalR.DependencyInjection
{
    public static class ConfigurationResolver
    {
        public static T GetConfiguration<T>(IConfigurationRoot config) where T : Configuration, new()
        {
            var configurationResult = new T();
            GetObjectProperties(config, "", configurationResult);
            return configurationResult;
        }

        private static void GetObjectProperties(IConfigurationRoot config, string previousSectionName, object propertyInstance)
        {
            var configurationResultType = propertyInstance.GetType();
            var allProperties = configurationResultType.GetProperties();
            var sectionName = configurationResultType.Name.Replace("Configuration", "");
            foreach (var subProperty in allProperties)
            {
                if (!string.IsNullOrWhiteSpace(previousSectionName) && !previousSectionName.EndsWith(":")) previousSectionName += ":";
                SetPropertyValue(propertyInstance, subProperty, config, $"{previousSectionName}{sectionName}");
            }
        }

        private static void SetPropertyValue(object parentObject, PropertyInfo property, IConfigurationRoot config, string previousSectionName)
        {
            object propertyInstance;
            if (property.PropertyType == typeof(string)) propertyInstance = "";
            else propertyInstance = Activator.CreateInstance(property.PropertyType);
            if (IsPrimitive(property.PropertyType))
            {
                propertyInstance = config[$"{previousSectionName}:{property.Name}"];
            }
            else
            {
                GetObjectProperties(config, previousSectionName, propertyInstance);
            }
            property.SetValue(parentObject, propertyInstance);
        }

        private static bool IsPrimitive(Type type)
        {
            return type.GetTypeInfo().IsPrimitive || type == typeof(String);
        }
    }
}
