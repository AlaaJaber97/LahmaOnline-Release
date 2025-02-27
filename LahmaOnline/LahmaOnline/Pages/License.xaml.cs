﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LahmaOnline.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class License : ContentPage
    {
        public Property.LicenseProperty LicenseProperty { get; set; } = new Property.LicenseProperty();
        public LayoutOptions HorizontalOptionsArrow => !AppStatics.IsRTL ? LayoutOptions.Start : LayoutOptions.End;

        public License()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<Interface.IStatusBarStyleManager>().SetLightTheme();
            LoadInfo();
        }

        private async void LoadInfo()
        {
            try
            {
                var modelInfo = new BLL.M.Mobile.Localization
                {
                    LanguageId = AppStatics.Language,
                    ResourceName = "mob.license"
                };

                var responseInfo = await new Services.HttpExtension<BLL.M.Mobile.Localization>().Post("Localization/GetLocalizationByLanguageIdandResourceName", modelInfo);
                LicenseProperty.TextPage = responseInfo?.ResourceValue;
            }
            catch (Exception ex)
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    await App._nav.DisplayAlert(MultiLanguage.MLResource.InternetConnectionTitle, MultiLanguage.MLResource.InternetConnectionMessage, MultiLanguage.MLResource.Ok);
                else
#if DEBUG
                    await App._nav.DisplayAlert(MultiLanguage.MLResource.Error, ex.ToString(), MultiLanguage.MLResource.Ok);
#elif RELEASE
                    await App._nav.DisplayAlert(MultiLanguage.MLResource.Error, MultiLanguage.MLResource.FailedMessage , MultiLanguage.MLResource.Ok);
#endif
            }
        }

        private void BackPage(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}