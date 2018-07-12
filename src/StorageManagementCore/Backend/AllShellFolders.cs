using System;

// ReSharper disable StringLiteralTypo

namespace StorageManagementCore.Backend {
	public class AllShellFolders {
		public static readonly AdvancedUserShellFolder AddNewPrograms = new AdvancedUserShellFolder {
			Name = "AddNewPrograms",
			WindowsIdentifier = new Guid(0xde61d971, 0x5ebc, 0x4f02, 0xa3, 0xa9, 0x6c, 0x82, 0x89, 0x5e, 0x5c, 0x04),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder AdminTools = new AdvancedUserShellFolder {
			Name = "AdminTools",
			WindowsIdentifier = new Guid(0x724EF170, 0xA42D, 0x4FEF, 0x9F, 0x26, 0xB6, 0x0E, 0x84, 0x6F, 0xBA, 0x4F),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder AppUpdates = new AdvancedUserShellFolder {
			Name = "AppUpdates",
			WindowsIdentifier = new Guid(0xa305ce99, 0xf527, 0x492b, 0x8b, 0x1a, 0x7e, 0x76, 0xfa, 0x98, 0xd6, 0xe4),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder CDBurning = new AdvancedUserShellFolder {
			Name = "CDBurning",
			WindowsIdentifier = new Guid(0x9E52AB10, 0xF80D, 0x49DF, 0xAC, 0xB8, 0x43, 0x30, 0xF5, 0x68, 0x78, 0x55),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder ChangeRemovePrograms = new AdvancedUserShellFolder {
			Name = "ChangeRemovePrograms",
			WindowsIdentifier = new Guid(0xdf7266ac, 0x9274, 0x4867, 0x8d, 0x55, 0x3b, 0xd6, 0x61, 0xde, 0x87, 0x2d),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder CommonAdminTools = new AdvancedUserShellFolder {
			Name = "CommonAdminTools",
			WindowsIdentifier = new Guid(0xD0384E7D, 0xBAC3, 0x4797, 0x8F, 0x14, 0xCB, 0xA2, 0x29, 0xB3, 0x92, 0xB5),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder CommonOEMLinks = new AdvancedUserShellFolder {
			Name = "CommonOEMLinks",
			WindowsIdentifier = new Guid(0xC1BAE2D0, 0x10DF, 0x4334, 0xBE, 0xDD, 0x7A, 0xA2, 0x0B, 0x22, 0x7A, 0x9D),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder CommonPrograms = new AdvancedUserShellFolder {
			Name = "CommonPrograms",
			WindowsIdentifier = new Guid(0x0139D44E, 0x6AFE, 0x49F2, 0x86, 0x90, 0x3D, 0xAF, 0xCA, 0xE6, 0xFF, 0xB8),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder CommonStartMenu = new AdvancedUserShellFolder {
			Name = "CommonStartMenu",
			WindowsIdentifier = new Guid(0xA4115719, 0xD62E, 0x491D, 0xAA, 0x7C, 0xE7, 0x4B, 0x8B, 0xE3, 0xB0, 0x67),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder CommonStartup = new AdvancedUserShellFolder {
			Name = "CommonStartup",
			WindowsIdentifier = new Guid(0x82A5EA35, 0xD9CD, 0x47C5, 0x96, 0x29, 0xE1, 0x5D, 0x2F, 0x71, 0x4E, 0x6E),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder CommonTemplates = new AdvancedUserShellFolder {
			Name = "CommonTemplates",
			WindowsIdentifier = new Guid(0xB94237E7, 0x57AC, 0x4347, 0x91, 0x51, 0xB0, 0x8C, 0x6C, 0x32, 0xD1, 0xF7),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder ComputerFolder = new AdvancedUserShellFolder {
			Name = "ComputerFolder",
			WindowsIdentifier = new Guid(0x0AC0837C, 0xBBF8, 0x452A, 0x85, 0x0D, 0x79, 0xD0, 0x8E, 0x66, 0x7C, 0xA7),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ConflictFolder = new AdvancedUserShellFolder {
			Name = "ConflictFolder",
			WindowsIdentifier = new Guid(0x4bfefb45, 0x347d, 0x4006, 0xa5, 0xbe, 0xac, 0x0c, 0xb0, 0x56, 0x71, 0x92),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ConnectionsFolder = new AdvancedUserShellFolder {
			Name = "ConnectionsFolder",
			WindowsIdentifier = new Guid(0x6F0CD92B, 0x2E97, 0x45D1, 0x88, 0xFF, 0xB0, 0xD1, 0x86, 0xB8, 0xDE, 0xDD),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Contacts = new AdvancedUserShellFolder {
			Name = "Contacts",
			WindowsIdentifier = new Guid(0x56784854, 0xC6CB, 0x462b, 0x81, 0x69, 0x88, 0xE3, 0x50, 0xAC, 0xB8, 0x82),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder ControlPanelFolder = new AdvancedUserShellFolder {
			Name = "ControlPanelFolder",
			WindowsIdentifier = new Guid(0x82A74AEB, 0xAEB4, 0x465C, 0xA0, 0x14, 0xD0, 0x97, 0xEE, 0x34, 0x6D, 0x63),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Cookies = new AdvancedUserShellFolder {
			Name = "Cookies",
			WindowsIdentifier = new Guid(0x2B0F765D, 0xC0E9, 0x4171, 0x90, 0x8E, 0x08, 0xA6, 0x11, 0xB8, 0x4F, 0xF6),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Desktop = new AdvancedUserShellFolder {
			Name = "Desktop",
			WindowsIdentifier = new Guid(0xB4BFCC3A, 0xDB2C, 0x424C, 0xB0, 0x29, 0x7F, 0xE9, 0x9A, 0x87, 0xC6, 0x41),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Documents = new AdvancedUserShellFolder {
			Name = "Documents",
			WindowsIdentifier = new Guid(0xFDD39AD0, 0x238F, 0x46AF, 0xAD, 0xB4, 0x6C, 0x85, 0x48, 0x03, 0x69, 0xC7),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Downloads = new AdvancedUserShellFolder {
			Name = "Downloads",
			WindowsIdentifier = new Guid(0x374DE290, 0x123F, 0x4565, 0x91, 0x64, 0x39, 0xC4, 0x92, 0x5E, 0x46, 0x7B),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Favorites = new AdvancedUserShellFolder {
			Name = "Favorites",
			WindowsIdentifier = new Guid(0x1777F761, 0x68AD, 0x4D8A, 0x87, 0xBD, 0x30, 0xB7, 0x59, 0xFA, 0x33, 0xDD),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Fonts = new AdvancedUserShellFolder {
			Name = "Fonts", WindowsIdentifier = new Guid(0xFD228CB7, 0xAE11, 0x4AE3, 0x86, 0x4C, 0x16, 0xF3, 0x91, 0x0A, 0xB8, 0xFE),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Games = new AdvancedUserShellFolder {
			Name = "Games", WindowsIdentifier = new Guid(0xCAC52C1A, 0xB53D, 0x4edc, 0x92, 0xD7, 0x6B, 0x2E, 0x8A, 0xC1, 0x94, 0x34),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder GameTasks = new AdvancedUserShellFolder {
			Name = "GameTasks",
			WindowsIdentifier = new Guid(0x054FAE61, 0x4DD8, 0x4787, 0x80, 0xB6, 0x09, 0x02, 0x20, 0xC4, 0xB7, 0x00),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder History = new AdvancedUserShellFolder {
			Name = "History",
			WindowsIdentifier = new Guid(0xD9DC8A3B, 0xB784, 0x432E, 0xA7, 0x81, 0x5A, 0x11, 0x30, 0xA7, 0x59, 0x63),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder InternetCache = new AdvancedUserShellFolder {
			Name = "InternetCache",
			WindowsIdentifier = new Guid(0x352481E8, 0x33BE, 0x4251, 0xBA, 0x85, 0x60, 0x07, 0xCA, 0xED, 0xCF, 0x9D),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder InternetFolder = new AdvancedUserShellFolder {
			Name = "InternetFolder",
			WindowsIdentifier = new Guid(0x4D9F7874, 0x4E0C, 0x4904, 0x96, 0x7B, 0x40, 0xB0, 0xD2, 0x0C, 0x3E, 0x4B),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Links = new AdvancedUserShellFolder {
			Name = "Links", WindowsIdentifier = new Guid(0xbfb9d5e0, 0xc6a9, 0x404c, 0xb2, 0xb2, 0xae, 0x6d, 0xb6, 0xaf, 0x49, 0x68),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder LocalAppData = new AdvancedUserShellFolder {
			Name = "LocalAppData",
			WindowsIdentifier = new Guid(0xF1B32785, 0x6FBA, 0x4FCF, 0x9D, 0x55, 0x7B, 0x8E, 0x7F, 0x15, 0x70, 0x91),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder LocalAppDataLow = new AdvancedUserShellFolder {
			Name = "LocalAppDataLow",
			WindowsIdentifier = new Guid(0xA520A1A4, 0x1780, 0x4FF6, 0xBD, 0x18, 0x16, 0x73, 0x43, 0xC5, 0xAF, 0x16),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder LocalizedResourcesDir = new AdvancedUserShellFolder {
			Name = "LocalizedResourcesDir",
			WindowsIdentifier = new Guid(0x2A00375E, 0x224C, 0x49DE, 0xB8, 0xD1, 0x44, 0x0D, 0xF7, 0xEF, 0x3D, 0xDC),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Music = new AdvancedUserShellFolder {
			Name = "Music", WindowsIdentifier = new Guid(0x4BD8D571, 0x6D19, 0x48D3, 0xBE, 0x97, 0x42, 0x22, 0x20, 0x08, 0x0E, 0x43),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder NetHood = new AdvancedUserShellFolder {
			Name = "NetHood",
			WindowsIdentifier = new Guid(0xC5ABBF53, 0xE17F, 0x4121, 0x89, 0x00, 0x86, 0x62, 0x6F, 0xC2, 0xC9, 0x73),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder NetworkFolder = new AdvancedUserShellFolder {
			Name = "NetworkFolder",
			WindowsIdentifier = new Guid(0xD20BEEC4, 0x5CA8, 0x4905, 0xAE, 0x3B, 0xBF, 0x25, 0x1E, 0xA0, 0x9B, 0x53),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder OriginalImages = new AdvancedUserShellFolder {
			Name = "OriginalImages",
			WindowsIdentifier = new Guid(0x2C36C0AA, 0x5812, 0x4b87, 0xBF, 0xD0, 0x4C, 0xD0, 0xDF, 0xB1, 0x9B, 0x39),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder PhotoAlbums = new AdvancedUserShellFolder {
			Name = "PhotoAlbums",
			WindowsIdentifier = new Guid(0x69D2CF90, 0xFC33, 0x4FB7, 0x9A, 0x0C, 0xEB, 0xB0, 0xF0, 0xFC, 0xB4, 0x3C),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Pictures = new AdvancedUserShellFolder {
			Name = "Pictures",
			WindowsIdentifier = new Guid(0x33E28130, 0x4E1E, 0x4676, 0x83, 0x5A, 0x98, 0x39, 0x5C, 0x3B, 0xC3, 0xBB),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Playlists = new AdvancedUserShellFolder {
			Name = "Playlists",
			WindowsIdentifier = new Guid(0xDE92C1C7, 0x837F, 0x4F69, 0xA3, 0xBB, 0x86, 0xE6, 0x31, 0x20, 0x4A, 0x23),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder PrintersFolder = new AdvancedUserShellFolder {
			Name = "PrintersFolder",
			WindowsIdentifier = new Guid(0x76FC4E2D, 0xD6AD, 0x4519, 0xA6, 0x63, 0x37, 0xBD, 0x56, 0x06, 0x81, 0x85),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder PrintHood = new AdvancedUserShellFolder {
			Name = "PrintHood",
			WindowsIdentifier = new Guid(0x9274BD8D, 0xCFD1, 0x41C3, 0xB3, 0x5E, 0xB1, 0x3F, 0x55, 0xA7, 0x58, 0xF4),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Profile = new AdvancedUserShellFolder {
			Name = "Profile",
			WindowsIdentifier = new Guid(0x5E6C858F, 0x0E22, 0x4760, 0x9A, 0xFE, 0xEA, 0x33, 0x17, 0xB6, 0x71, 0x73),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramData = new AdvancedUserShellFolder {
			Name = "ProgramData",
			WindowsIdentifier = new Guid(0x62AB5D82, 0xFDC1, 0x4DC3, 0xA9, 0xDD, 0x07, 0x0D, 0x1D, 0x49, 0x5D, 0x97),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramFiles = new AdvancedUserShellFolder {
			Name = "ProgramFiles",
			WindowsIdentifier = new Guid(0x905e63b6, 0xc1bf, 0x494e, 0xb2, 0x9c, 0x65, 0xb7, 0x32, 0xd3, 0xd2, 0x1a),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramFilesX64 = new AdvancedUserShellFolder {
			Name = "ProgramFilesX64",
			WindowsIdentifier = new Guid(0x6D809377, 0x6AF0, 0x444b, 0x89, 0x57, 0xA3, 0x77, 0x3F, 0x02, 0x20, 0x0E),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramFilesX86 = new AdvancedUserShellFolder {
			Name = "ProgramFilesX86",
			WindowsIdentifier = new Guid(0x7C5A40EF, 0xA0FB, 0x4BFC, 0x87, 0x4A, 0xC0, 0xF2, 0xE0, 0xB9, 0xFA, 0x8E),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramFilesCommon = new AdvancedUserShellFolder {
			Name = "ProgramFilesCommon",
			WindowsIdentifier = new Guid(0xF7F1ED05, 0x9F6D, 0x47A2, 0xAA, 0xAE, 0x29, 0xD3, 0x17, 0xC6, 0xF0, 0x66),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramFilesCommonX64 = new AdvancedUserShellFolder {
			Name = "ProgramFilesCommonX64",
			WindowsIdentifier = new Guid(0x6365D5A7, 0x0F0D, 0x45E5, 0x87, 0xF6, 0x0D, 0xA5, 0x6B, 0x6A, 0x4F, 0x7D),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ProgramFilesCommonX86 = new AdvancedUserShellFolder {
			Name = "ProgramFilesCommonX86",
			WindowsIdentifier = new Guid(0xDE974D24, 0xD9C6, 0x4D3E, 0xBF, 0x91, 0xF4, 0x45, 0x51, 0x20, 0xB9, 0x17),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Programs = new AdvancedUserShellFolder {
			Name = "Programs",
			WindowsIdentifier = new Guid(0xA77F5D77, 0x2E2B, 0x44C3, 0xA6, 0xA2, 0xAB, 0xA6, 0x01, 0x05, 0x4A, 0x51),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Public = new AdvancedUserShellFolder {
			Name = "Public",
			WindowsIdentifier = new Guid(0xDFDF76A2, 0xC82A, 0x4D63, 0x90, 0x6A, 0x56, 0x44, 0xAC, 0x45, 0x73, 0x85),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder PublicDesktop = new AdvancedUserShellFolder {
			Name = "PublicDesktop",
			WindowsIdentifier = new Guid(0xC4AA340D, 0xF20F, 0x4863, 0xAF, 0xEF, 0xF8, 0x7E, 0xF2, 0xE6, 0xBA, 0x25),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder PublicDocuments = new AdvancedUserShellFolder {
			Name = "PublicDocuments",
			WindowsIdentifier = new Guid(0xED4824AF, 0xDCE4, 0x45A8, 0x81, 0xE2, 0xFC, 0x79, 0x65, 0x08, 0x36, 0x34),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder PublicDownloads = new AdvancedUserShellFolder {
			Name = "PublicDownloads",
			WindowsIdentifier = new Guid(0x3D644C9B, 0x1FB8, 0x4f30, 0x9B, 0x45, 0xF6, 0x70, 0x23, 0x5F, 0x79, 0xC0),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder PublicGameTasks = new AdvancedUserShellFolder {
			Name = "PublicGameTasks",
			WindowsIdentifier = new Guid(0xDEBF2536, 0xE1A8, 0x4c59, 0xB6, 0xA2, 0x41, 0x45, 0x86, 0x47, 0x6A, 0xEA),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder PublicMusic = new AdvancedUserShellFolder {
			Name = "PublicMusic",
			WindowsIdentifier = new Guid(0x3214FAB5, 0x9757, 0x4298, 0xBB, 0x61, 0x92, 0xA9, 0xDE, 0xAA, 0x44, 0xFF),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder PublicPictures = new AdvancedUserShellFolder {
			Name = "PublicPictures",
			WindowsIdentifier = new Guid(0xB6EBFB86, 0x6907, 0x413C, 0x9A, 0xF7, 0x4F, 0xC2, 0xAB, 0xF0, 0x7C, 0xC5),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder PublicVideos = new AdvancedUserShellFolder {
			Name = "PublicVideos",
			WindowsIdentifier = new Guid(0x2400183A, 0x6185, 0x49FB, 0xA2, 0xD8, 0x4A, 0x39, 0x2A, 0x60, 0x2B, 0xA3),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder QuickLaunch = new AdvancedUserShellFolder {
			Name = "QuickLaunch",
			WindowsIdentifier = new Guid(0x52a4f021, 0x7b75, 0x48a9, 0x9f, 0x6b, 0x4b, 0x87, 0xa2, 0x10, 0xbc, 0x8f),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Recent = new AdvancedUserShellFolder {
			Name = "Recent",
			WindowsIdentifier = new Guid(0xAE50C081, 0xEBD2, 0x438A, 0x86, 0x55, 0x8A, 0x09, 0x2E, 0x34, 0x98, 0x7A),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder RecycleBinFolder = new AdvancedUserShellFolder {
			Name = "RecycleBinFolder",
			WindowsIdentifier = new Guid(0xB7534046, 0x3ECB, 0x4C18, 0xBE, 0x4E, 0x64, 0xCD, 0x4C, 0xB7, 0xD6, 0xAC),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder ResourceDir = new AdvancedUserShellFolder {
			Name = "ResourceDir",
			WindowsIdentifier = new Guid(0x8AD10C31, 0x2ADB, 0x4296, 0xA8, 0xF7, 0xE4, 0x70, 0x12, 0x32, 0xC9, 0x72),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder RoamingAppData = new AdvancedUserShellFolder {
			Name = "RoamingAppData",
			WindowsIdentifier = new Guid(0x3EB685DB, 0x65F9, 0x4CF6, 0xA0, 0x3A, 0xE3, 0xEF, 0x65, 0x72, 0x9F, 0x3D),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder SampleMusic = new AdvancedUserShellFolder {
			Name = "SampleMusic",
			WindowsIdentifier = new Guid(0xB250C668, 0xF57D, 0x4EE1, 0xA6, 0x3C, 0x29, 0x0E, 0xE7, 0xD1, 0xAA, 0x1F),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SamplePictures = new AdvancedUserShellFolder {
			Name = "SamplePictures",
			WindowsIdentifier = new Guid(0xC4900540, 0x2379, 0x4C75, 0x84, 0x4B, 0x64, 0xE6, 0xFA, 0xF8, 0x71, 0x6B),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SamplePlaylists = new AdvancedUserShellFolder {
			Name = "SamplePlaylists",
			WindowsIdentifier = new Guid(0x15CA69B3, 0x30EE, 0x49C1, 0xAC, 0xE1, 0x6B, 0x5E, 0xC3, 0x72, 0xAF, 0xB5),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SampleVideos = new AdvancedUserShellFolder {
			Name = "SampleVideos",
			WindowsIdentifier = new Guid(0x859EAD94, 0x2E85, 0x48AD, 0xA7, 0x1A, 0x09, 0x69, 0xCB, 0x56, 0xA6, 0xCD),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SavedGames = new AdvancedUserShellFolder {
			Name = "SavedGames",
			WindowsIdentifier = new Guid(0x4C5C32FF, 0xBB9D, 0x43b0, 0xB5, 0xB4, 0x2D, 0x72, 0xE5, 0x4E, 0xAA, 0xA4),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SavedSearches = new AdvancedUserShellFolder {
			Name = "SavedSearches",
			WindowsIdentifier = new Guid(0x7d1d3a04, 0xdebb, 0x4115, 0x95, 0xcf, 0x2f, 0x29, 0xda, 0x29, 0x20, 0xda),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SEARCH_CSC = new AdvancedUserShellFolder {
			Name = "SEARCH_CSC",
			WindowsIdentifier = new Guid(0xee32e446, 0x31ca, 0x4aba, 0x81, 0x4f, 0xa5, 0xeb, 0xd2, 0xfd, 0x6d, 0x5e),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SEARCH_MAPI = new AdvancedUserShellFolder {
			Name = "SEARCH_MAPI",
			WindowsIdentifier = new Guid(0x98ec0e18, 0x2098, 0x4d44, 0x86, 0x44, 0x66, 0x97, 0x93, 0x15, 0xa2, 0x81),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SearchHome = new AdvancedUserShellFolder {
			Name = "SearchHome",
			WindowsIdentifier = new Guid(0x190337d1, 0xb8ca, 0x4121, 0xa6, 0x39, 0x6d, 0x47, 0x2d, 0x16, 0x97, 0x2a),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SendTo = new AdvancedUserShellFolder {
			Name = "SendTo",
			WindowsIdentifier = new Guid(0x8983036C, 0x27C0, 0x404B, 0x8F, 0x08, 0x10, 0x2D, 0x10, 0xDC, 0xFD, 0x74),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder SidebarDefaultParts = new AdvancedUserShellFolder {
			Name = "SidebarDefaultParts",
			WindowsIdentifier = new Guid(0x7B396E54, 0x9EC5, 0x4300, 0xBE, 0x0A, 0x24, 0x82, 0xEB, 0xAE, 0x1A, 0x26),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SidebarParts = new AdvancedUserShellFolder {
			Name = "SidebarParts",
			WindowsIdentifier = new Guid(0xA75D362E, 0x50FC, 0x4fb7, 0xAC, 0x2C, 0xA8, 0xBE, 0xAA, 0x31, 0x44, 0x93),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder StartMenu = new AdvancedUserShellFolder {
			Name = "StartMenu",
			WindowsIdentifier = new Guid(0x625B53C3, 0xAB48, 0x4EC1, 0xBA, 0x1F, 0xA1, 0xEF, 0x41, 0x46, 0xFC, 0x19),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Startup = new AdvancedUserShellFolder {
			Name = "Startup",
			WindowsIdentifier = new Guid(0xB97D20BB, 0xF46A, 0x4C97, 0xBA, 0x10, 0x5E, 0x36, 0x08, 0x43, 0x08, 0x54),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder SyncManagerFolder = new AdvancedUserShellFolder {
			Name = "SyncManagerFolder",
			WindowsIdentifier = new Guid(0x43668BF8, 0xC14E, 0x49B2, 0x97, 0xC9, 0x74, 0x77, 0x84, 0xD7, 0x84, 0xB7),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SyncResultsFolder = new AdvancedUserShellFolder {
			Name = "SyncResultsFolder",
			WindowsIdentifier = new Guid(0x289a9a43, 0xbe44, 0x4057, 0xa4, 0x1b, 0x58, 0x7a, 0x76, 0xd7, 0xe7, 0xf9),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SyncSetupFolder = new AdvancedUserShellFolder {
			Name = "SyncSetupFolder",
			WindowsIdentifier = new Guid(0x0F214138, 0xB1D3, 0x4a90, 0xBB, 0xA9, 0x27, 0xCB, 0xC0, 0xC5, 0x38, 0x9A),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder System = new AdvancedUserShellFolder {
			Name = "System",
			WindowsIdentifier = new Guid(0x1AC14E77, 0x02E7, 0x4E5D, 0xB7, 0x44, 0x2E, 0xB1, 0xAE, 0x51, 0x98, 0xB7),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder SystemX86 = new AdvancedUserShellFolder {
			Name = "SystemX86",
			WindowsIdentifier = new Guid(0xD65231B0, 0xB2F1, 0x4857, 0xA4, 0xCE, 0xA8, 0xE7, 0xC6, 0xEA, 0x7D, 0x27),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Templates = new AdvancedUserShellFolder {
			Name = "Templates",
			WindowsIdentifier = new Guid(0xA63293E8, 0x664E, 0x48DB, 0xA0, 0x79, 0xDF, 0x75, 0x9E, 0x05, 0x09, 0xF7),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder TreeProperties = new AdvancedUserShellFolder {
			Name = "TreeProperties",
			WindowsIdentifier = new Guid(0x5b3749ad, 0xb49f, 0x49c1, 0x83, 0xeb, 0x15, 0x37, 0x0f, 0xbd, 0x48, 0x82),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder UserProfiles = new AdvancedUserShellFolder {
			Name = "UserProfiles",
			WindowsIdentifier = new Guid(0x0762D272, 0xC50A, 0x4BB0, 0xA3, 0x82, 0x69, 0x7D, 0xCD, 0x72, 0x9B, 0x80),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder UsersFiles = new AdvancedUserShellFolder {
			Name = "UsersFiles",
			WindowsIdentifier = new Guid(0xf3ce0f7c, 0x4901, 0x4acc, 0x86, 0x48, 0xd5, 0xd4, 0x4b, 0x04, 0xef, 0x8f),
			Undefined = true, IsUserSpecific = false, ShouldBeEdited = false
		};

		public static readonly AdvancedUserShellFolder Videos = new AdvancedUserShellFolder {
			Name = "Videos",
			WindowsIdentifier = new Guid(0x18989B1D, 0x99B5, 0x455B, 0x84, 0x1C, 0xAB, 0x7C, 0x74, 0xE4, 0xDD, 0xFC),
			Undefined = false, IsUserSpecific = true, ShouldBeEdited = true
		};

		public static readonly AdvancedUserShellFolder Windows = new AdvancedUserShellFolder {
			Name = "Windows",
			WindowsIdentifier = new Guid(0xF38BF404, 0x1D43, 0x42F2, 0x93, 0x05, 0x67, 0xDE, 0x0B, 0x28, 0xFC, 0x23),
			Undefined = false, IsUserSpecific = false, ShouldBeEdited = false
		};
	}
}