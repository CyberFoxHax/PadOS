using System;

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

		public double GetAngle(){
			return Math.Atan2(X, Y);
		}

		public double GetLength(){
			return Math.Sqrt(X*X + Y*Y);
		}

		public double GetSquared(){
			return X*X + Y*Y;
		}

		public Vector2 GetNormalized(){
			if (X == 0 && Y == 0) return this;
			if (X == 0) return new Vector2(0, Y > 0 ? 1 : -1);
			if (Y == 0) return new Vector2(X > 0 ? 1 : -1, 0);

			return this / (Math.Max(Math.Abs(X), Math.Abs(Y)) / GetLength());
		}

		public static Vector2 operator +(Vector2 a, double b) => new Vector2(
			a.X + b,
			a.Y + b
		);
		public static Vector2 operator -(Vector2 a, double b) => a + -b;
		public static Vector2 operator -(Vector2 a) => a*-1;
		public static Vector2 operator *(Vector2 a, double b) => new Vector2(
			a.X * b,
			a.Y * b
		);
		public static Vector2 operator /(Vector2 a, double b) => a * (1 / b);
		public static Vector2 operator +(double a, Vector2 b) => b + a;
		public static Vector2 operator -(double a, Vector2 b) => b - a;
		public static Vector2 operator *(double a, Vector2 b) => b * a;
		public static Vector2 operator /(double a, Vector2 b) => new Vector2(
			a / b.X,
			a / b.Y
		);
		public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(
			a.X + b.X,
			a.Y + b.Y
		);
		public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(
			a.X - b.X,
			a.Y - b.Y
		);
		public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(
			a.X * b.X,
			a.Y * b.Y
		);
		public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(
			a.X / b.X,
			a.Y / b.Y
		);

		public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X &&
		                                                        a.Y == b.Y;

		public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

		public bool Equals(Vector2 other) => X.Equals(other.X) && Y.Equals(other.Y);

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector2 && Equals((Vector2)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public override string ToString() => "X: " + X + ", Y: " + Y;

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => 
			((System.Collections.Generic.IEnumerable<double>) this).GetEnumerator();

		System.Collections.Generic.IEnumerator<double> System.Collections.Generic.IEnumerable<double>.GetEnumerator(){
			yield return X;
			yield return Y;
		}
	}
}