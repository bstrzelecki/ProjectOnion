namespace ProjectOnion
{
	class TimeUnit
	{
		float miliSecs;
		/// <summary>
		/// Standarized time unit for MBBS games.
		/// </summary>
		/// <param name="miliSecs">value in houndreds of miliseconds</param>
		public TimeUnit(float miliSecs)
		{
			this.miliSecs = miliSecs * 100f;
		}
		public static implicit operator float(TimeUnit tu)
		{
			return tu.miliSecs;
		}
	}
}
