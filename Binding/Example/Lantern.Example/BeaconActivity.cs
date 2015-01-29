using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace MyriadMobile.Lantern.Example
{
	[IntentFilter(new[] {
		"com.myriadmobile.library.lantern.beacon_detected_receiver_action",
		"com.myriadmobile.library.lantern.beacon_expiration_receiver_action",
		"com.myriadmobile.library.lantern.beacon_service_status_action" })]
	[Activity(Label = "Lantern Example", MainLauncher = true, Icon = "@drawable/icon")]
	public class BeaconActivity : Activity
	{
		private List<Beacon> beacons;
		private BeaconAdapter adapter;
		private ListView listView;
		private TextView scanningStatus;
		private Switch scanToggle;

		private BeaconReceiver beaconReceiver;
		private ServiceStatusReceiver serviceStatusReceiver;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.ActivityMain);

			beacons = new List<Beacon>();
			listView = FindViewById<ListView>(Resource.Id.lv_beacons);

			adapter = new BeaconAdapter(this, Resource.Layout.BeaconItem, beacons);

			listView.Adapter = adapter;

			scanningStatus = FindViewById<TextView>(Resource.Id.tv_status);
			scanToggle = FindViewById<Switch>(Resource.Id.swtScan);
			scanToggle.CheckedChange += scanToggle_CheckedChange;

			beaconReceiver = new BeaconReceiver(this);
			serviceStatusReceiver = new ServiceStatusReceiver(this);

			beaconReceiver.ActionReceived += beaconReceiver_ActionReceived;
			beaconReceiver.BeaconExpired += beaconExpirationReceiver_ActionReceived;
			serviceStatusReceiver.StatusReceived += serviceStatusReceiver_StatusReceived;

			BeaconServiceController.StartBeaconService(this, 20000, 60000, 7000, 5000, null);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			BeaconServiceController.StopBeaconService(this);

			beaconReceiver.ActionReceived -= beaconReceiver_ActionReceived;
			beaconReceiver.BeaconExpired -= beaconExpirationReceiver_ActionReceived;
			serviceStatusReceiver.StatusReceived -= serviceStatusReceiver_StatusReceived;

			beaconReceiver.Dispose();
			serviceStatusReceiver.Dispose();
		}

		private void scanToggle_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			if (e.IsChecked)
			{
				// Start the service.
				BeaconServiceController.StartBeaconService(this, 20000, 60000, 5000, 5000, null);
			}
			else
			{
				// Stop the service.
				BeaconServiceController.StopBeaconService(this);
			}
		}

		private void beaconReceiver_ActionReceived(object sender, BeaconEventArgs e)
		{
			if (!beacons.Contains(e.Beacon))
			{
				beacons.Add(e.Beacon);
				adapter.NotifyDataSetChanged();
			}
		}

		private void serviceStatusReceiver_StatusReceived(object sender, BeaconStatusEventArgs e)
		{
			int statusStringId = Resource.String.off;

			switch (e.Status)
			{
				case BeaconStatus.FastScanning:
					statusStringId = Resource.String.fast_scanning;
					break;
				case BeaconStatus.NotScanning:
					statusStringId = Resource.String.not_scanning;
					break;
				case BeaconStatus.Scanning:
					statusStringId = Resource.String.scanning;
					break;
				case BeaconStatus.Off:
				default:
					statusStringId = Resource.String.off;
					break;
			}

			scanningStatus.Text = Resources.GetString(statusStringId);
		}

		private void beaconExpirationReceiver_ActionReceived(object sender, BeaconEventArgs e)
		{
			if (beacons.Contains(e.Beacon))
			{
				beacons.Remove(e.Beacon);
				adapter.NotifyDataSetChanged();
			}
		}

	}
}

