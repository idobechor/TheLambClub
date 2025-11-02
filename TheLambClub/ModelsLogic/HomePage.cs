using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;
using TheLambClub.Views;

namespace TheLambClub.ModelsLogic
{
    internal class HomePage:HomePageModel
    {
        public override void MoveToGamePage()
        {
            if (Application.Current != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current.MainPage = new RuningGameView();
                });
            }
        }
    
        public override void ShowNumericPromptCasting(object obj)
        {
            ShowNumericPrompt(obj);
        }
        private async void ShowNumericPrompt(object obj)
        {
            string result = await Application.Current!.MainPage!.DisplayPromptAsync(Strings.customRoomCodeTitleTxt,Strings.customRoomCodeTxt,  maxLength: 6, keyboard: Keyboard.Numeric);
        }
     
        public override void ShowInstructionsPrompt(object obj)
        {
            Application.Current!.MainPage!.DisplayAlert(Strings.InsructionsTxtTitle, Strings.InsructionsTxt, Strings.Ok);
        }
    }    
}
