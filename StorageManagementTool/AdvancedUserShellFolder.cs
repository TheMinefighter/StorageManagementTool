using System;
using System.Linq;

namespace StorageManagementTool {
   public partial struct AdvancedUserShellFolder {
      public static AdvancedUserShellFolder[] AllUSF;
      public bool shouldBeEdited;
      public Guid WindowsIdentifier;
      public string Name;
      public bool Undefined;
      public string LocalizedName;
      public string DefaultValue;
      public bool IsUserSpecific;

      public static AdvancedUserShellFolder GetUSF(string name) => AllUSF.First(x => x.Name == name);
      public static AdvancedUserShellFolder GetUSF(Guid windowsIdentifier) => AllUSF.First(x => x.WindowsIdentifier == windowsIdentifier);
      
      public static void LoadUSF() {
         string systemDrive = Environment.GetEnvironmentVariable("systemdrive");
         string userDir = Environment.GetEnvironmentVariable("userprofile");
         //Def without admin will probably retest in future; new user
         AllUSF=new AdvancedUserShellFolder[] {
         new AdvancedUserShellFolder{Name="AddNewPrograms",WindowsIdentifier= new Guid("de61d971-5ebc-4f02-a3a9-6c82895e5c04"),Undefined = true},
         new AdvancedUserShellFolder{Name="AdminTools",WindowsIdentifier= new Guid("724EF170-A42D-4FEF-9F26-B60E846FBA4F"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="AppUpdates",WindowsIdentifier= new Guid("a305ce99-f527-492b-8b1a-7e76fa98d6e4"),Undefined = true},
         new AdvancedUserShellFolder{Name="CDBurning",WindowsIdentifier= new Guid("9E52AB10-F80D-49DF-ACB8-4330F5687855"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="ChangeRemovePrograms",WindowsIdentifier= new Guid("df7266ac-9274-4867-8d55-3bd661de872d"),Undefined = true},
         new AdvancedUserShellFolder{Name="CommonAdminTools",WindowsIdentifier= new Guid("D0384E7D-BAC3-4797-8F14-CBA229B392B5")},
         new AdvancedUserShellFolder{Name="CommonOEMLinks",WindowsIdentifier= new Guid("C1BAE2D0-10DF-4334-BEDD-7AA20B227A9D")},
         new AdvancedUserShellFolder{Name="CommonPrograms",WindowsIdentifier= new Guid("0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8")},
         new AdvancedUserShellFolder{Name="CommonStartMenu",WindowsIdentifier= new Guid("A4115719-D62E-491D-AA7C-E74B8BE3B067")},
         new AdvancedUserShellFolder{Name="CommonStartup",WindowsIdentifier= new Guid("82A5EA35-D9CD-47C5-9629-E15D2F714E6E")},
         new AdvancedUserShellFolder{Name="CommonTemplates",WindowsIdentifier= new Guid("B94237E7-57AC-4347-9151-B08C6C32D1F7")},
         new AdvancedUserShellFolder{Name="ComputerFolder",WindowsIdentifier= new Guid("0AC0837C-BBF8-452A-850D-79D08E667CA7"),Undefined = true},
         new AdvancedUserShellFolder{Name="ConflictFolder",WindowsIdentifier= new Guid("4bfefb45-347d-4006-a5be-ac0cb0567192"),Undefined = true},
         new AdvancedUserShellFolder{Name="ConnectionsFolder",WindowsIdentifier= new Guid("6F0CD92B-2E97-45D1-88FF-B0D186B8DEDD"),Undefined = true},
         new AdvancedUserShellFolder{Name="Contacts",WindowsIdentifier= new Guid("56784854-C6CB-462b-8169-88E350ACB882"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="ControlPanelFolder",WindowsIdentifier= new Guid("82A74AEB-AEB4-465C-A014-D097EE346D63"),Undefined = true},
         new AdvancedUserShellFolder{Name="Cookies",WindowsIdentifier= new Guid("2B0F765D-C0E9-4171-908E-08A611B84FF6"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Desktop",WindowsIdentifier= new Guid("B4BFCC3A-DB2C-424C-B029-7FE99A87C641"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Documents",WindowsIdentifier= new Guid("FDD39AD0-238F-46AF-ADB4-6C85480369C7"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Downloads",WindowsIdentifier= new Guid("374DE290-123F-4565-9164-39C4925E467B"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Favorites",WindowsIdentifier= new Guid("1777F761-68AD-4D8A-87BD-30B759FA33DD"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Fonts",WindowsIdentifier= new Guid("FD228CB7-AE11-4AE3-864C-16F3910AB8FE")},
         new AdvancedUserShellFolder{Name="Games",WindowsIdentifier= new Guid("CAC52C1A-B53D-4edc-92D7-6B2E8AC19434"),Undefined = true},
         new AdvancedUserShellFolder{Name="GameTasks",WindowsIdentifier= new Guid("054FAE61-4DD8-4787-80B6-090220C4B700"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="History",WindowsIdentifier= new Guid("D9DC8A3B-B784-432E-A781-5A1130A75963"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="InternetCache",WindowsIdentifier= new Guid("352481E8-33BE-4251-BA85-6007CAEDCF9D"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="InternetFolder",WindowsIdentifier= new Guid("4D9F7874-4E0C-4904-967B-40B0D20C3E4B"),Undefined = true},
         new AdvancedUserShellFolder{Name="Links",WindowsIdentifier= new Guid("bfb9d5e0-c6a9-404c-b2b2-ae6db6af4968"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="LocalAppData",WindowsIdentifier= new Guid("F1B32785-6FBA-4FCF-9D55-7B8E7F157091"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="LocalAppDataLow",WindowsIdentifier= new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="LocalizedResourcesDir",WindowsIdentifier= new Guid("2A00375E-224C-49DE-B8D1-440DF7EF3DDC"),Undefined = true},
         new AdvancedUserShellFolder{Name="Music",WindowsIdentifier= new Guid("4BD8D571-6D19-48D3-BE97-422220080E43"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="NetHood",WindowsIdentifier= new Guid("C5ABBF53-E17F-4121-8900-86626FC2C973"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="NetworkFolder",WindowsIdentifier= new Guid("D20BEEC4-5CA8-4905-AE3B-BF251EA09B53"),Undefined = true},
         new AdvancedUserShellFolder{Name="OriginalImages",WindowsIdentifier= new Guid("2C36C0AA-5812-4b87-BFD0-4CD0DFB19B39"),Undefined = true},
         new AdvancedUserShellFolder{Name="PhotoAlbums",WindowsIdentifier= new Guid("69D2CF90-FC33-4FB7-9A0C-EBB0F0FCB43C"),Undefined = true},
         new AdvancedUserShellFolder{Name="Pictures",WindowsIdentifier= new Guid("33E28130-4E1E-4676-835A-98395C3BC3BB"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Playlists",WindowsIdentifier= new Guid("DE92C1C7-837F-4F69-A3BB-86E631204A23"),Undefined = true},
         new AdvancedUserShellFolder{Name="PrintersFolder",WindowsIdentifier= new Guid("76FC4E2D-D6AD-4519-A663-37BD56068185"),Undefined = true},
         new AdvancedUserShellFolder{Name="PrintHood",WindowsIdentifier= new Guid("9274BD8D-CFD1-41C3-B35E-B13F55A758F4"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Profile",WindowsIdentifier= new Guid("5E6C858F-0E22-4760-9AFE-EA3317B67173"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="ProgramData",WindowsIdentifier= new Guid("62AB5D82-FDC1-4DC3-A9DD-070D1D495D97")},
         new AdvancedUserShellFolder{Name="ProgramFiles",WindowsIdentifier= new Guid("905e63b6-c1bf-494e-b29c-65b732d3d21a")},
         new AdvancedUserShellFolder{Name="ProgramFilesX64",WindowsIdentifier= new Guid("6D809377-6AF0-444b-8957-A3773F02200E"),Undefined = true},
         new AdvancedUserShellFolder{Name="ProgramFilesX86",WindowsIdentifier= new Guid("7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E")},
         new AdvancedUserShellFolder{Name="ProgramFilesCommon",WindowsIdentifier= new Guid("F7F1ED05-9F6D-47A2-AAAE-29D317C6F066")},
         new AdvancedUserShellFolder{Name="ProgramFilesCommonX64",WindowsIdentifier= new Guid("6365D5A7-0F0D-45E5-87F6-0DA56B6A4F7D"),Undefined = true},
         new AdvancedUserShellFolder{Name="ProgramFilesCommonX86",WindowsIdentifier= new Guid("DE974D24-D9C6-4D3E-BF91-F4455120B917")},
         new AdvancedUserShellFolder{Name="Programs",WindowsIdentifier= new Guid("A77F5D77-2E2B-44C3-A6A2-ABA601054A51"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Public",WindowsIdentifier= new Guid("DFDF76A2-C82A-4D63-906A-5644AC457385")},
         new AdvancedUserShellFolder{Name="PublicDesktop",WindowsIdentifier= new Guid("C4AA340D-F20F-4863-AFEF-F87EF2E6BA25")},
         new AdvancedUserShellFolder{Name="PublicDocuments",WindowsIdentifier= new Guid("ED4824AF-DCE4-45A8-81E2-FC7965083634")},
         new AdvancedUserShellFolder{Name="PublicDownloads",WindowsIdentifier= new Guid("3D644C9B-1FB8-4f30-9B45-F670235F79C0")},
         new AdvancedUserShellFolder{Name="PublicGameTasks",WindowsIdentifier= new Guid("DEBF2536-E1A8-4c59-B6A2-414586476AEA")},
         new AdvancedUserShellFolder{Name="PublicMusic",WindowsIdentifier= new Guid("3214FAB5-9757-4298-BB61-92A9DEAA44FF")},
         new AdvancedUserShellFolder{Name="PublicPictures",WindowsIdentifier= new Guid("B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5")},
         new AdvancedUserShellFolder{Name="PublicVideos",WindowsIdentifier= new Guid("2400183A-6185-49FB-A2D8-4A392A602BA3")},
         new AdvancedUserShellFolder{Name="QuickLaunch",WindowsIdentifier= new Guid("52a4f021-7b75-48a9-9f6b-4b87a210bc8f"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Recent",WindowsIdentifier= new Guid("AE50C081-EBD2-438A-8655-8A092E34987A"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="RecycleBinFolder",WindowsIdentifier= new Guid("B7534046-3ECB-4C18-BE4E-64CD4CB7D6AC"),Undefined = true},
         new AdvancedUserShellFolder{Name="ResourceDir",WindowsIdentifier= new Guid("8AD10C31-2ADB-4296-A8F7-E4701232C972")},
         new AdvancedUserShellFolder{Name="RoamingAppData",WindowsIdentifier= new Guid("3EB685DB-65F9-4CF6-A03A-E3EF65729F3D"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="SampleMusic",WindowsIdentifier= new Guid("B250C668-F57D-4EE1-A63C-290EE7D1AA1F"),Undefined = true},
         new AdvancedUserShellFolder{Name="SamplePictures",WindowsIdentifier= new Guid("C4900540-2379-4C75-844B-64E6FAF8716B"),Undefined = true},
         new AdvancedUserShellFolder{Name="SamplePlaylists",WindowsIdentifier= new Guid("15CA69B3-30EE-49C1-ACE1-6B5EC372AFB5"),Undefined = true},
         new AdvancedUserShellFolder{Name="SampleVideos",WindowsIdentifier= new Guid("859EAD94-2E85-48AD-A71A-0969CB56A6CD"),Undefined = true},
         new AdvancedUserShellFolder{Name="SavedGames",WindowsIdentifier= new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="SavedSearches",WindowsIdentifier= new Guid("7d1d3a04-debb-4115-95cf-2f29da2920da"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="SEARCH_CSC",WindowsIdentifier= new Guid("ee32e446-31ca-4aba-814f-a5ebd2fd6d5e"),Undefined = true},
         new AdvancedUserShellFolder{Name="SEARCH_MAPI",WindowsIdentifier= new Guid("98ec0e18-2098-4d44-8644-66979315a281"),Undefined = true},
         new AdvancedUserShellFolder{Name="SearchHome",WindowsIdentifier= new Guid("190337d1-b8ca-4121-a639-6d472d16972a"),Undefined = true},
         new AdvancedUserShellFolder{Name="SendTo",WindowsIdentifier= new Guid("8983036C-27C0-404B-8F08-102D10DCFD74"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="SidebarDefaultParts",WindowsIdentifier= new Guid("7B396E54-9EC5-4300-BE0A-2482EBAE1A26"),Undefined = true},
         new AdvancedUserShellFolder{Name="SidebarParts",WindowsIdentifier= new Guid("A75D362E-50FC-4fb7-AC2C-A8BEAA314493"),Undefined = true},
         new AdvancedUserShellFolder{Name="StartMenu",WindowsIdentifier= new Guid("625B53C3-AB48-4EC1-BA1F-A1EF4146FC19"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Startup",WindowsIdentifier= new Guid("B97D20BB-F46A-4C97-BA10-5E3608430854"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="SyncManagerFolder",WindowsIdentifier= new Guid("43668BF8-C14E-49B2-97C9-747784D784B7"),Undefined = true},
         new AdvancedUserShellFolder{Name="SyncResultsFolder",WindowsIdentifier= new Guid("289a9a43-be44-4057-a41b-587a76d7e7f9"),Undefined = true},
         new AdvancedUserShellFolder{Name="SyncSetupFolder",WindowsIdentifier= new Guid("0F214138-B1D3-4a90-BBA9-27CBC0C5389A"),Undefined = true},
         new AdvancedUserShellFolder{Name="System",WindowsIdentifier= new Guid("1AC14E77-02E7-4E5D-B744-2EB1AE5198B7")},
         new AdvancedUserShellFolder{Name="SystemX86",WindowsIdentifier= new Guid("D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27")},
         new AdvancedUserShellFolder{Name="Templates",WindowsIdentifier= new Guid("A63293E8-664E-48DB-A079-DF759E0509F7"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="TreeProperties",WindowsIdentifier= new Guid("5b3749ad-b49f-49c1-83eb-15370fbd4882"),Undefined = true},
         new AdvancedUserShellFolder{Name="UserProfiles",WindowsIdentifier= new Guid("0762D272-C50A-4BB0-A382-697DCD729B80")},
         new AdvancedUserShellFolder{Name="UsersFiles",WindowsIdentifier= new Guid("f3ce0f7c-4901-4acc-8648-d5d44b04ef8f"),Undefined = true},
         new AdvancedUserShellFolder{Name="Videos",WindowsIdentifier= new Guid("18989B1D-99B5-455B-841C-AB7C74E4DDFC"),IsUserSpecific = true},
         new AdvancedUserShellFolder{Name="Windows",WindowsIdentifier= new Guid("F38BF404-1D43-42F2-9305-67DE0B28FC23")}
      };}
      
   }
}