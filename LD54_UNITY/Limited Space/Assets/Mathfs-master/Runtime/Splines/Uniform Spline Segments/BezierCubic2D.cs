// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)
// Do not manually edit - this file is generated by MathfsCodegen.cs

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Freya {

	/// <summary>An optimized uniform 2D Cubic bézier segment, with 4 control points</summary>
	[Serializable] public struct BezierCubic2D : IParamSplineSegment<Polynomial2D,Vector2Matrix4x1> {

		const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

		[SerializeField] Vector2Matrix4x1 pointMatrix;
		[NonSerialized] Polynomial2D curve;
		[NonSerialized] bool validCoefficients;

		/// <summary>Creates a uniform 2D Cubic bézier segment, from 4 control points</summary>
		/// <param name="p0">The starting point of the curve</param>
		/// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
		/// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
		/// <param name="p3">The end point of the curve</param>
		public BezierCubic2D( Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3 ) : this(new Vector2Matrix4x1(p0, p1, p2, p3)){}
		/// <summary>Creates a uniform 2D Cubic bézier segment, from 4 control points</summary>
		/// <param name="pointMatrix">The matrix containing the control points of this spline</param>
		public BezierCubic2D( Vector2Matrix4x1 pointMatrix ) => (this.pointMatrix,curve,validCoefficients) = (pointMatrix,default,false);

		public Polynomial2D Curve {
			get {
				if( validCoefficients )
					return curve; // no need to update
				validCoefficients = true;
				return curve = new Polynomial2D(
					P0,
					3*(-P0+P1),
					3*P0-6*P1+3*P2,
					-P0+3*P1-3*P2+P3
				);
			}
		}
		public Vector2Matrix4x1 PointMatrix {[MethodImpl( INLINE )] get => pointMatrix; [MethodImpl( INLINE )] set => _ = ( pointMatrix = value, validCoefficients = false ); }
		/// <summary>The starting point of the curve</summary>
		public Vector2 P0{ [MethodImpl( INLINE )] get => pointMatrix.m0; [MethodImpl( INLINE )] set => _ = ( pointMatrix.m0 = value, validCoefficients = false ); }
		/// <summary>The second control point of the curve, sometimes called the start tangent point</summary>
		public Vector2 P1{ [MethodImpl( INLINE )] get => pointMatrix.m1; [MethodImpl( INLINE )] set => _ = ( pointMatrix.m1 = value, validCoefficients = false ); }
		/// <summary>The third control point of the curve, sometimes called the end tangent point</summary>
		public Vector2 P2{ [MethodImpl( INLINE )] get => pointMatrix.m2; [MethodImpl( INLINE )] set => _ = ( pointMatrix.m2 = value, validCoefficients = false ); }
		/// <summary>The end point of the curve</summary>
		public Vector2 P3{ [MethodImpl( INLINE )] get => pointMatrix.m3; [MethodImpl( INLINE )] set => _ = ( pointMatrix.m3 = value, validCoefficients = false ); }
		/// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
		public Vector2 this[ int i ] {
			get => i switch { 0 => P0, 1 => P1, 2 => P2, 3 => P3, _ => throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" ) };
			set { switch( i ){ case 0: P0 = value; break; case 1: P1 = value; break; case 2: P2 = value; break; case 3: P3 = value; break; default: throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" ); }}
		}
		public static bool operator ==( BezierCubic2D a, BezierCubic2D b ) => a.pointMatrix == b.pointMatrix;
		public static bool operator !=( BezierCubic2D a, BezierCubic2D b ) => !( a == b );
		public bool Equals( BezierCubic2D other ) => P0.Equals( other.P0 ) && P1.Equals( other.P1 ) && P2.Equals( other.P2 ) && P3.Equals( other.P3 );
		public override bool Equals( object obj ) => obj is BezierCubic2D other && pointMatrix.Equals( other.pointMatrix );
		public override int GetHashCode() => pointMatrix.GetHashCode();
		public override string ToString() => $"({pointMatrix.m0}, {pointMatrix.m1}, {pointMatrix.m2}, {pointMatrix.m3})";

		/// <summary>Returns this spline segment in 3D, where z = 0</summary>
		/// <param name="curve2D">The 2D curve to cast to 3D</param>
		public static explicit operator BezierCubic3D( BezierCubic2D curve2D ) => new BezierCubic3D( curve2D.P0, curve2D.P1, curve2D.P2, curve2D.P3 );
		public static explicit operator HermiteCubic2D( BezierCubic2D s ) =>
			new HermiteCubic2D(
				s.P0,
				3*(-s.P0+s.P1),
				s.P3,
				3*(-s.P2+s.P3)
			);
		public static explicit operator CatRomCubic2D( BezierCubic2D s ) =>
			new CatRomCubic2D(
				6*s.P0-6*s.P1+s.P3,
				s.P0,
				s.P3,
				s.P0-6*s.P2+6*s.P3
			);
		public static explicit operator UBSCubic2D( BezierCubic2D s ) =>
			new UBSCubic2D(
				6*s.P0-7*s.P1+2*s.P2,
				2*s.P1-s.P2,
				-s.P1+2*s.P2,
				2*s.P1-7*s.P2+6*s.P3
			);
		/// <summary>Returns a linear blend between two bézier curves</summary>
		/// <param name="a">The first spline segment</param>
		/// <param name="b">The second spline segment</param>
		/// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
		public static BezierCubic2D Lerp( BezierCubic2D a, BezierCubic2D b, float t ) =>
			new(
				Vector2.LerpUnclamped( a.P0, b.P0, t ),
				Vector2.LerpUnclamped( a.P1, b.P1, t ),
				Vector2.LerpUnclamped( a.P2, b.P2, t ),
				Vector2.LerpUnclamped( a.P3, b.P3, t )
			);

		/// <summary>Returns a linear blend between two bézier curves, where the tangent directions are spherically interpolated</summary>
		/// <param name="a">The first spline segment</param>
		/// <param name="b">The second spline segment</param>
		/// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
		public static BezierCubic2D Slerp( BezierCubic2D a, BezierCubic2D b, float t ) {
			Vector2 P0 = Vector2.LerpUnclamped( a.P0, b.P0, t );
			Vector2 P3 = Vector2.LerpUnclamped( a.P3, b.P3, t );
			return new BezierCubic2D(
				P0,
				P0 + (Vector2)Vector3.SlerpUnclamped( a.P1 - a.P0, b.P1 - b.P0, t ),
				P3 + (Vector2)Vector3.SlerpUnclamped( a.P2 - a.P3, b.P2 - b.P3, t ),
				P3
			);
		}
		/// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
		/// <param name="t">The t-value to split at</param>
		public (BezierCubic2D pre, BezierCubic2D post) Split( float t ) {
			Vector2 a = new Vector2(
				P0.x + ( P1.x - P0.x ) * t,
				P0.y + ( P1.y - P0.y ) * t );
			Vector2 b = new Vector2(
				P1.x + ( P2.x - P1.x ) * t,
				P1.y + ( P2.y - P1.y ) * t );
			Vector2 c = new Vector2(
				P2.x + ( P3.x - P2.x ) * t,
				P2.y + ( P3.y - P2.y ) * t );
			Vector2 d = new Vector2(
				a.x + ( b.x - a.x ) * t,
				a.y + ( b.y - a.y ) * t );
			Vector2 e = new Vector2(
				b.x + ( c.x - b.x ) * t,
				b.y + ( c.y - b.y ) * t );
			Vector2 p = new Vector2(
				d.x + ( e.x - d.x ) * t,
				d.y + ( e.y - d.y ) * t );
			return ( new BezierCubic2D( P0, a, d, p ), new BezierCubic2D( p, e, c, P3 ) );
		}
	}
}
