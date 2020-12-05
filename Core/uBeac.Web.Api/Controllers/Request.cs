namespace uBeac.Web.Api.Controllers
{
    public interface IRequest
    {
        string Language { get; set; }
        string SessionId { get; set; }
    }

    public interface IRequest<TEntity> : IRequest
    {
        TEntity Data { get; set; }
    }

    public class Request<TEntity> : IRequest<TEntity>
    {
        public string Language { get; set; }
        public string SessionId { get; set; }
        public TEntity Data { get; set; }
    }
}
