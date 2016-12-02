namespace PadOS{
	public struct Vector2 : System.Collections.Generic.IEnumerable<double> {
		public Vector2(double x) : this(x,0){}
		public Vector2(double x, double y)
			: this() {
			X = x;
			Y = y;
		}
		public double X;
		public double Y;

		public void Add(double x) {
			X = x;
		}
		public void Add(double x, double y) {
			X = x;
			Y = y;
		}

		public static Vector2 operator +(Vector2 a, double b) {
			return new Vector2(
				a.X + b,
				a.Y + b
			);
		}
		public static Vector2 operator -(Vector2 a, double b) {
			return a + -b;
		}

		public static Vector2 operator -(Vector2 a) {
			return a*-1;
		}

		public static Vector2 operator *(Vector2 a, double b) {
			return new Vector2(
				a.X * b,
				a.Y * b
			);
		}
		public static Vector2 operator /(Vector2 a, double b) {
			return a * (1 / b);
		}
		public static Vector2 operator +(double a, Vector2 b) {
			return b + a;
		}
		public static Vector2 operator -(double a, Vector2 b) {
			return b - a;
		}
		public static Vector2 operator *(double a, Vector2 b) {
			return b * a;
		}
		public static Vector2 operator /(double a, Vector2 b) {
			return new Vector2(
				a / b.X,
				a / b.Y
			);
		}

		public static Vector2 operator +(Vector2 a, Vector2 b) {
			return new Vector2(
				a.X + b.X,
				a.Y + b.Y
			);
		}
		public static Vector2 operator -(Vector2 a, Vector2 b) {
			return new Vector2(
				a.X - b.X,
				a.Y - b.Y
			);
		}
		public static Vector2 operator *(Vector2 a, Vector2 b) {
			return new Vector2(
				a.X * b.X,
				a.Y * b.Y
			);
		}
		public static Vector2 operator /(Vector2 a, Vector2 b) {
			return new Vector2(
				a.X / b.X,
				a.Y / b.Y
			);
		}

		public static bool operator ==(Vector2 a, Vector2 b){
			return
				a.X == b.X &&
				a.Y == b.Y;
		}

		public static bool operator !=(Vector2 a, Vector2 b){
			return !(a == b);
		}

		public bool Equals(Vector2 other) {
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector2 && Equals((Vector2)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public override string ToString() {
			return "X: " + X + ", Y: " + Y;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return ((System.Collections.Generic.IEnumerable<double>) this).GetEnumerator();
		}

		System.Collections.Generic.IEnumerator<double> System.Collections.Generic.IEnumerable<double>.GetEnumerator(){
			yield return X;
			yield return Y;
		}
	}
}