using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.IO;

namespace MaterGame
{
	public static class StorageHelper
	{
		private static StorageContainer GetContainer( string directory, string fileName )
		{
			StorageDevice device = null;
			var a = StorageDevice.BeginShowSelector( null, null );
			a.AsyncWaitHandle.WaitOne();
			device = StorageDevice.EndShowSelector( a );
			a.AsyncWaitHandle.Close();

			StorageContainer container = null;
			var b = device.BeginOpenContainer( directory, null, null );
			b.AsyncWaitHandle.WaitOne();
			container = device.EndOpenContainer( b );
			b.AsyncWaitHandle.Close();

			return container;
		}

		public static void SaveData<T>( string directory, string fileName, T data )
		{
			var container = GetContainer( directory, fileName );

			if( container.FileExists( fileName ) )
				container.DeleteFile( fileName );

			var stream = container.CreateFile( fileName );
			var xml = new XmlSerializer( typeof( T ) );
			xml.Serialize( stream, data );
			stream.Close();
			container.Dispose();
		}
		public static T LoadData<T>( string directory, string fileName )
		{
			var container = GetContainer( directory, fileName );

			if( !container.FileExists( fileName ) )
				return default( T );

			var stream = container.OpenFile( fileName, FileMode.Open );
			var xml = new XmlSerializer( typeof( T ) );
			T data = (T)xml.Deserialize( stream );
			stream.Close();
			container.Dispose();

			return data;
		}
	}
}
