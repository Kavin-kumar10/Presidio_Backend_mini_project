using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.ActivationExceptions;
using RefundManagementApplication.Exceptions.AuthExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.AuthReqDTOs;
using RefundManagementApplication.Models.DTOs.ResponseDTO.LoginResponseDTOs;
using RefundManagementApplication.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace RefundManagementApplication.Services
{
    public class UserServices : IUserServices
    {
        private readonly IRepository<int, Member> _memrepo;
        private readonly IRepository<int,User> _userRepo;
        private readonly ITokenServices _tokenServices;

        public UserServices(IRepository<int,Member> memrepo,IRepository<int,User> userRepo,ITokenServices tokenservices) {
            _memrepo = memrepo;
            _userRepo = userRepo;
            _tokenServices = tokenservices;
        }
        public async Task<LoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            var userDB = await _userRepo.Get(loginDTO.UserId);
            if (userDB == null)
            {
                throw new UnauthorizedUserException("Invalid username or password");
            }
            HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            bool isPasswordSame = ComparePassword(encrypterPass, userDB.Password);
            if (isPasswordSame)
            {
                var member = await _memrepo.Get(loginDTO.UserId);
                if (userDB.Status == "Active")
                {
                    LoginReturnDTO loginReturnDTO = MapMemberToLoginReturn(member);
                    return loginReturnDTO;
                }

                throw new UserNotActiveException("Your account is not activated");
            }
            throw new UnauthorizedUserException("Invalid username or password");
        }

        private LoginReturnDTO MapMemberToLoginReturn(Member member)
        {
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.MemberID = member.Id;
            returnDTO.Role = member.Role;
            returnDTO.Token = _tokenServices.GenerateToken(member);
            return returnDTO;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<Member> Register(RegisterRequestDTO registerRequestDTO)
        {
            Member member = new Member();
            User user = new User();
            try
            {
                member = new Member();
                member.email = registerRequestDTO.email;
                member.Name = registerRequestDTO.Name;
                member.Role = MemberRole.User;
                member = await _memrepo.Add(member);
                user = await MapRegisterRequestDTOtoUser(registerRequestDTO);
                user.MemberId = member.Id;
                user = await _userRepo.Add(user);
                return member;
            }
            catch (Exception ex) {
                throw new Exception();
            }
            //if (user == null)
            //    await RevertMemberInsert(member);
            //if (member == null)
            //    await RevertUserInsert(user);
            throw new UnableToRegisterException("Not able to register at this moment");
        }

        private async Task RevertUserInsert(User user)
        {
            await _userRepo.Delete(user.MemberId);
        }

        private async Task RevertMemberInsert(Member member)
        {

            await _memrepo.Delete(member.Id);
        }

        public async Task<User> MapRegisterRequestDTOtoUser(RegisterRequestDTO registerRequestDTO)
        {
            User user = new User();
            HMACSHA512 hMACSHA = new HMACSHA512();
            user.Status = "Disabled";
            user.PasswordHashKey = hMACSHA.Key;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(registerRequestDTO.password));
            return user;
        }
    }
}
