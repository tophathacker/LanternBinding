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
	public class BeaconReceiver : BroadcastReceiver
	{
		public event EventHandler<BeaconEventArgs> ActionReceived;

		public event EventHandler<BeaconEventArgs> BeaconExpired;

		private Context androidContext;
		private IntentFilter intentFilter;

		public BeaconReceiver(Context context)
		{
			androidContext = context;

			intentFilter = new IntentFilter();
			intentFilter.AddAction(BeaconService.BeaconDetectedReceiverAction);
			intentFilter.AddAction(BeaconService.BeaconExpirationReceiverAction);
			androidContext.RegisterReceiver(this, intentFilter);
		}

		public override void OnReceive(Context context, Intent intent)
		{
			Bundle extras = intent.Extras;
			if (extras != null)
			{
				Beacon beacon;
				if (intent.Action.Equals(BeaconService.BeaconDetectedReceiverAction))
				{
					beacon = extras.GetParcelable(BeaconService.BeaconReceiverExtra) as Beacon;
					OnActionReceived(new BeaconEventArgs { Beacon = beacon });
				}
				else if (intent.Action.Equals(BeaconService.BeaconExpirationReceiverAction))
				{
					beacon = extras.GetParcelable(BeaconService.BeaconReceiverExtra) as Beacon;
					OnBeaconExpiring(new BeaconEventArgs { Beacon = beacon });
				}
			}
		}

		public virtual void OnActionReceived(BeaconEventArgs beaconEventArgs)
		{
			if (ActionReceived != null)
			{
				ActionReceived.Invoke(this, beaconEventArgs);
			}
		}

		public virtual void OnBeaconExpiring(BeaconEventArgs beaconEventArgs)
		{
			if (BeaconExpired != null)
			{
				BeaconExpired.Invoke(this, beaconEventArgs);
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