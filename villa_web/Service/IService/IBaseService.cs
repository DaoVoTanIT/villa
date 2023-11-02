using villa_web.Models;

namespace villa_web.Service.Iservice{
    public interface IBaseService{
        APIResponse responseModel {get; set;}
        Task<T> SendAsync<T>(APIRequest aPIRequest);
    }
}