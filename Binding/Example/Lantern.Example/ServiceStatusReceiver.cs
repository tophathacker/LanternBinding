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

namespace MyriadMobile.Lantern.Example
{
	public class ServiceStatusReceiver : BroadcastReceiver
	{
		public string ReceiverActionName
		{
			get { return BeaconService.BeaconServiceStatusAction; }
		}

		public event EventHandler<BeaconStatusEventArgs> StatusReceived;

		private Context androidContext;
		private IntentFilter intentFilter;

		public ServiceStatusReceiver(Context context)
		{
			androidContext = context;

			intentFilter = new IntentFilter();
			intentFilter.AddAction(ReceiverActionName);
			androidContext.RegisterReceiver(this, intentFilter);
		}

		public override void OnReceive(Context context, Intent intent)
		{
			Bundle extras = intent.Extras;
			if (extras != null)
			{
				if (intent.Action.Equals(ReceiverActionName))
				{
					BeaconStatus status = (BeaconStatus)extras.GetInt(BeaconService.BeaconServiceStatusChangeExtra);
					OnStatusReceived(new BeaconStatusEventArgs { Status = status });
				}
			}
		}

		public virtual void OnStatusReceived(BeaconStatusEventArgs statusEventArgs)
		{
			if (StatusReceived != null)
			{
				StatusReceived.Invoke(this, statusEventArgs);
			}
		}

		protected override void Dispose(bool disposing)
		{
			androidContext.UnregisterReceiver(this);
			androidContext = null;

			intentFilter.Dispose();
			intentFilter = null;

			base.Dispose(disposing);
		}
	}
}