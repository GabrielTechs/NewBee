﻿using MonkeyCache.FileStore;
using NewBeeProject.Models;
using NewBeeProject.Services;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace NewBeeProject.ViewModels
{
    public class LoginPageViewModel: BaseViewModel
    {
        public DelegateCommand LoginCommand { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public DelegateCommand NavRegisterCommand { get; set; }
        public DelegateCommand NavForgottenPasswordCommand { get; set; }

        public LoginPageViewModel(INavigationService navigationService ,IAPIService APIservice) : base(navigationService)
        {

            LoginCommand = new DelegateCommand(async () =>
              {
                 
                  if(!string.IsNullOrEmpty(UserID) || !string.IsNullOrEmpty(Password))
                  {
                      Student LoggedStudent = await APIservice.CheckLogin(UserID, Password);
                      if (!LoggedStudent.Equals(null))
                      {
                          LoggedStudent.StudentCoursesList = await APIservice.GetSchedule(UserID);
                          Barrel.Current.Add<Student>("LoggedStudent", LoggedStudent, TimeSpan.FromMinutes(20));
                          await navigationService.NavigateAsync(new Uri($"/{NavConstants.HomeMasterDetail}/{NavConstants.Navigation}/{NavConstants.Home}", UriKind.Absolute));
                      }
                      
                      // TODO: Login Error
                  }


                  //TODO: Incorrect login error message
              });

            NavRegisterCommand = new DelegateCommand(async () =>
            {
               await navigationService.NavigateAsync($"{NavConstants.Navigation}/{NavConstants.Registration}");
            });
            NavForgottenPasswordCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync($"{NavConstants.Navigation}/{NavConstants.ForgottenPassword}");
            });
        }
    }
}
