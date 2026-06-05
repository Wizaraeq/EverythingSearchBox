		//internal static bool CopyCut( IPluginServer pluginServer, IShellBrowser shellBrowser, bool fCopy )
		//{
		//    Address[] addresses;
		//    if( pluginServer.TryGetSelection( out addresses ) )
		//    {
		//        List<string> lstPaths = new List<string>();
		//        foreach( Address ad in addresses )
		//        {
		//            if( ad.Path != null && ad.Path.Length > 0 && ( File.Exists( ad.Path ) || Directory.Exists( ad.Path ) ) )
		//            {
		//                lstPaths.Add( ad.Path );
		//            }
		//        }

		//        // is there any other way to copy/cut files?
		//        // APPCOMMAND_CUT??

		//        if( lstPaths.Count > 0 )
		//        {
		//            IDataObject iData = new DataObject( DataFormats.FileDrop, lstPaths.ToArray() );

		//            byte bEffect = fCopy ? (byte)5 : (byte)2;

		//            using( MemoryStream dropEffect = new MemoryStream( 4 ) )
		//            {
		//                byte[] bytes = new byte[] { bEffect, 0, 0, 0 };
		//                dropEffect.Write( bytes, 0, bytes.Length );

		//                iData.SetData( "Preferred DropEffect", dropEffect );
		//                Clipboard.SetDataObject( iData, true );
		//            }

		//            if( !fCopy && shellBrowser != null )
		//                FileOps.SetCutEffect( shellBrowser );

		//            return true;
		//        }
		//    }

		//    return false;
		//}	
