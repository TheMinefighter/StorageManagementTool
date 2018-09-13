// ReSharper disable StringLiteralTypo

namespace StorageManagementCore.Backend {
	public partial class ShellFolder {
		public static class KnownShellFolders {
			public static readonly ShellFolder AddNewPrograms =
				new ShellFolder("AddNewPrograms", "de61d971-5ebc-4f02-a3a9-6c82895e5c04", false, false, null);

			public static readonly ShellFolder AdminTools =
				new ShellFolder("AdminTools", "724EF170-A42D-4FEF-9F26-B60E846FBA4F", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Administrative Tools");

			public static readonly ShellFolder AppUpdates =
				new ShellFolder("AppUpdates", "a305ce99-f527-492b-8b1a-7e76fa98d6e4", false, false, null);

			public static readonly ShellFolder CDBurning =
				new ShellFolder("CDBurning", "9E52AB10-F80D-49DF-ACB8-4330F5687855", true, true,
					@"%USERPROFILE%\AppData\Local\Microsoft\Windows\Burn\Burn");

			public static readonly ShellFolder ChangeRemovePrograms =
				new ShellFolder("ChangeRemovePrograms", "df7266ac-9274-4867-8d55-3bd661de872d", false, false, null);

			public static readonly ShellFolder CommonAdminTools =
				new ShellFolder("CommonAdminTools", "D0384E7D-BAC3-4797-8F14-CBA229B392B5", false, true,
					@"%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\Administrative Tools");

			public static readonly ShellFolder CommonOEMLinks =
				new ShellFolder("CommonOEMLinks", "C1BAE2D0-10DF-4334-BEDD-7AA20B227A9D", false, true, null);

			public static readonly ShellFolder CommonPrograms =
				new ShellFolder("CommonPrograms", "0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8", false, true,
					@"%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs");

			public static readonly ShellFolder CommonStartMenu =
				new ShellFolder("CommonStartMenu", "A4115719-D62E-491D-AA7C-E74B8BE3B067", false, true,
					@"%PROGRAMDATA%\Microsoft\Windows\Start Menu");

			public static readonly ShellFolder CommonStartup =
				new ShellFolder("CommonStartup", "82A5EA35-D9CD-47C5-9629-E15D2F714E6E", false, true,
					@"%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\Startup");

			public static readonly ShellFolder CommonTemplates =
				new ShellFolder("CommonTemplates", "B94237E7-57AC-4347-9151-B08C6C32D1F7", false, true,
					@"%PROGRAMDATA%\Microsoft\Windows\Templates");

			public static readonly ShellFolder ComputerFolder =
				new ShellFolder("ComputerFolder", "0AC0837C-BBF8-452A-850D-79D08E667CA7", false, false, "");

			public static readonly ShellFolder ConflictFolder =
				new ShellFolder("ConflictFolder", "4bfefb45-347d-4006-a5be-ac0cb0567192", false, false, null);

			public static readonly ShellFolder ConnectionsFolder =
				new ShellFolder("ConnectionsFolder", "6F0CD92B-2E97-45D1-88FF-B0D186B8DEDD", false, false, null);

			public static readonly ShellFolder Contacts =
				new ShellFolder("Contacts", "56784854-C6CB-462b-8169-88E350ACB882", true, true, @"%USERPROFILE%\Contacts");

			public static readonly ShellFolder ControlPanelFolder =
				new ShellFolder("ControlPanelFolder", "82A74AEB-AEB4-465C-A014-D097EE346D63", false, false, null);

			public static readonly ShellFolder Cookies =
				new ShellFolder("Cookies", "2B0F765D-C0E9-4171-908E-08A611B84FF6", true, true,
					@"%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCookies");

			public static readonly ShellFolder Desktop =
				new ShellFolder("Desktop", "B4BFCC3A-DB2C-424C-B029-7FE99A87C641", true, true, @"%USERPROFILE%\Desktop");

			public static readonly ShellFolder Documents =
				new ShellFolder("Documents", "FDD39AD0-238F-46AF-ADB4-6C85480369C7", true, true, @"%USERPROFILE%\Documents");

			public static readonly ShellFolder Downloads =
				new ShellFolder("Downloads", "374DE290-123F-4565-9164-39C4925E467B", true, true, @"%USERPROFILE%\Downloads");

			public static readonly ShellFolder Favorites =
				new ShellFolder("Favorites", "1777F761-68AD-4D8A-87BD-30B759FA33DD", true, true, @"%USERPROFILE%\Favorites");

			public static readonly ShellFolder Fonts =
				new ShellFolder("Fonts", "FD228CB7-AE11-4AE3-864C-16F3910AB8FE", false, false, @"%WINDIR%\Fonts");

			public static readonly ShellFolder Games =
				new ShellFolder("Games", "CAC52C1A-B53D-4edc-92D7-6B2E8AC19434", false, false, null);

			public static readonly ShellFolder GameTasks =
				new ShellFolder("GameTasks", "054FAE61-4DD8-4787-80B6-090220C4B700", true, false,
					@"%USERPROFILE%\AppData\Local\Microsoft\Windows\GameExplorer");

			public static readonly ShellFolder History =
				new ShellFolder("History", "D9DC8A3B-B784-432E-A781-5A1130A75963", true, true,
					@"%USERPROFILE%\AppData\Local\Microsoft\Windows\History");

			public static readonly ShellFolder InternetCache =
				new ShellFolder("InternetCache", "352481E8-33BE-4251-BA85-6007CAEDCF9D", true, false,
					@"%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCache");

			public static readonly ShellFolder InternetFolder =
				new ShellFolder("InternetFolder", "4D9F7874-4E0C-4904-967B-40B0D20C3E4B", false, false, null);

			public static readonly ShellFolder Links =
				new ShellFolder("Links", "bfb9d5e0-c6a9-404c-b2b2-ae6db6af4968", true, true, @"%USERPROFILE%\Links");

			public static readonly ShellFolder LocalAppData =
				new ShellFolder("LocalAppData", "F1B32785-6FBA-4FCF-9D55-7B8E7F157091", true, false, @"%USERPROFILE%\AppData\Local");

			public static readonly ShellFolder LocalAppDataLow =
				new ShellFolder("LocalAppDataLow", "A520A1A4-1780-4FF6-BD18-167343C5AF16", true, false,
					@"%USERPROFILE%\AppData\LocalLow");

			public static readonly ShellFolder LocalizedResourcesDir =
				new ShellFolder("LocalizedResourcesDir", "2A00375E-224C-49DE-B8D1-440DF7EF3DDC", false, false, null);

			public static readonly ShellFolder Music =
				new ShellFolder("Music", "4BD8D571-6D19-48D3-BE97-422220080E43", true, true, @"%USERPROFILE%\Music");

			public static readonly ShellFolder NetHood =
				new ShellFolder("NetHood", "C5ABBF53-E17F-4121-8900-86626FC2C973", true, false,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Network Shortcuts");

			public static readonly ShellFolder NetworkFolder =
				new ShellFolder("NetworkFolder", "D20BEEC4-5CA8-4905-AE3B-BF251EA09B53", false, false, null);

			public static readonly ShellFolder OriginalImages =
				new ShellFolder("OriginalImages", "2C36C0AA-5812-4b87-BFD0-4CD0DFB19B39", false, false, null);

			public static readonly ShellFolder PhotoAlbums =
				new ShellFolder("PhotoAlbums", "69D2CF90-FC33-4FB7-9A0C-EBB0F0FCB43C", false, false, null);

			public static readonly ShellFolder Pictures =
				new ShellFolder("Pictures", "33E28130-4E1E-4676-835A-98395C3BC3BB", true, true, @"%USERPROFILE%\Pictures");

			public static readonly ShellFolder Playlists =
				new ShellFolder("Playlists", "DE92C1C7-837F-4F69-A3BB-86E631204A23", false, false, null);

			public static readonly ShellFolder PrintersFolder =
				new ShellFolder("PrintersFolder", "76FC4E2D-D6AD-4519-A663-37BD56068185", false, false, null);

			public static readonly ShellFolder PrintHood =
				new ShellFolder("PrintHood", "9274BD8D-CFD1-41C3-B35E-B13F55A758F4", true, false,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Printer Shortcuts");

			public static readonly ShellFolder Profile =
				new ShellFolder("Profile", "5E6C858F-0E22-4760-9AFE-EA3317B67173", true, false, @"%HOMEDRIVE%\Users\%USERNAME%");

			public static readonly ShellFolder ProgramData =
				new ShellFolder("ProgramData", "62AB5D82-FDC1-4DC3-A9DD-070D1D495D97", false, false, @"%HOMEDRIVE%\ProgramData");

			public static readonly ShellFolder ProgramFiles =
				new ShellFolder("ProgramFiles", "905e63b6-c1bf-494e-b29c-65b732d3d21a", false, false,
					@"%HOMEDRIVE%\Program Files (x86)");

			public static readonly ShellFolder ProgramFilesX64 =
				new ShellFolder("ProgramFilesX64", "6D809377-6AF0-444b-8957-A3773F02200E", false, false, null);

			public static readonly ShellFolder ProgramFilesX86 =
				new ShellFolder("ProgramFilesX86", "7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E", false, false,
					@"%HOMEDRIVE%\Program Files (x86)");

			public static readonly ShellFolder ProgramFilesCommon =
				new ShellFolder("ProgramFilesCommon", "F7F1ED05-9F6D-47A2-AAAE-29D317C6F066", false, false,
					@"%PROGRAMFILES(X86)%\Common Files");

			public static readonly ShellFolder ProgramFilesCommonX64 =
				new ShellFolder("ProgramFilesCommonX64", "6365D5A7-0F0D-45E5-87F6-0DA56B6A4F7D", false, false, null);

			public static readonly ShellFolder ProgramFilesCommonX86 = new ShellFolder("ProgramFilesCommonX86",
				"DE974D24-D9C6-4D3E-BF91-F4455120B917", false, false, @"%PROGRAMFILES(X86)%\Common Files");

			public static readonly ShellFolder Programs =
				new ShellFolder("Programs", "A77F5D77-2E2B-44C3-A6A2-ABA601054A51", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs");

			public static readonly ShellFolder Public =
				new ShellFolder("Public", "DFDF76A2-C82A-4D63-906A-5644AC457385", false, false, @"%HOMEDRIVE%\Users\Public");

			public static readonly ShellFolder PublicDesktop =
				new ShellFolder("PublicDesktop", "C4AA340D-F20F-4863-AFEF-F87EF2E6BA25", false, true, @"%PUBLIC%\Desktop");

			public static readonly ShellFolder PublicDocuments =
				new ShellFolder("PublicDocuments", "ED4824AF-DCE4-45A8-81E2-FC7965083634", false, true, @"%PUBLIC%\Documents");

			public static readonly ShellFolder PublicDownloads =
				new ShellFolder("PublicDownloads", "3D644C9B-1FB8-4f30-9B45-F670235F79C0", false, true, @"%PUBLIC%\Downloads");

			public static readonly ShellFolder PublicGameTasks =
				new ShellFolder("PublicGameTasks", "DEBF2536-E1A8-4c59-B6A2-414586476AEA", false, true,
					@"%PROGRAMDATA%\Microsoft\Windows\GameExplorer");

			public static readonly ShellFolder PublicMusic =
				new ShellFolder("PublicMusic", "3214FAB5-9757-4298-BB61-92A9DEAA44FF", false, true, @"%PUBLIC%\Music");

			public static readonly ShellFolder PublicPictures =
				new ShellFolder("PublicPictures", "B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5", false, true, @"%PUBLIC%\Pictures");

			public static readonly ShellFolder PublicVideos =
				new ShellFolder("PublicVideos", "2400183A-6185-49FB-A2D8-4A392A602BA3", false, true, @"%PUBLIC%\Videos");

			public static readonly ShellFolder QuickLaunch =
				new ShellFolder("QuickLaunch", "52a4f021-7b75-48a9-9f6b-4b87a210bc8f", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Internet Explorer\Quick Launch");

			public static readonly ShellFolder Recent =
				new ShellFolder("Recent", "AE50C081-EBD2-438A-8655-8A092E34987A", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Recent");

			public static readonly ShellFolder RecycleBinFolder =
				new ShellFolder("RecycleBinFolder", "B7534046-3ECB-4C18-BE4E-64CD4CB7D6AC", false, false, null);

			public static readonly ShellFolder ResourceDir =
				new ShellFolder("ResourceDir", "8AD10C31-2ADB-4296-A8F7-E4701232C972", false, false, @"%WINDIR%\resources");

			public static readonly ShellFolder RoamingAppData =
				new ShellFolder("RoamingAppData", "3EB685DB-65F9-4CF6-A03A-E3EF65729F3D", true, true,
					@"%USERPROFILE%\AppData\Roaming");

			public static readonly ShellFolder SampleMusic =
				new ShellFolder("SampleMusic", "B250C668-F57D-4EE1-A63C-290EE7D1AA1F", false, false, null);

			public static readonly ShellFolder SamplePictures =
				new ShellFolder("SamplePictures", "C4900540-2379-4C75-844B-64E6FAF8716B", false, false, null);

			public static readonly ShellFolder SamplePlaylists =
				new ShellFolder("SamplePlaylists", "15CA69B3-30EE-49C1-ACE1-6B5EC372AFB5", false, false, null);

			public static readonly ShellFolder SampleVideos =
				new ShellFolder("SampleVideos", "859EAD94-2E85-48AD-A71A-0969CB56A6CD", false, false, null);

			public static readonly ShellFolder SavedGames =
				new ShellFolder("SavedGames", "4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4", true, false, @"%USERPROFILE%\Saved Games");

			public static readonly ShellFolder SavedSearches =
				new ShellFolder("SavedSearches", "7d1d3a04-debb-4115-95cf-2f29da2920da", true, false, @"%USERPROFILE%\Searches");

			public static readonly ShellFolder SEARCH_CSC =
				new ShellFolder("SEARCH_CSC", "ee32e446-31ca-4aba-814f-a5ebd2fd6d5e", false, false, null);

			public static readonly ShellFolder SEARCH_MAPI =
				new ShellFolder("SEARCH_MAPI", "98ec0e18-2098-4d44-8644-66979315a281", false, false, null);

			public static readonly ShellFolder SearchHome =
				new ShellFolder("SearchHome", "190337d1-b8ca-4121-a639-6d472d16972a", false, false, null);

			public static readonly ShellFolder SendTo =
				new ShellFolder("SendTo", "8983036C-27C0-404B-8F08-102D10DCFD74", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\SendTo");

			public static readonly ShellFolder SidebarDefaultParts =
				new ShellFolder("SidebarDefaultParts", "7B396E54-9EC5-4300-BE0A-2482EBAE1A26", false, false, null);

			public static readonly ShellFolder SidebarParts =
				new ShellFolder("SidebarParts", "A75D362E-50FC-4fb7-AC2C-A8BEAA314493", false, false, null);

			public static readonly ShellFolder StartMenu =
				new ShellFolder("StartMenu", "625B53C3-AB48-4EC1-BA1F-A1EF4146FC19", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu");

			public static readonly ShellFolder Startup =
				new ShellFolder("Startup", "B97D20BB-F46A-4C97-BA10-5E3608430854", true, true,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup");

			public static readonly ShellFolder SyncManagerFolder =
				new ShellFolder("SyncManagerFolder", "43668BF8-C14E-49B2-97C9-747784D784B7", false, false, null);

			public static readonly ShellFolder SyncResultsFolder =
				new ShellFolder("SyncResultsFolder", "289a9a43-be44-4057-a41b-587a76d7e7f9", false, false, null);

			public static readonly ShellFolder SyncSetupFolder =
				new ShellFolder("SyncSetupFolder", "0F214138-B1D3-4a90-BBA9-27CBC0C5389A", false, false, null);

			public static readonly ShellFolder System =
				new ShellFolder("System", "1AC14E77-02E7-4E5D-B744-2EB1AE5198B7", false, false, @"%WINDIR%\system32");

			public static readonly ShellFolder SystemX86 =
				new ShellFolder("SystemX86", "D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27", false, false, @"%WINDIR%\SysWOW64");

			public static readonly ShellFolder Templates =
				new ShellFolder("Templates", "A63293E8-664E-48DB-A079-DF759E0509F7", true, false,
					@"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Templates");

			public static readonly ShellFolder TreeProperties =
				new ShellFolder("TreeProperties", "5b3749ad-b49f-49c1-83eb-15370fbd4882", false, false, null);

			public static readonly ShellFolder UserProfiles =
				new ShellFolder("UserProfiles", "0762D272-C50A-4BB0-A382-697DCD729B80", false, false, @"%HOMEDRIVE%\Users");

			public static readonly ShellFolder UsersFiles =
				new ShellFolder("UsersFiles", "f3ce0f7c-4901-4acc-8648-d5d44b04ef8f", false, false, null);

			public static readonly ShellFolder Videos =
				new ShellFolder("Videos", "18989B1D-99B5-455B-841C-AB7C74E4DDFC", true, true, @"%USERPROFILE%\Videos");

			public static readonly ShellFolder Windows =
				new ShellFolder("Windows", "F38BF404-1D43-42F2-9305-67DE0B28FC23", false, false, @"%HOMEDRIVE%\WINDOWS");
		}
	}
}