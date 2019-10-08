namespace AuthService.API.Services
{
    using AuthService.API.Entities.Repository;
    using AuthService.API.Models;
    using System;

    public class UserInfoService : IUserService
    {
        private readonly IUserInfoRepository userRepository;

        public UserInfoService(IUserInfoRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool IsUserExist(UserModel userModel)
        {
            var userEntity = new Entities.UserDetail
            {
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber
            };
            var user = userRepository.GetById(userEntity);
            return user != null;
        }

        public UserModel GetUser(string email, string password)
        {
            var user = userRepository.GetUserByUserIdAndPasssword(email, password);

            if (user != null)
            {
                return new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    ConfirmPassword = user.Password,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Password,
                    Gender = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.PhoneNumber,
                    Status = user.Status,
                    Insurance = user.Insurance,
                    Description = user.Description,
                    License = user.License,
                    Permit = user.Permit,
                    Registration = user.Registration,
                    VehicleNumber = user.VehicleNumber
                };
            }

            return null;
        }

        public int Add(UserModel userModel)
        {
            var userEntity = new Entities.UserDetail
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Password = userModel.Password,
                ConfirmPassword = userModel.ConfirmPassword,
                DateOfBirth = userModel.DateOfBirth,
                Email = userModel.Email,
                Gender = userModel.Gender,
                PhoneNumber = userModel.PhoneNumber,
                RoleId = userModel.PhoneNumber,
                Status = userModel.Status,
                Insurance = userModel.Insurance,
                Description = userModel.Description,
                License = userModel.License,
                Permit = userModel.Permit,
                Registration = userModel.Registration,
                VehicleNumber = userModel.VehicleNumber,
            };

            return userRepository.Add(userEntity);
        }
    }
}
