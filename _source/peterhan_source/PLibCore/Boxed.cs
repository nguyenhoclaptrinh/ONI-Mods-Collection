using System;

namespace PeterHan.PLib.Core {
	/// <summary>
	/// A wrapper class for value types to avoid double-boxing when triggering events.
	/// </summary>
	/// <typeparam name="T">The value type to box.</typeparam>
	public sealed class Boxed<T> where T : struct {
		/// <summary>
		/// The boxed value.
		/// </summary>
		public readonly T value;

		public Boxed(T value) {
			this.value = value;
		}

		/// <summary>
		/// Gets a boxed version of the value.
		/// </summary>
		/// <param name="value">The value to box.</param>
		/// <returns>A boxed wrapper for the value.</returns>
		public static Boxed<T> Get(T value) {
			return new Boxed<T>(value);
		}

		public override string ToString() {
			return value.ToString();
		}
	}
}
