using Sat.Recruitment.Service.Dtos;
using System;
using System.Collections.Generic;
using System.IO; 
using System.Threading.Tasks;
 using System.Text.RegularExpressions;
using System.Linq;
using Sat.Recruitment.Service.Services.Interfaces;
 

namespace Sat.Recruitment.Service.Services.Implementation
{
    public class FileDataService : IDataService
    {



        public FileDataService()
        {

        }

        public async Task<List<User>> GetUsers(string userTextPath)
        {
            var x = userTextPath;
            var reader = ReadUsersFromFile(userTextPath);

            var result = new List<User>();

            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();

                

               


              var   user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = (UserType)Enum.Parse(typeof(UserType), line.Split(',')[4].ToString(), true),   
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                result.Add(user);
            }

            reader.Close();

            return result;
        }


        public async Task<bool> CreateUser(User user, string userTextPath)
        {
            var users = await this.GetUsers(userTextPath);



            var newUser = new User()
            {
                Name = user.Name,
                 Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                UserType = user.UserType,
                Money = user.Money,
                
            

            };

            this.ValidateNewUser(newUser, users);

            users.Add(newUser);

            await this.SaveUserList(users, userTextPath);

            return true;

        }

        private void ValidateNewUser(User newUser, List<User> users)
        {
            this.ValidateEmail(newUser.Email);

            this.ValidateDuplicates(newUser, users);

        }

        private async Task CleanFile(string userTextPath)
        {
             
              await   System.IO.File.WriteAllTextAsync(Directory.GetCurrentDirectory() + userTextPath, String.Empty); 

        }

        private void ValidateDuplicates(User newUser, List<User> users)
        {
            if (users.Where(x => x.Email == newUser.Email).FirstOrDefault() != null)
                throw new ValidationExeption("Usuario con Email Duplicado");

            if (users.Where(x => x.Name == newUser.Name).FirstOrDefault() != null)
                throw new ValidationExeption("Usuario con Nombre Duplicado");

        }

        private void ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
                throw new ValidationExeption("Email no Valido");  
        }

        private async Task SaveUserList(List<User> users, string userTextPath)
        {
            await CleanFile(userTextPath);

            using (StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory() + userTextPath, false))
            {
                foreach (var user in users)
                {
                    await file.WriteLineAsync($@"{user.Name},{user.Email},{user.Phone},{user.Address},{user.UserType.ToString().Trim()},{user.Money}");
                }

                file.Close();
            }


        }

        private StreamReader ReadUsersFromFile(string userTextPath)
        {
            var path = Directory.GetCurrentDirectory() + userTextPath;

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);

            return reader;
        }
    }

}
 
