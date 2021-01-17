﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartDisplay.Models;
using System.Net.Http;

namespace ChartDisplay.Controllers
{
    [Route("api/setdata")]
    [ApiController]
    public class SetDataController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> setData(DataSet dataSet)
        {
            HttpClient client = new HttpClient();
            var responseJson = "";
            try
            {
                string[] bodyArray = Array.ConvertAll(dataSet.Data, s => s.ToString());
                HttpContent content = new StringContent(string.Concat("[",string.Join(",",bodyArray),"]"));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var Result = await client.PostAsync("http://192.168.31.132/set", content);
                if (Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    responseJson = await Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return await Task.FromResult(responseJson);
        }
    }
}
