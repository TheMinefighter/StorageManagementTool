; This script is the result of much copy pasting from the nsis documentation and some own work.
; I would not consider any of the parts represented in this documentes as significant.
; Anyway acknowledgement is hereby granted to all contributors of the nsis documentation
!include "MUI.nsh"
!include LogicLib.nsh
;!include StrContainsFun.nsi
!include StrRepFun.nsi
Name "StorageManagementTool"
OutFile "installer.exe"
RequestExecutionLevel highest
InstallDir "$PROGRAMFILES\StorageManagementTool"
ShowInstDetails show
!define MUI_ICON "icon.ico"
!define APPNAME "StorageManagementTool"
!define COMPANYNAME "TheMinefighter"
!define DESCRIPTION "A tool for managing the storage of your pc"
!define VERSIONMAJOR 1
!define VERSIONMINOR 2
!define VERSIONBUILD 3
!define HELPURL "https://theminefighter.github.io/StorageManagementTool/"
!define UPDATEURL "https://github.com/TheMinefighter/StorageManagementTool/releases" 
!define ABOUTURL "https://theminefighter.github.io/" 
!define INSTALLSIZE 12000


!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "License.rtf"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
Var StartMenuFolder
!insertmacro MUI_PAGE_STARTMENU "Application" $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_TEXT "Start the StorageManagementTool"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_LANGUAGE "English"



Var IntSMName
 
Section "StorageManagementTool Core" CoreComponent
	SectionIn RO
	;LogSet On
	SetOverwrite try
 	Push $StartMenuFolder
 	Call ValidateSM
	Pop $StartMenuFolder
	;MessageBox MB_OK $StartMenuFolder ;For debugging purposes
	CreateDirectory $INSTDIR\bin
	CreateDirectory $INSTDIR\bin\de-DE
	CreateDirectory $INSTDIR\bin\en-US
	SetOutPath $INSTDIR\bin
	File /x *.xml /x *.pdb ..\StorageManagementCore\bin\Release\*
	SetOutPath $INSTDIR\bin\de-DE
	File ..\StorageManagementCore\bin\Release\de-DE\*
	SetOutPath $INSTDIR\bin\en-US
	File ..\StorageManagementCore\bin\Release\en-US\*
	WriteUninstaller "uninstall.exe"	
	StrCmp $StartMenuFolder "<None>" SkipSM
	CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
	SetOutPath $INSTDIR\bin
	CreateShortCut "$SMPROGRAMS\$StartMenuFolder\StorageManagementTool.lnk" "$INSTDIR\bin\StorageManagementCLI.bat" "" "$INSTDIR\bin\icon.ico"
	;MessageBox MB_OK "$SMPROGRAMS\$StartMenuFolder\StorageManagementTool.lnk"
	;MessageBox MB_OK "$INSTDIR\bin\StorageManagementCLI.bat"
	;MessageBox MB_OK "$INSTDIR\bin\icon.ico"
	;Abort
	SkipSM:
  	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayIcon" "$\"$INSTDIR\bin\icon.ico$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "Publisher" "${COMPANYNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "HelpLink" "${HELPURL}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLUpdateInfo" "${UPDATEURL}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLInfoAbout" "${ABOUTURL}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayVersion" "1.2-b-2.0"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "InternalStartmenuFolder" "$StartMenuFolder"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "VersionMajor" ${VERSIONMAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "VersionMinor" ${VERSIONMINOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoRepair" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "EstimatedSize" ${INSTALLSIZE}

SectionEnd

Section "Automatic Update (Can be changed later)" AutoUpdates
	CreateDirectory $APPDATA\StorageManagementTool
	SetOutPath $APPDATA\StorageManagementTool
	File .\EnableUpdates\MainConfiguration.json
SectionEnd

Section "Protect installation folder (strongly recommended)" ProtectInstall
	ExecShell "-ProtectInstallationFolder" "$INSTDIR\bin\StorageManagementCore.exe"
SectionEnd

;Section "Send To HDD (Can be changed later)"
;	CreateShortCut
;SectionEnd

Function ValidateSM 
	Pop $1
	Push $1
   Push "\"
   Push ""
   Call StrRep
   Pop $1
	StrCpy $2 $1 1
	strCmp $2 ">" SMError
	StrCpy $2 $1 "" -1 
   StrCmp $2 "." SMError

	StrCmp $1 "CON" SMError
   StrCmp $1 "PRN" SMError
   StrCmp $1 "AUX" SMError
   StrCmp $1 "NUL" SMError
   StrCmp $1 "COM1" SMError
   StrCmp $1 "COM2" SMError
   StrCmp $1 "COM3" SMError
   StrCmp $1 "COM4" SMError
   StrCmp $1 "COM5" SMError
   StrCmp $1 "COM6" SMError
   StrCmp $1 "COM7" SMError
   StrCmp $1 "COM8" SMError
   StrCmp $1 "COM9" SMError
   StrCmp $1 "LPT1" SMError
   StrCmp $1 "LPT2" SMError
	StrCmp $1 "LPT3" SMError
	StrCmp $1 "LPT4" SMError
	StrCmp $1 "LPT5" SMError
   StrCmp $1 "LPT6" SMError
   StrCmp $1 "LPT7" SMError
   StrCmp $1 "LPT8" SMError
   StrCmp $1 "LPT9" SMError

   Push $1
	Return
	SMError:
	StrCpy $1 "<None>"
	Push $1
	Return
FunctionEnd
Function LaunchLink
   ExecShell "" "$INSTDIR\bin\StorageManagementCore.exe"
FunctionEnd

Section "uninstall"
	ExecShell "" "$WINDIR\System32\SCHTASKS.exe" "/DELETE /TN StorageManagementTool_SSDMonitoring /F"
	ReadRegStr $IntSMName HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InternalStartmenuFolder"
	;This could theoretically also be true if the user is called _FOLDER but nobody would do that
	StrCmp $IntSMName "<NONE>" done
	RMDir /r $IntSMName
	done:
	RMDir /r $INSTDIR\bin
 	
	# Remove uninstaller information from the registry
	
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
	RMDir /r $INSTDIR
SectionEnd
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${AutoUpdates} "Enables fully automatic updates. Update settings can be changed later on."
!insertmacro MUI_DESCRIPTION_TEXT ${CoreComponent} "The main part of the StorageManagementTool"
!insertmacro MUI_DESCRIPTION_TEXT ${ProtectInstall} "Protect the installation folder, so that it can only be modified with administrator priviliges. Disabling this might open security vulnerabilities!"
!insertmacro MUI_FUNCTION_DESCRIPTION_END