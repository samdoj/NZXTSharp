﻿using System;
using System.Collections.Generic;
using System.Text;

using NZXTSharp.Devices;
using NZXTSharp.Params;
using NZXTSharp.Exceptions;

// TOTEST
namespace NZXTSharp.Effects {

    /// <summary>
    /// Represents an RGB breathing effect.
    /// </summary>
    public class Breathing : IEffect {

        // Per Effect Fields
        private int _EffectByte = 0x07;
        private string _EffectName = "Breathing";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() { NZXTDeviceType.HuePlus };

        /// <inheritdoc/>
        public Color[] Colors;
        private Channel _Channel;
        private _03Param _Param1;
        private CISS _Param2;
        private int speed = 2;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public Channel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs a <see cref="Breathing"/> effect.
        /// </summary>
        /// <param name="Colors">A <see cref="Color"/> array of colors to display.</param>
        public Breathing(Color[] Colors) {
            this.Colors = Colors;
            ValidateParams();
        }

        /// <summary>
        /// Constructs a <see cref="Breathing"/> effect.
        /// </summary>
        /// <param name="Colors">A <see cref="Color"/> array of colors to display.</param>
        /// <param name="Speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public Breathing(Color[] Colors, int Speed) {
            this.Colors = Colors;
            this.speed = Speed;
            ValidateParams();
        }

        private void ValidateParams() {
            if (this.Colors.Length > 15) {
                throw new TooManyColorsProvidedException();
            }

            if (speed > 4 || speed < 0) {
                throw new InvalidEffectSpeedException();
            }
        }

        /// <inheritdoc/>
        public bool IsCompatibleWith(NZXTDeviceType Type)
        {
            return CompatibleWith.Contains(Type) ? true : false;
        }

        /// <inheritdoc/>
        public List<byte[]> BuildBytes(Channel Channel) {
            List<byte[]> outList = new List<byte[]>();
            for (int colorIndex = 0; colorIndex < Colors.Length; colorIndex++) {
                byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel, 0x07, _Param1, new CISS(colorIndex, this.speed) };
                byte[] final = SettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Colors[colorIndex]));
                outList.Add(final);
            }
            return outList;
        }
    }
}
