using System;
using System.Collections.Generic;
using System.Linq;
using Nac.Geometory;

namespace Nac.Helpers
{
	public static class Helper
	{
		public static void DisposeRange( IEnumerable<IDisposable> array )
		{
			foreach( var item in array )
				item.Dispose();
		}
		public static void DisposeRange( IEnumerator<IDisposable> array )
		{
			while( array.MoveNext() )
				array.Current.Dispose();
		}

		public static int GetEnumLength( this Type enumType )
		{
			if( !enumType.IsEnum )
				throw new ArgumentException( "引数が列挙体の型情報ではありません。" );
			return Enum.GetValues( enumType ).Length;
		}

		public static void Swap<Type>( ref Type obj1, ref Type obj2 )
		{
			Type temp = obj1;
			obj1 = obj2;
			obj2 = obj1;
		}
		public static T GetRandom<T>( this IEnumerable<T> collection, Random machine = null )
		{
			if( collection.Count() == 0 ) return default( T );
			machine = machine ?? new Random();
			return collection.ElementAt( machine.Next( collection.Count() ) );
		}

		public static void TryInvoke( Action action )
		{
			if( action != null )
				action();
		}
		public static void TryInvoke<T1>( Action<T1> action, T1 arg1 )
		{
			if( action != null )
				action( arg1 );
		}
		public static void TryInvoke<T1, T2>( Action<T1,T2> action, T1 arg1, T2 arg2 )
		{
			if( action != null )
				action( arg1, arg2 );
		}
		public static void TryInvoke<TResult>( Func<TResult> function, ref TResult result )
		{
			if( function != null )
				result = function();
		}
		public static void TryInvoke<TResult, T1>( Func<T1, TResult> function, T1 arg1, ref TResult result )
		{
			if( function != null )
				result = function( arg1 );
		}
		public static void TryInvoke<TResult, T1, T2>( Func<T1, T2, TResult> function, T1 arg1, T2 arg2, ref TResult result )
		{
			if( function != null )
				result = function( arg1, arg2 );
		}
	}
}
