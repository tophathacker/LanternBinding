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
using Java.Text;
using Java.Util;

namespace MyriadMobile.Lantern.Example
{
	public class BeaconAdapter : BaseAdapter
	{

		private List<Beacon> data;
		private Context context;
		private int layoutResourceId;

		public BeaconAdapter(Context context, int layoutResourceId, List<Beacon> data)
		{
			this.context = context;
			this.data = data;
			this.layoutResourceId = layoutResourceId;
		}

		public override int Count
		{
			get { return data.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return data[position];
		}

		public override long GetItemId(int position)
		{

			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup viewGroup)
		{

			ViewHolder holder;

			Beacon beacon = data[position];

			if (convertView == null)
			{
				LayoutInflater inflater = ((Activity)context).LayoutInflater;

				convertView = inflater.Inflate(layoutResourceId, viewGroup, false);
				holder = new ViewHolder();


				holder.tvMac = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_mac);
				holder.tvDistance = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_distance);
				holder.tvUuid = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_uuid);
				holder.tvMinor = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_minor);
				holder.tvMajor = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_major);
				holder.tvRssi = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_rssi);
				holder.tvExpiration = (TextView)convertView.FindViewById(Resource.Id.tv_beacon_expiration);
				holder.linearRoot = (LinearLayout)convertView.FindViewById(Resource.Id.lnr_beacon_root);

				convertView.Tag = holder;
			}
			else
			{
				holder = (ViewHolder)convertView.Tag;
			}

			Calendar expireTime = Calendar.Instance;

			expireTime.TimeInMillis = beacon.ExpirationTime;

			holder.tvMac.Text = string.Concat(context.GetString(Resource.String.mac), beacon.BluetoothAddress);
			holder.tvUuid.Text = string.Concat(context.GetString(Resource.String.uuid), beacon.Uuid);
			holder.tvDistance.Text = string.Concat(context.GetString(Resource.String.proximity) + Beacon.ProximityToString((int)beacon.Proximity));
			holder.tvMinor.Text = string.Concat(context.GetString(Resource.String.minor), beacon.Minor);
			holder.tvMajor.Text = string.Concat(context.GetString(Resource.String.major), beacon.Major);
			holder.tvRssi.Text = string.Concat(context.GetString(Resource.String.rssi), beacon.Rssi);
			holder.tvExpiration.Text = string.Format("{0}{1:hh':'mm a}", context.GetString(Resource.String.expiration), expireTime.Time);

			switch ((int)beacon.Proximity)
			{
				case Beacon.ProximityUnknown:
					holder.linearRoot.SetBackgroundColor(context.Resources.GetColor(Resource.Color.beacon_unkown));
					break;
				case Beacon.ProximityFar:
					holder.linearRoot.SetBackgroundColor(context.Resources.GetColor(Resource.Color.beacon_far));
					break;
				case Beacon.ProximityNear:
					holder.linearRoot.SetBackgroundColor(context.Resources.GetColor(Resource.Color.beacon_near));
					break;
				case Beacon.ProximityImmediate:
					holder.linearRoot.SetBackgroundColor(context.Resources.GetColor(Resource.Color.beacon_immediate));
					break;
			}

			return convertView;
		}

		private class ViewHolder : Java.Lang.Object
		{
			public LinearLayout linearRoot;
			public TextView tvMac;
			public TextView tvUuid;
			public TextView tvDistance;
			public TextView tvMinor;
			public TextView tvMajor;
			public TextView tvRssi;
			public TextView tvExpiration;
		}


	}
}