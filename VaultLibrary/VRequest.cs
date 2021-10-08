using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VaultLibrary
{

    internal class VRequest<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
