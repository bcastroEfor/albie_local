using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ActioBP.General.ResponseModels
{
    public class CustomActionResult: ContentResult
    {
        public CustomActionResult(HttpStatusCode statusCode, IEnumerable<string> messages)
        {
            var resp = new { Status = statusCode, StatusCode = (int)statusCode, Message = messages };

            this.StatusCode = (int)statusCode;
            this.Content = Newtonsoft.Json.JsonConvert.SerializeObject(resp);
            this.ContentType = "application/json; charset=utf-8";
        }
    }
}
