using System;
using System.Reflection;
using CBH.Chat.Domain;
using Microsoft.Extensions.Configuration;

namespace CBH.Chat.DependencyInjection
{
    public static class ConfigurationResolver
    {
        public static T GetConfiguration<T>(IConfigurationRoot config) where T : Configuration, new()
        {
            var configurationResult = new T();
            GetObjectProperties(config, string.Empty, configurationResult);
            return configurationResult;
        }

        private static void GetObjectProperties(IConfigurationRoot config, string previousSectionName, object propertyInstance)
        {
            var configurationResultType = propertyInstance.GetType();
            var allProperties = configurationResultType.GetProperties();
            var sectionName = configurationResultType.Name.Replace("Configuration", string.Empty);
            foreach (var subProperty in allProperties)
            {
                if (!string.IsNullOrWhiteSpace(previousSectionName) && !previousSectionName.EndsWith(":"))
                {
                    previousSectionName += ":";
                }

                SetPropertyValue(propertyInstance, subProperty, config, $"{previousSectionName}{sectionName}");
            }
        }

        private static void SetPropertyValue(object parentObject, PropertyInfo property, IConfigurationRoot config, string previousSectionName)
        {
            var propertyInstance = property.PropertyType == typeof(string) ? string.Empty : Activator.CreateInstance(property.PropertyType);

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

        private static bool IsPrimitive(Type type) => type.GetTypeInfo().IsPrimitive || type == typeof(string);
    }
}
