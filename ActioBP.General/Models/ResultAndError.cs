using ActioBP.General.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ActioBP.General.Models
{
    public class ResultAndError<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; }
        public bool HasErrors { get { return this.Errors.Any(); } }
        public T Result { get; set; }

        public ResultAndError()
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Errors = new List<string>();
        }
        public object ErrorsToObject()
        {
            return new { Message = this.Errors.ToArray() };
        }
        public string ErrorsToString(bool htmlLines = true)
        {
            var separator = ", ";
            if (htmlLines) separator = "<br />";
            var r = string.Join(separator, Errors);
            return r;
        }
        public ResultAndError<T> AddError(Exception error, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            this.StatusCode = errorCode;
            var eMessage = this.BuildError(error);

            return this.AddError(eMessage, errorCode);
        }
        public ResultAndError<T> AddError(string error, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            this.StatusCode = errorCode;
            this.Errors.Add(error);

            return this;
        }
        public ResultAndError<T> AddError(List<Exception> errors, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            ResultAndError<T> resp = this;
            if (errors == null) return resp;

            this.StatusCode = errorCode;
            foreach (var e in errors)
            {
                resp = resp.AddError(e, errorCode);
            }

            return resp;
        }
        public ResultAndError<T> AddError(List<string> errors, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            ResultAndError<T> resp = this;
            if (errors == null) return resp;

            this.StatusCode = errorCode;
            foreach (var e in errors)
            {
                resp = resp.AddError(e, errorCode);
            }

            return resp;
        }
        public ResultAndError<T> AddResult(T result)
        {
            this.Result = result;

            return this;
        }


        private string BuildError(Exception e)
        {
            return e.ToString(true);
        }
    }

}
