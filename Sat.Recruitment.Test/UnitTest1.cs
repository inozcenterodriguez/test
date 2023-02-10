using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Service.Dtos;
using Sat.Recruitment.Service.Services.Implementation;
using Sat.Recruitment.Service.Services.Interfaces;
using Xunit;

namespace Sat.Recruitment.Test
{
    // Prueba Hecha por Marlon Calles, Software developer. 
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        private IDataService fileDataService;
        private IUsersService usersService;


       
        public UnitTest1( )
        {
            this.fileDataService = Substitute.For<IDataService>(); ;
            this.usersService = Substitute.For<IUsersService>();
        }





        [Theory, AutoData]
        
        public async void WhenServiceResponseisCorrectfullControllerResponseis200(User user, List<User> users, string textPath )
        {
            //Arrange 

            var MyConfig = new MyConfig()
            {
                UserTextPath = textPath,
            };


            IOptions<MyConfig> appSettingsOptions = Options.Create(MyConfig);

            fileDataService.CreateUser(user, textPath).Returns(true);

            usersService = new UsersService(fileDataService);

            fileDataService.GetUsers(textPath).Returns(users);

            var userController = new RecController(usersService, appSettingsOptions);



            //Act
            var result = userController.CreateUser(user).Result;


            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
        }




        [Theory, AutoData]
        public async void WhenTryingtoCreateaDuplicateUserServiceThrowsAnException( User user )
        {

            //Arrange 

            user.Name = "Juan";

            user.Email = "Jdd@gmail.com"; 

            var host = await this.CreateHost(); 
           
            var client = host.GetTestClient();
             
            var content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "/Recruitment/users");

            message.Content = content; 


            // Act
            var response = await client.SendAsync(message);



            // Assert
            var responseString =  JsonConvert.DeserializeObject<HttpResponseC>(await response.Content.ReadAsStringAsync());
            Assert.Equal ("Usuario con Nombre Duplicado", responseString.title); 
            
        }




        [Theory, AutoData]
        public async void WhenTryingtoCreateaBadEmailUserServiceThrowsAnException(User user)
        {

            //Arrange 

            user.Name = "Jose";

            user.Email = "Jddgmail.com";

            var host = await this.CreateHost();

            var client = host.GetTestClient();

            var content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "/Recruitment/users");

            message.Content = content;


            // Act
            var response = await client.SendAsync(message);



            // Assert
            var responseString = JsonConvert.DeserializeObject<HttpResponseC>(await response.Content.ReadAsStringAsync());
            Assert.Equal("Email no Valido", responseString.title);

        }






        [Theory, AutoData]
        public async void WhenTryingtoCreateaIncompleteUserServiceThrowsAnException(UserIncomplete user)
        {

            //Arrange 

            

            user.Email = "Jddgmail.com";

            var host = await this.CreateHost();

            var client = host.GetTestClient();

            var content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "/Recruitment/users");

            message.Content = content;


            // Act
            var response = await client.SendAsync(message);



            // Assert
            var responseString = JsonConvert.DeserializeObject<HttpResponseC>(await response.Content.ReadAsStringAsync());
            Assert.Equal("One or more validation errors occurred.", responseString.title);
            Assert.Equal(400, responseString.status);
        }









        private async  Task<IHost> CreateHost()
        {

            var args = new List<string>().ToArray();

            var hostBuilder = new HostBuilder() 
           

                                
                               .ConfigureWebHost(webHost =>
                               { 
                                   webHost.UseTestServer();
                                   webHost.UseStartup<Sat.Recruitment.Api.Startup>();

                               }).ConfigureAppConfiguration((hostContext, configApp) =>
                               {
                                   configApp.AddJsonFile("appsettings.json", optional: true);
                                   configApp.AddJsonFile(
                                      $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                                      optional: true);
                                   configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                                   configApp.AddCommandLine(args);
                               })
                               
                               ;

            

            return await   hostBuilder.StartAsync();  
        }
    }
}
