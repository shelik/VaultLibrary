using NUnit.Framework;
using VaultLibrary;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestVault
{
    public struct KV<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
    public class Tests
    {
        private Vault vaultClient;

        [SetUp]
        public void Setup()
        {
            vaultClient = new Vault("http://localhost:8200/v1", "s.Fxb5KSlykumdBBPHnwERjloB"); 
        }

        [Test]
        public void TestGet()
        {
            var response = vaultClient.GetValueAsync<KV<Dictionary<string, string>>>("kv/data", "w").Result;
            Assert.AreEqual(response.Data["e"], "e");
        }
        [Test]
        public async Task TestPostKVDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("zzx11", "v");

            KV<Dictionary<string, string>> secKV = new KV<Dictionary<string, string>>
            {
                Data = dict
            };
                            
            await vaultClient.SetValueAsync("kv/data", "w", secKV);

            var response = vaultClient.GetValueAsync<KV<Dictionary<string, string>>>("kv/data", "w").Result;
            Assert.AreEqual(response.Data["zzx11"], "v");
        }
    }
}