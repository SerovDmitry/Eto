using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eto.Drawing
{
	/// <summary>
	/// Dash style for a <see cref="Pen"/>
	/// </summary>
	/// <seealso cref="Pen.DashStyle"/>
	/// <copyright>(c) 2012 by Curtis Wensley</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	public sealed class DashStyle : IEquatable<DashStyle>
	{
		readonly float[] dashes;
		float offset;

		/// <summary>
		/// Gets the dashes and gaps for this style
		/// </summary>
		/// <remarks>
		/// The values specified are the dash lengths and gap lengths in alternating order.
		/// The lengths are multiplied by the thickness of the pen.
		/// 
		/// For example, values of 2, 1 would have a dash of (2 * thickness) followed by a gap of (1 * thickness).
		/// </remarks>
		/// <value>The dashes to use for a pen</value>
		public float[] Dashes { get { return dashes; } }

		/// <summary>
		/// Gets the offset of the first dash
		/// </summary>
		/// <remarks>
		/// A value of 1 indicates that the first dash should start at the (1*thickness) of the pen.
		/// </remarks>
		/// <value>The offset of the first dash, in multiples of pen thickness</value>
		public float Offset { get { return offset; } }

		/// <summary>
		/// Gets a value indicating whether this dash style is solid
		/// </summary>
		/// <value><c>true</c> if this instance is solid; otherwise, <c>false</c>.</value>
		public bool IsSolid
		{
			get { return Dashes == null || Dashes.Length == 0; }
		}

		public static bool TryParse (string value, out DashStyle style)
		{
			if (string.IsNullOrEmpty (value)) {
				style = DashStyles.Solid;
				return true;
			}

			switch (value.ToLowerInvariant ()) {
			case "solid":
				style = DashStyles.Solid;
				return true;
			case "dash":
				style = DashStyles.Dash;
				return true;
			case "dot":
				style = DashStyles.Dot;
				return true;
			case "dashdot":
				style = DashStyles.DashDot;
				return true;
			case "dashdotdot":
				style = DashStyles.DashDotDot;
				return true;
			}

			var values = value.Split (',');
			if (values.Length == 0) {
				style = DashStyles.Solid;
				return true;
			}
			float offset;
			if (!float.TryParse (values [0], out offset))
				throw new ArgumentOutOfRangeException ("value", value);
			float [] dashes = null;
			if (values.Length > 1) {
				dashes = new float [values.Length - 1];
				for (int i = 0; i < dashes.Length; i++) {
					float dashValue;
					if (!float.TryParse (values [i + 1], out dashValue))
						throw new ArgumentOutOfRangeException ("value", value);
					dashes [i] = dashValue;
				}
			}

			style = new DashStyle (offset, dashes);
			return true;
		}

		public static DashStyle Parse (string value)
		{
			DashStyle style;
			if (TryParse (value, out style))
				return style;
			throw new ArgumentOutOfRangeException ("value", value, "Cannot convert value to a color");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Drawing.DashStyle"/> class.
		/// </summary>
		public DashStyle ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Drawing.DashStyle"/> class.
		/// </summary>
		/// <param name="offset">Offset of the first dash in the style</param>
		/// <param name="dashes">Dashes to use for the style.  See <see cref="Dashes"/></param>
		public DashStyle (float offset, params float[] dashes)
		{
			if (dashes != null && dashes.Any (r => r <= 0))
				throw new ArgumentOutOfRangeException ("dashes", dashes, "Each dash or gap must have a size greater than zero");
			this.offset = offset;
			this.dashes = dashes;
		}

		/// <summary>
		/// Compares two DashStyle objects for equality
		/// </summary>
		/// <param name="style1">First style to compare</param>
		/// <param name="style2">Second style to compare</param>
		public static bool operator == (DashStyle style1, DashStyle style2)
		{
			if ((object)style1 == null || (object)style2 == null)
				return object.ReferenceEquals (style1, style2);
			if (style1.Offset != style2.Offset)
				return false;
			if (style1.Dashes == null)
				return style2.Dashes == null;
			return style2.Dashes != null && style1.Dashes.SequenceEqual (style2.Dashes);
		}

		/// <summary>
		/// Compares two DashStyle objects for inequality
		/// </summary>
		/// <param name="style1">First style to compare</param>
		/// <param name="style2">Second style to compare</param>
		public static bool operator != (DashStyle style1, DashStyle style2)
		{
			return !(style1 == style2);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Eto.Drawing.DashStyle"/> is equal to the current <see cref="Eto.Drawing.DashStyle"/>.
		/// </summary>
		/// <param name="other">The <see cref="Eto.Drawing.DashStyle"/> to compare with the current <see cref="Eto.Drawing.DashStyle"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Eto.Drawing.DashStyle"/> is equal to the current
		/// <see cref="Eto.Drawing.DashStyle"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (DashStyle other)
		{
			return this == other;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Eto.Drawing.DashStyle"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Eto.Drawing.DashStyle"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Eto.Drawing.DashStyle"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			return obj is DashStyle && this == (DashStyle)obj;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Eto.Drawing.DashStyle"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Eto.Drawing.DashStyle"/>.</returns>
		public override string ToString()
		{
			if (object.ReferenceEquals(this, DashStyles.Dash))
				return "dash";
			else if (object.ReferenceEquals(this, DashStyles.Dot))
				return "dot";
			else if (object.ReferenceEquals(this, DashStyles.DashDot))
				return "dashdot";
			else if (object.ReferenceEquals(this, DashStyles.DashDotDot))
				return "dashdotdot";
			else if (object.ReferenceEquals(this, DashStyles.Solid))
				return "solid";
			else
				return string.Format ("{0},{1}", Offset, string.Join (",", Dashes.Select (r => r.ToString ())));
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Eto.Drawing.DashStyle"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			int hash = offset.GetHashCode ();
			if (dashes != null) {
				for (int i = 0; i < dashes.Length; i++)
					hash ^= dashes[i].GetHashCode ();
			}
			return hash;
		}
	}
}
