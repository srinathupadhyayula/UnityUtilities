using System;
using UnityEngine;
using Utilities.Types;

namespace Utilities.Extensions.Math
{
	/// <summary>
	/// Math extensions
	/// </summary>
	/// /// <remarks>
	/// <para>These set of Extensions are from<b cref="https://github.com/DapperDino">Andrew Rumak</b>'s
	/// <see cref="https://github.com/Deadcows/MyBox">My-Box</see> repository.</para>
	/// </remarks>
    public static partial class MathExtensions
    {
        public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);
		
		public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
		public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);
		
		/// <summary>
		/// Snap to grid of "round" size
		/// </summary>
		public static float Snap(this float val, float round) => round * Mathf.Round(val / round);

		/// <summary>
		/// Returns the sign 1/-1 evaluated at the given value.
		/// </summary>
		public static int Sign(IComparable x) => x.CompareTo(0);
		
		/// <summary>
		/// Shortcut for Mathf.Approximately
		/// </summary>
		public static bool Approximately(this float value, float compare) => Mathf.Approximately(value, compare);
		
		
		/// <summary>
		/// Value is in [0, 1) range.
		/// </summary>
		public static bool InRange01(this float value) => InRange(value, 0, 1);

		/// <summary>
		/// Value is in [closedLeft, openRight) range.
		/// </summary>
		public static bool InRange<T>(this T value, T closedLeft, T openRight) where T : IComparable =>
			value.CompareTo(closedLeft) >= 0 && value.CompareTo(openRight) < 0;

		/// <summary>
		/// Value is in a RangedFloat.
		/// </summary>
		public static bool InRange(this float value, RangedFloat range) => value.InRange(range.Min, range.Max);

		/// <summary>
		/// Value is in a RangedInt.
		/// </summary>
		public static bool InRange(this int value, RangedInt range) => value.InRange(range.Min, range.Max);

		/// <summary>
		/// Value is in [closedLeft, closedRight] range, max-inclusive.
		/// </summary>
		public static bool InRangeInclusive<T>(this T value, T closedLeft, T closedRight) where T : IComparable =>
			value.CompareTo(closedLeft) >= 0 && value.CompareTo(closedRight) <= 0;

		/// <summary>
		/// Value is in a RangedFloat, max-inclusive.
		/// </summary>
		public static bool InRangeInclusive(this float value, RangedFloat range) =>
			value.InRangeInclusive(range.Min, range.Max);

		/// <summary>
		/// Value is in a RangedInt, max-inclusive.
		/// </summary>
		public static bool InRangeInclusive(this int value, RangedInt range) =>
			value.InRangeInclusive(range.Min, range.Max);
	}
}