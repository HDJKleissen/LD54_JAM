// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)
// Do not manually edit - this file is generated by MathfsCodegen.cs

using System;
namespace Freya {
	/// <summary>A 3x1 column matrix with float values</summary>
	[Serializable] public struct Matrix3x1 {
		public float m0, m1, m2;
		public Matrix3x1(float m0, float m1, float m2) => (this.m0, this.m1, this.m2) = (m0, m1, m2);
		public float this[int row] {
			get => row switch{0 => m0, 1 => m1, 2 => m2, _ => throw new IndexOutOfRangeException( $"Matrix row index has to be from 0 to 2, got: {row}" )};
			set {
				switch(row) {
					case 0: m0 = value; break; case 1: m1 = value; break; case 2: m2 = value; break;
					default: throw new IndexOutOfRangeException( $"Matrix row index has to be from 0 to 2, got: {row}" );
				}
			}
		}
		/// <summary>Linearly interpolates between two matrices, based on a value <c>t</c></summary>
		/// <param name="t">The value to blend by</param>
		public static Matrix3x1 Lerp( Matrix3x1 a, Matrix3x1 b, float t ) => new Matrix3x1(Mathfs.Lerp( a.m0, b.m0, t ), Mathfs.Lerp( a.m1, b.m1, t ), Mathfs.Lerp( a.m2, b.m2, t ));
		public static bool operator ==( Matrix3x1 a, Matrix3x1 b ) => a.m0 == b.m0 && a.m1 == b.m1 && a.m2 == b.m2;
		public static bool operator !=( Matrix3x1 a, Matrix3x1 b ) => !( a == b );
		public bool Equals( Matrix3x1 other ) => m0.Equals( other.m0 ) && m1.Equals( other.m1 ) && m2.Equals( other.m2 );
		public override bool Equals( object obj ) => obj is Matrix3x1 other && Equals( other );
		public override int GetHashCode() => HashCode.Combine( m0, m1, m2 );
		public override string ToString() => $"[{m0}]\n[{m1}]\n[{m2}]";
	}
}
