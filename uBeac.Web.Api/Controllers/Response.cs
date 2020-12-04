using System.Collections.Generic;

namespace uBeac.Web.Api.Controllers
{
    public interface IResponse
    {
        int Duration { get; set; } // ms
        int StatusCode { get; set; }
        string Message { get; set; }
        Dictionary<string, string> Errors { get; }
        string TraceId { get; set; }
        string Language { get; set; }
        string SessionId { get; set; }
        IRequest Request { get; }
    }

    public interface IResponse<TEntity> : IResponse
    {
        TEntity Data { get; set; }
    }

    public class Response : IResponse
    {
        public int Duration { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; private set; }
        public string TraceId { get; set; }
        public string Language { get; set; }
        public string SessionId { get; set; }
        public IRequest Request { get; }

        public Response()
        {
            Errors = new Dictionary<string, string>();
        }
    }

    public class Response<TEntity> : Response, IResponse<TEntity>
    {
        public TEntity Data { get; set; }
    }
}
