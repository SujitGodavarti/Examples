﻿using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Controllers
{
    /// <summary>
    /// Settings controller
    /// </summary>
    [Authorize(Policy = "AuthenticatedPosNotifyUser")]
    [Route("api/v1/settings")]
    public class SettingsController : Controller
    {
        /// <summary>
        /// Application settings
        /// </summary>
        private readonly AppSettings appSettings;

        public SettingsController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        /// <summary>
        /// Update config.json file data
        /// </summary>
        /// <param name="config">Configuration of Angular app</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostSettings([FromBody] AppConfig config)
        {
            if (config == null || config.ApiServer == null || config.Application == null)
            {
                return BadRequest();
            }

            var filePath = Path.Combine(Environment.CurrentDirectory, appSettings.ConfigFile);

            string output = JsonConvert.SerializeObject(config, Formatting.Indented);
            using (StreamWriter file = new StreamWriter(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, config);
            }

            return Ok();
        }
    }
}