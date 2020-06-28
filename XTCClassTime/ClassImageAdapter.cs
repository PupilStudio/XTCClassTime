using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XTCClassTime
{
    class ClassImageAdapter : BaseAdapter
    {
        List<ClassTime> classes;
        Context con;
        private LayoutInflater inflater;

        private string FmtInt(int x)
        {
            string s = x.ToString();
            if (s.Length == 1)
            {
                s = '0' + s;
            }
            return s;
        }
        public ClassImageAdapter(Context context, List<ClassTime> cts)
        {
            con = context;
            classes = cts;
            inflater = LayoutInflater.From(context);
        }

        public override int Count
        {
            get
            {
                return classes.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return classes[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ClassTime ct = classes[position];
            View view;
            if (convertView == null)
            {
                view = LayoutInflater.From(con).Inflate(Resource.Layout.list_item_class, null);
            }
            else
            {
                view = convertView;//直接对convertView进行重用
            }
            ImageView classImg = view.FindViewById<ImageView>(Resource.Id.ClassImage);
            TextView className = view.FindViewById<TextView>(Resource.Id.ClassName);
            TextView classTime = view.FindViewById<TextView>(Resource.Id.ClassTime);

            classImg.SetImageResource(DataController.GetClassImage(ct.ClassName));
            className.Text = ct.ClassName;
            classTime.Text = FmtInt(ct.BeginHour) + ":" + FmtInt(ct.BeginMinute) + " - "
                + FmtInt(ct.EndHour) + ":" + FmtInt(ct.EndMinute);

            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

    }
}