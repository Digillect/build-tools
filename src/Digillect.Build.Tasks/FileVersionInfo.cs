using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Digillect.Build.Tasks
{
    public class FileVersionInfo : Task
    {
		[Required]
		public ITaskItem[] Files
		{
			get;
			set;
		}

		[Output]
		public ITaskItem[] FileVersionInfos
		{
			get;
			private set;
		}

		public override bool Execute()
		{
			if ( this.Files == null )
			{
				throw new ArgumentNullException("Files");
			}

			List<ITaskItem> list = new List<ITaskItem>();

			foreach ( var item in this.Files )
			{
				System.Diagnostics.FileVersionInfo fvi;

				try
				{
					fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(item.ItemSpec);
				}
				catch ( IOException ex )
				{
					this.Log.LogErrorFromException(ex, false, false, item.ItemSpec);

					continue;
				}

				ITaskItem verItem = new TaskItem(item);

				verItem.SetMetadata("Comments", fvi.Comments);
				verItem.SetMetadata("CompanyName", fvi.CompanyName);
				verItem.SetMetadata("FileDescription", fvi.FileDescription);
				//verItem.SetMetadata("FileName", fvi.FileName);
				verItem.SetMetadata("FileVersion", new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart).ToString());
				verItem.SetMetadata("VarFileVersion", fvi.FileVersion);
				verItem.SetMetadata("InternalName", fvi.InternalName);
				verItem.SetMetadata("Language", fvi.Language);
				verItem.SetMetadata("LegalCopyright", fvi.LegalCopyright);
				verItem.SetMetadata("LegalTrademarks", fvi.LegalTrademarks);
				verItem.SetMetadata("OriginalFilename", fvi.OriginalFilename);
				verItem.SetMetadata("PrivateBuild", fvi.PrivateBuild);
				verItem.SetMetadata("ProductVersion", new Version(fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart, fvi.ProductPrivatePart).ToString());
				verItem.SetMetadata("VarProductVersion", fvi.ProductVersion);
				verItem.SetMetadata("SpecialBuild", fvi.SpecialBuild);

				list.Add(verItem);
			}

			this.FileVersionInfos = list.ToArray();

			return !this.Log.HasLoggedErrors;
		}
	}
}
