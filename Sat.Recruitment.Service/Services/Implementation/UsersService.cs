
 
using Sat.Recruitment.Service.Dtos;
using Sat.Recruitment.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
 
using System.Threading.Tasks;

namespace Sat.Recruitment.Service.Services.Implementation
{
    public class UsersService : IUsersService
    {
        private IDataService dataUsersService;

        

        public UsersService(  IDataService _dataUsersService)
        { 

            this.dataUsersService = _dataUsersService;
        }

        public async  Task<bool> CreateUser(User user,string userTextPath)
        {
            this.CalculateUserBonus(user);

            try
            {
                var result = await dataUsersService.CreateUser(user, userTextPath);

                return result;
            }
            catch (Exception e) 
            {

                throw e;
            }
          
            
        }


        public async Task<List<User>> GetUsers(  string userTextPath)
        {
            var result = await dataUsersService.GetUsers( userTextPath);

            return result;
        }

       public void  CalculateUserBonus(User user)
        {
            switch (user.UserType)
            {
                case UserType.Normal:
                    user.Money = user.Money > 100?   (user.Money * Convert.ToDecimal(0.12)) + user.Money : user.Money < 100 && user.Money > 10?  (user.Money * Convert.ToDecimal(0.8)) + user.Money :user.Money;
                break;

                case UserType.Premium:
                    user.Money = user.Money > 100 ?  (user.Money * Convert.ToDecimal(2)) + user.Money : user.Money;
                    break;
                case UserType.SuperUser:
                    user.Money = user.Money > 100 ?  (user.Money * Convert.ToDecimal(0.20)) + user.Money:user.Money;
                    break;
              
            }
        }

       
    }


}
 
