# Moving files and folders
You can simply move files or whole folders from your SSD to your HDD or NAS. You can do that in the program or via the [Send to HDD feature](usage.html#Send-to-HDD).

In the main tab you can select which file(s) or folder(s) you want to move, choose (multiple) from the suggestions or paste paths, separated with semicolons.
 
Then you can specify the path you want to move the data to:
By default it moves your data (e.g. C:\\Users\\YourName\\AppData\\Local\\YourDataDirectory\\) to your the configured export path (e.g. F:\\SSD).
From there the program reconstructs the original path (e.g. F:\\SSD\\C\\Users\\YourName\\AppData\\Local\\YourDataDirectory\\)  
You can disable that reconstruction by checking 'Use absolute path'. Furthermore you can change that path and (optionally) make that changed path the default.
# Customize ShellFolders
You can customize the windows ShellFolders (e.g. Documents, Downloads..) from this program. For these operations the program has to run as administrator.

To do that go to the 'Edit ShellFolders' tab. There you can select the ShellFolder you want to edit.
Then you can inspect the current path, select a new path and apply that change.
 
Furthermore you can decide whether to move existing items (strongly recommended) and whether to change dependent ShellFolders.
[Please touch these settings only if you really know what you are doing.](warnings.html)
 
# Customize windows indexing location

You can change the location where windows indexing files are located. Therefore enter the 'Search indexing' tab.
There you can change the location of windows search indexing.
**This feature is in alpha.**

# Monitoring folders

The program is able to monitor folders (e.g. %localappdata%). If there is a new subfolder or file created the program can react to that it 3 ways:
- Ignore that
- Move the file / folder to the preconfigured path
- Ask which of the options to run

To configure that feature fo to the 'SSD Monitoring' tab. There you can enable SSD Monitoring and  add, remove or change monitored folders
# Configure pagefiles
In the  pagefiles tab you can customize the three pagefiles windows has
## pagefile.sys
You can add and remove pagefiles and adjust their size, and apply the configuration

## swapfile.sys
You can set the drive of the swapfile
**This feature is in alpha.**

## hibfil.sys
You can en- or disable the hibfil.sys 

# Send to HDD
The program has a feature which adds a 'Store on HDD' option to the 'Send to' option of the windows explorer.
When you want to use that feature check 'Show Store on HDD in Send to menu of the explorer' in the Settings tab
# Further settings
Such a big project has multiple settings (and many more planned), which are documented in the following sections:
## Language
The GUI exists (for 99.8%) in english and german to change that go to Settings > Language,
select the language you want and click "Safe and restart"
## Credentials & Authorizations
Such a program needs many authorizations, but due to the fact that it is open-source,
so you can read each line of code and look what the program does with your authorizations.
First of all you can run the program as administrator to access most functionality.
If you dom't want to start it as admin every time yourself,
you can enable "Ask for administrator privileges on startup" in the settings tab.

For some operations (which I am trying to solve differently) the program needs more rights due to windows weird User Account Control (UAC),
which makes it required for the program to run some commands "as user" therefore it needs the name and password of an administrator account.
These are NOT stored between sessions.
These are securely stored as [SecureString and thereby encrypted](https://msdn.microsoft.com/en-us/library/system.security.securestring(v=vs.110).aspx#Anchor_5),
so that even if my program would want to it could not get your password in plain text.
To erase these from memory in a session go to Settings > Authorizations and click "Delete Credentials" thereby your encrypted password gets disposed.