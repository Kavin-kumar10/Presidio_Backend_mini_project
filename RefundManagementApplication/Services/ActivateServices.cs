using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.ResponseDTO.Activation;

namespace RefundManagementApplication.Services
{
    public class ActivateServices : IActivateServices
    {
        private IRepository<int, User> _repo;
        public ActivateServices(IRepository<int,User> repo) { 
            _repo = repo;
        }
        public async Task<ActivateReturnDTO> Activate(int MemberId)
        {
            ActivateReturnDTO returnDTO = new ActivateReturnDTO();
            var reqMem = await _repo.Get(MemberId);
            if (reqMem != null) {
                reqMem.Status = "Active";
                var res = await _repo.Update(reqMem);
                returnDTO.Id = res.MemberId;
                returnDTO.Status = res.Status;
                return returnDTO;
            }
            throw new UserNotFoundException();
        }

        public async Task<ActivateReturnDTO> Deactivate(int MemberId)
        {
            ActivateReturnDTO returnDTO = new ActivateReturnDTO();
            var reqMem = await _repo.Get(MemberId);
            if (reqMem != null)
            {
                reqMem.Status = "Disabled";
                var res = await _repo.Update(reqMem);
                returnDTO.Id = res.MemberId;
                returnDTO.Status = res.Status;
                return returnDTO;
            }
            throw new UserNotFoundException();
        }
    }
}
