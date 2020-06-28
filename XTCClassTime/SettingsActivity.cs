using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Lang.Reflect;
using Android.Gestures;
using Android.Views;
using Android.Content;

namespace XTCClassTime
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : AppCompatActivity
    {
        string[] settingItems = new string[] { "查看课程表", "管理科目"};
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);
            //RequestWindowFeature(WindowFeatures.NoTitle);
            //ActionBar.Hide();
            SupportActionBar.Hide();

            ArrayAdapter<string> itemsAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_simple_xtc_layout, settingItems);
            (FindViewById<ListView>(Resource.Id.SettingsList)).Adapter = itemsAdapter;
            (FindViewById<ListView>(Resource.Id.SettingsList)).ItemClick += (sender, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        var intent = new Intent(this, typeof(WeekPickerActivity));
                        StartActivity(intent);
                        break;
                }
            };
            // Create your application here
        }
    }
}