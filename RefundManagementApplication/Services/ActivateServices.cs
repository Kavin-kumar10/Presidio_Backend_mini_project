using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.AuthExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.ResponseDTO.Activation;
using RefundManagementApplication.Models.Enums;
using System.Data;

namespace RefundManagementApplication.Services
{
    public class ActivateServices : IActivateServices
    {
        private IRepository<int, User> _userRepo;
        private IRepository<int, Member> _memRepo;

        public ActivateServices(IRepository<int,User> userrepo,IRepository<int,Member> memRepo) { 
            _userRepo = userrepo;
            _memRepo = memRepo;
        }
        public async Task<ActivateReturnDTO> Activate(int MemberId,MemberRole Role, Plan plan)
        {
            ActivateReturnDTO returnDTO = new ActivateReturnDTO();
            var reqUser = await _userRepo.Get(MemberId); // Activating Member
            var reqMember = await _memRepo.Get(MemberId); // Providing Role and Plan to Member
            if(reqMember != null) {
                reqMember.Role = Role;
                reqMember.Membership = plan;
                await _memRepo.Update(reqMember);
                returnDTO.Role = Role;
                returnDTO.Membership = plan;
            }
            if (reqUser != null) {
                reqUser.Status = "Active";
                var res = await _userRepo.Update(reqUser);
                returnDTO.Id = res.MemberId;
                returnDTO.Status = res.Status;
                return returnDTO;
            }
            throw new UserNotFoundException();
        }

        public async Task<ActivateReturnDTO> Deactivate(int MemberId)
        {
            ActivateReturnDTO returnDTO = new ActivateReturnDTO();
            var reqUser = await _userRepo.Get(MemberId);
            var reqMember = await _memRepo.Get(MemberId);
            if (reqUser != null)
            {
                reqUser.Status = "Disabled";
                var res = await _userRepo.Update(reqUser);
                returnDTO.Id = res.MemberId;
                returnDTO.Status = res.Status;
                returnDTO.Role = reqMember.Role;
                return returnDTO;
            }
            throw new UserNotFoundException();
        }
    }
}
