using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ipmc.extensionsapi.common;
using Newtonsoft.Json;
using Extensions = ipmc.extensionsapi.common.Extensions;
using System.Threading;

namespace ipmc.extensionsapi.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtensionsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ExtensionsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<List<Extensions>> GetExtensions()
        {
            var client = GetClient();
            var result = await client.GetFromJsonAsync<List<Extensions>>("") ?? new List<Extensions>();

            foreach (var extension in result)
            {
                var settings = await client.GetFromJsonAsync<List<ExtensionSetting>>($"{extension.ExtensionId}/settings");
                extension.Settings = settings;
            }

            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://apieuw.productmarketingcloud.com/api/v1.0.0/extensions/");
            client.DefaultRequestHeaders.Add("X-inRiver-APIKey", Request.Headers["X-inRiver-APIKey"].ToString());
            return client;
        }

        [HttpPost]
        public async Task<List<Extensions>> SaveExtensions(List<Extensions> extensions)
        {
            var client = GetClient();
            var savedExtensions = new List<Extensions>();

            foreach (var extension in extensions.Select(x => new {post = ToPostObject(x), ext = x}))
            {
                var result = await client.PostAsJsonAsync("", extension.post);
                foreach (var extensionSetting in extension.ext.Settings)
                {
                    await client.PutAsJsonAsync($"{extension.ext.ExtensionId}/settings", extensionSetting);
                }
                savedExtensions.Add(extension.ext);
            }

            return savedExtensions;
        }

        private PostObject ToPostObject(Extensions extensions)
        {
            return new PostObject
            {
                AssemblyName = extensions.AssemblyName,
                AssemblyType = extensions.AssemblyType,
                ExtensionId = extensions.ExtensionId,
                ExtensionType = extensions.ExtensionType,
                PackageId = extensions.Package.Id
            };
        }
    }

    public class PostObject
    {

        [JsonProperty("assemblyName")]
        public string AssemblyName { get; set; }

        [JsonProperty("assemblyType")]
        public string AssemblyType { get; set; }

        [JsonProperty("extensionId")]
        public string ExtensionId { get; set; }

        [JsonProperty("extensionType")]
        public string ExtensionType { get; set; }

        [JsonProperty("packageId")]
        public int PackageId { get; set; }
    }

    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            System.Diagnostics.Debug.WriteLine("Request:");
            System.Diagnostics.Debug.WriteLine(request.ToString());
            if (request.Content != null)
            {
                System.Diagnostics.Debug.WriteLine(await request.Content.ReadAsStringAsync());
            }
            System.Diagnostics.Debug.WriteLine("");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            System.Diagnostics.Debug.WriteLine("Response:");
            System.Diagnostics.Debug.WriteLine(response.ToString());
            if (response.Content != null)
            {
                System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
            }
            System.Diagnostics.Debug.WriteLine("");

            return response;
        }
    }
}
