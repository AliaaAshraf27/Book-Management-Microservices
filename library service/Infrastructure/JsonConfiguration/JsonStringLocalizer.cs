using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.JsonConfiguration
{ //read localized strings from a JSON file
    public class JsonStringLocalizer : IStringLocalizer //provides localized strings by key
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer = new();//convert data between JSON and objects (serialization)
        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
        }
        public LocalizedString this[string name]
        {
            // Takes the [BookAdded] and returns the value "string"
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            // Takes the [BookAdded] and returns the value "string {0}" = welcome aliaa
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments))
                    : actualValue;
            }
        }
        //opens a JSON file for the current culture
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;
                var key = reader.Value.ToString();
                reader.Read();
                var value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key, value);
            }
        }
        private string GetString(string key)
        {
            // start run from APICore 
            var filePath = Path.Combine(AppContext.BaseDirectory, $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json");
            var fullFilePath = Path.GetFullPath(filePath);// convert (Resources/en.json ) to user\aliaa\.................

            if (File.Exists(fullFilePath))
            {
                var result = GetValueFromJSON(key, fullFilePath);
                return result;
            }
            return string.Empty;
        }
        //read the json file and take the key
        private string GetValueFromJSON(string propertyName, string filePath)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
                return string.Empty;
            Console.WriteLine($"Looking for: {filePath}");
            //open a connection to a file
            // OR using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(stream); // convert bytes into text
            using JsonTextReader reader = new(streamReader); // read json file 
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == propertyName)
                {
                    reader.Read();
                    return _serializer.Deserialize<string>(reader);
                }
            }
            return string.Empty; // if the key isn’t found, return key name
        }
    }
}

