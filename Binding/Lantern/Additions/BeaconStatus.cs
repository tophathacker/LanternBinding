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

namespace MyriadMobile.Lantern
{
	public enum BeaconStatus
	{
		Off = BeaconService.BeaconStatusOff,
		Scanning = BeaconService.BeaconStatusScanning,
		FastScanning = BeaconService.BeaconStatusFastScanning,
		NotScanning = BeaconService.BeaconStatusNotScanning
	}
}