using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Tooth;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services.Interfaces
{
    public interface IToothService
    {
        public Task<BaseResponse<Tooth>> GetToothById(int id);
        public Task<BaseResponse<Tooth>> UpdateTooth(ToothUpdateRequest toothReq);
    }
}
