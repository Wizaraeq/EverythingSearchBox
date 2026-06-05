using System;
using System.Collections.Generic;
using System.Text;
using QTPlugin.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace QTFileTools
{
	internal class FileOps
	{
		private static Guid IID_IDataObject = new Guid( "{0000010e-0000-0000-C000-000000000046}" );
		private static readonly int NOERROR = 0;


		internal static bool CopyCut( IShellBrowser shellBrowser, bool fCopy )
		{
			IShellView shellView = null;
			object dataObject = null;

			byte bEffect = fCopy ? (byte)5 : (byte)2;

			try
			{
				if( NOERROR == shellBrowser.QueryActiveShellView( out shellView ) )
				{
					
					if( NOERROR == shellView.GetItemObject( (uint)SVGIO.SVGIO_SELECTION, ref IID_IDataObject, out dataObject ) )
					{
						IDataObject iData1 = new DataObject( dataObject );
						if( iData1.GetDataPresent( DataFormats.FileDrop ) )
						{
							string[] paths = (string[])iData1.GetData( DataFormats.FileDrop );

							IDataObject iData = new DataObject( DataFormats.FileDrop, paths );

							MemoryStream dropEffect = new MemoryStream();
							byte[] bytes = new byte[] { bEffect, 0, 0, 0 };
							dropEffect.Write( bytes, 0, bytes.Length );
							dropEffect.SetLength( bytes.Length );

							iData.SetData( "Preferred DropEffect", dropEffect );
							Clipboard.SetDataObject( iData, true );

							return true;
						}
					}
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
			finally
			{
				if( shellView != null )
				{
					Marshal.ReleaseComObject( shellView );
					shellView = null;
				}

				if( dataObject != null )
				{
					Marshal.ReleaseComObject( dataObject );
					dataObject = null;
				}
			}

			return false;
		}

		enum SVGIO : uint
		{
			SVGIO_BACKGROUND = 0,
			SVGIO_SELECTION = 0x1,
			SVGIO_ALLVIEW = 0x2,
			SVGIO_CHECKED = 0x3,
			SVGIO_TYPE_MASK = 0xf,
			SVGIO_FLAG_VIEWORDER = 0x80000000
		}
	}

}
