﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioLib;
using Newtonsoft.Json;

namespace ImpulseHd
{
	public class ImpulsePreset
	{
		[JsonIgnore]
		private double samplerate;

		public ImpulsePreset()
		{
			PresetVersion = 1000;
			Samplerate = 0.3333333;
			ImpulseLength = 0.5;
			WindowLength = 0.0;
			WindowMethod = 0.7;
			ImpulseConfig = new ImpulseConfig[0];
		}

		public ImpulseConfig[] ImpulseConfig { get; set; }

		public int PresetVersion { get; set; }

		public double Samplerate
		{
			get { return samplerate; }
			set
			{
				samplerate = value;
				if (ImpulseConfig == null)
					return;

				foreach (var ic in ImpulseConfig)
				{
					ic.Samplerate = SamplerateTransformed;
				}
			}
		}

		public double ImpulseLength { get; set; }
		public double WindowMethod { get; set; }
		public double WindowLength { get; set; }
		
		public int SamplerateTransformed
		{
			get
			{
				if (Samplerate < 0.25)
					return 44100;
				else if (Samplerate < 0.5)
					return 48000;
				else if (Samplerate < 0.75)
					return 44100 * 2;
				else
					return 96000;
			}
		}

		public int ImpulseLengthTransformed
		{
			get
			{
				var iVal = (int)((ImpulseLength - 0.0001) * 5);
				if (iVal == 0)
					return 256;
				else if (iVal == 1)
					return 512;
				else if (iVal == 2)
					return 1024;
				else if (iVal == 3)
					return 2048;
				else
					return 4096;
			}
		}

		public WindowMethod WindowMethodTransformed
		{
			get
			{
				if (WindowMethod < 0.25)
					return ImpulseHd.WindowMethod.Truncate;
				if (WindowMethod < 0.5)
					return ImpulseHd.WindowMethod.Linear;
				if (WindowMethod < 0.75)
					return ImpulseHd.WindowMethod.Logarithmic;
				else
					return ImpulseHd.WindowMethod.Cosine;
			}
		}
		public double WindowLengthTransformed => ValueTables.Get(WindowLength, ValueTables.Response2Oct) * 0.5;
	}
}
