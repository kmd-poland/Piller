using System;
using System.IO;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;
using Android.Graphics;
using Android.Support.V4.Content;

namespace Services
{
	public class AndroidImageLoader : ImageLoaderService
	{
		private readonly IMvxFileStore fileStore = Mvx.Resolve<IMvxFileStore>();

		public static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}

		public byte[] LoadImage(string name)
		{
			var path = this.fileStore.NativePath("Piller") + name;
            return ReadFully(fileStore.OpenRead(path));
		}

		public void SaveImage(byte[] bytes, string name, int compressionRate = 100)
		{
			var path = this.fileStore.NativePath("Piller_") + name;

			if (compressionRate != 100)
			{
				var stream = new MemoryStream();
				Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0 ,bytes.Length); 
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, compressionRate, stream);
				fileStore.WriteFile(path, stream.ToArray());	
			}
			else 
			{
				fileStore.WriteFile(path, bytes);	
			}
		}
	}
}
