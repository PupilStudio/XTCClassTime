using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XTCClassTime
{
    class ForbidReceiver : BroadcastReceiver
    {
        private const string 
            ACTION_CLASS_MODE = "com.xtc.setting.action.CLASS.ACTION", // 上课禁用
            ACTION_POWER_SAVING = "xtc.setting.action.POWER_SAVE_CHANGE", // 省电模式
            ACTION_KILL_APP = "android.intent.action.KILL_APP", // 高温
            ACTION_MIGRATION_KILL_APP = "android.intent.action.MIGRATION.KILL_APP", // 数据迁移
            ACTION_WATCH_LOSS = "com.xtc.setting.WATCH.LOSS", // 手表挂失
            ACTION_LONG_BATTERY_LIFE_CHANGE = "com.xtc.i3launcher.action.LONG_BATTERY_LIFE_CHANGE", // 未注释

            EXTRA_STATE = "com.xtc.setting.extra.STATE", // Extra State
            EXTRA_IS_LONG_BATTERY_LIFE = "com.xtc.i3launcher.extra.IS_LONG_BATTERY_LIFE";

        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            if (action == null)
            {
                return;
            }

            // 处理禁用
            switch (action)
            {
                case ACTION_CLASS_MODE:
                    bool isClassMode = intent.GetBooleanExtra(EXTRA_STATE, false);
                    if (isClassMode)
                    {
                        StopSomething();
                    }
                    break;
                case ACTION_POWER_SAVING:
                    bool isPowerSaving = intent.GetBooleanExtra("power_save_mode", false);
                    if (isPowerSaving)
                    {
                        StopSomething();
                    }
                    break;
                case ACTION_KILL_APP:
                case ACTION_MIGRATION_KILL_APP:
                    StopSomething();
                    break;
                case ACTION_WATCH_LOSS:
                    bool isWatchLoss = intent.GetBooleanExtra(EXTRA_STATE, false);
                    if (isWatchLoss)
                    {
                        StopSomething();
                    }
                    break;
                case ACTION_LONG_BATTERY_LIFE_CHANGE:
                    bool boolExtra = intent.GetBooleanExtra(EXTRA_IS_LONG_BATTERY_LIFE, false);
                    if (boolExtra)
                    {
                        StopSomething();
                    }
                    break;
                case Intent.ActionBatteryChanged:
                    int plugged = intent.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                    if (plugged == (int)Android.OS.BatteryPlugged.Ac
                        || plugged == (int)Android.OS.BatteryPlugged.Usb
                        || plugged == (int)Android.OS.BatteryPlugged.Wireless)
                    {
                        StopSomething();
                    }
                    break;
            }
        }

        private void StopSomething()
        {
            int pid = Android.OS.Process.MyPid();
            Android.OS.Process.KillProcess(pid);
        }

        public static void Register(Context context)
        {
            IntentFilter filter = new IntentFilter();
            filter.AddAction(ACTION_CLASS_MODE);
            filter.AddAction(ACTION_POWER_SAVING);
            filter.AddAction(ACTION_KILL_APP);
            filter.AddAction(ACTION_MIGRATION_KILL_APP);
            filter.AddAction(ACTION_WATCH_LOSS);
            context.RegisterReceiver(new ForbidReceiver(), filter);
        }
    }
}