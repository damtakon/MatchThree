using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MatchThree.Core;
using MatchThree.Core.Enum;
using Microsoft.Xna.Framework;

namespace MatchThree.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden |
                               ConfigChanges.ScreenSize
    )]
    public class Activity : AndroidGameActivity
    {
        private MainGame _game;
        private View _view;
        private SystemUiFlags _flags;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _game = new MainGame(PlatformEnum.Android, autoFacInit: AutoFacAndroid.Register);
            _view = _game.Services.GetService(typeof(View)) as View;
            _flags = SystemUiFlags.Fullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.Immersive |
                     SystemUiFlags.ImmersiveSticky | SystemUiFlags.LayoutHideNavigation | SystemUiFlags.LowProfile |
                     SystemUiFlags.Visible;

            Immersive = true;
            SetContentView(_view);

            _view.SystemUiVisibility = (StatusBarVisibility) _flags;
            _view.SystemUiVisibilityChange += ViewOnSystemUiVisibilityChange;
            _game.Run();
        }

        private void ViewOnSystemUiVisibilityChange(object sender, View.SystemUiVisibilityChangeEventArgs e)
        {
            if (sender is View targetView && targetView.SystemUiVisibility != (StatusBarVisibility)_flags)
            {
                targetView.SystemUiVisibility = (StatusBarVisibility)_flags;
            }
        }
    }
}