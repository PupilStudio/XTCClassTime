using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace XTCClassTime
{
    class ClassImageAdapter : BaseAdapter
    {
        List<ClassImage> classes;
        Context con;
        private LayoutInflater inflater;

        public ClassImageAdapter(Context context, List<ClassImage> cts)
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
            ClassImage ct = classes[position];
            View view;
            if (convertView == null)
            {
                view = LayoutInflater.From(con).Inflate(Resource.Layout.list_item_image, null);
            }
            else
            {
                view = convertView;//直接对convertView进行重用
            }
            ImageView classImg = view.FindViewById<ImageView>(Resource.Id.SubjectImage);
            TextView className = view.FindViewById<TextView>(Resource.Id.SubjectName);

            classImg.SetImageResource(DataController.GetClassImage(ct.DisplayName));
            className.Text = ct.DisplayName;

            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

    }
}