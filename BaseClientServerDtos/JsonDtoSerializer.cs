using Newtonsoft.Json;
using System;

namespace BaseClientServerDtos
{
    public static class JsonDtoSerializer
    {
        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None // We do our own custom version of this in client->server communications
            };
            return serializerSettings;
        }

        public static string SerializeDto(FiniteDto dto)
        {
            string serialized = JsonConvert.SerializeObject(dto, Formatting.None, GetJsonSerializerSettings());
            return serialized;
        }

        public static string GetDtoName(string serializedDto)
        {
            try
            {
                FiniteDto dto = JsonConvert.DeserializeObject<FiniteDto>(serializedDto);
                return dto.DtoName;
            }
            catch
            {
                // TODO: maybe track why we got an empty named or invalid dto here.
            }
            return null;
        }

        public static T DeserializeAs<T>(string serializedDto)
        {
            try
            {
                T dto = JsonConvert.DeserializeObject<T>(serializedDto);
                return dto;
            }
            catch
            {
                // ignore
            }
            return default(T);
        }
    }
}
