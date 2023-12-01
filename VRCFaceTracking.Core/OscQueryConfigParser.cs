﻿using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VRCFaceTracking.Core;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.OSC.DataTypes;
using VRCFaceTracking.Core.OSC.Query.mDNS.Types.OscQuery;
using VRCFaceTracking.Core.Params;
using VRCFaceTracking.OSC;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VRCFaceTracking
{
    public class OscQueryConfigParser
    {
        private static ILogger<OscQueryConfigParser> _logger;

        public OscQueryConfigParser(ILogger<OscQueryConfigParser> parserLogger)
        {
            _logger = parserLogger;
        }

        public static string AvatarId = "";
        private readonly HttpClient _httpClient = new();

        public async Task<(IAvatarInfo avatarInfo, List<Parameter> relevantParameters)?> ParseNewAvatar(IPEndPoint oscQueryEndpoint)
        {
            // Request on the endpoint + /avatar/parameters
            var httpEndpoint = "http://" + oscQueryEndpoint + "/avatar";
            
            // Get the response
            var response = await _httpClient.GetStringAsync(httpEndpoint);
            
            var avatarConfig = JsonConvert.DeserializeObject<OSCQueryNode>(response);
            var avatarInfo = new OscQueryAvatarInfo(avatarConfig);
            
            // Reset all parameters
            var paramList = new List<Parameter>();
            foreach (var parameter in UnifiedTracking.AllParameters_v2.Concat(UnifiedTracking.AllParameters_v1).ToArray())
            {
                paramList.AddRange(parameter.ResetParam(avatarInfo.Parameters));
            }

            AvatarId = avatarInfo.Id;

            return (avatarInfo, paramList);
        }
    }
}