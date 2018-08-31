!include "MUI.nsh"
!include LogicLib.nsh
!include StrContainsFun.nsi

Name "StorageManagementTool"
OutFile "installer.exe"
RequestExecutionLevel highest
InstallDir "$PROGRAMFILES\StorageManagementTool"
ShowInstDetails show

!define APPNAME "StorageManagementTool"
!define COMPANYNAME "TheMinefighter"
!define DESCRIPTION "A short description goes here"
# These three must be integers
!define VERSIONMAJOR 1
!define VERSIONMINOR 1
!define VERSIONBUILD 1
# These will be displayed by the "Click here for support information" link in "Add/Remove Programs"
# It is possible to use "mailto:" links in here to open the email client
!define HELPURL "https://github.com/TheMinefighter/StorageManagementTool/releases" # "Support Information" link
!define UPDATEURL "https://github.com/TheMinefighter/TheMinefighter.github.io/releases" # "Product Updates" link
!define ABOUTURL "https://theminefighter.github.io/" # "Publisher" link
# This is the size (in kB) of all the files copied into "Program Files"
!define INSTALLSIZE 10000

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "License.rtf"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
Var StartMenuFolder
!insertmacro MUI_PAGE_STARTMENU "Application" $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_TEXT "Start a shortcut"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_LANGUAGE "English"

Var IntSMName
 
Section "StorageManagementTool Core"
	SectionIn RO
	CreateDirectory $INSTDIR\bin
	CreateDirectory $INSTDIR\bin\de-DE
	CreateDirectory $INSTDIR\bin\en-US
	SetOutPath $INSTDIR\bin
	File ..\StorageManagementCore\bin\Release\*
	SetOutPath $INSTDIR\bin\de-DE
	File ..\StorageManagementCore\bin\Release\de-DE\*
	SetOutPath $INSTDIR\bin\en-US
	File ..\StorageManagementCore\bin\Release\en-US\*
	;SetOutPath $INSTDIR\uninstall
	WriteUninstaller "uninstall.exe"
	${StrContains} $0 "_FOLDER" $StartMenuFolder
	StrCmp $0 "" notfound
	StrCpy $StartMenuFolder "[NONE]"
	Goto done
	notfound:
	CreateDirectory $StartMenuFolder
	CreateShortCut $StartMenuFolder\StorageManagementTool.lnk $INSTDIR\bin\StorageManagementCLI.bat "" "$INSTDIR\bin\icon.ico"
	done:
  	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayName" "${COMPANYNAME} - ${APPNAME} - ${DESCRIPTION}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "UninstallString" "$\"$INSTDIR\uninstall\uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\uninstall\uninstall.exe$\" /S"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayIcon" "$\"$INSTDIR\logo.ico$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "Publisher" "$\"${COMPANYNAME}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "HelpLink" "$\"${HELPURL}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "URLUpdateInfo" "$\"${UPDATEURL}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "URLInfoAbout" "$\"${ABOUTURL}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayVersion" "$\"${VERSIONMAJOR}.${VERSIONMINOR}.${VERSIONBUILD}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InternalStartmenuFolder" "${StartMenuFolder}"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "VersionMajor" ${VERSIONMAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "VersionMinor" ${VERSIONMINOR}
	# There is no option for modifying or repairing the install
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoRepair" 1
	# Set the INSTALLSIZE constant (!defined at the top of this script) so Add/Remove Programs can accurately report the size
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "EstimatedSize" ${INSTALLSIZE}
SectionEnd

Section "Automatic Update (Can be changed later)"
	CreateDirectory $APPDATA\StorageManagementTool
	SetOutPath $APPDATA\StorageManagementTool
	File .\EnableUpdates\MainConfiguration.json
SectionEnd

;Section "Send To HDD (Can be changed later)"
;	CreateShortCut
;SectionEnd

Function LaunchLink
  MessageBox MB_OK "Reached LaunchLink $\r$\n \
                   SMPROGRAMS: $SMPROGRAMS  $\r$\n \
                   Start Menu Folder: $STARTMENU_FOLDER $\r$\n \
                   InstallDirectory: $INSTDIR "
  ExecShell "" "$INSTDIR\bin\StorageManagementCore.exe"
FunctionEnd

Section "uninstall"
	Goto done
    ReadRegStr $IntSMName HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InternalStartmenuFolder"
	;This could theoretically also be true if the user is called _FOLDER but nobody would do that
	StrCmp $IntSMName "[NONE]" done
	RMDir /r ${IntSMName}
	done:
	# Remove Start Menu launcher
	;delete "$SMPROGRAMS\${COMPANYNAME}\${APPNAME}.lnk"
	# Try to remove the Start Menu folder - this will only happen if it is empty
	;rmDir -r "$SMPROGRAMS\${COMPANYNAME}"
 
	# Remove files
	;delete $INSTDIR\app.exe
	;delete $INSTDIR\logo.ico
 
	# Always delete uninstaller as the last action
	;delete $INSTDIR\uninstall.exe
 
	# Try to remove the install directory - this will only happen if it is empty
	RMDir /r "${INSTDIR}\bin"
 
	# Remove uninstaller information from the registry
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}"
SectionEnd




