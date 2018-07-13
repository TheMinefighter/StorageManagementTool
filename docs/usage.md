# Moving files and folders
You can simply move files or whole folders from your SSD to your HDD or NAS. You can do that in the program or via the [Send to HDD feature](usage.html#Send-to-HDD).

In the main tab you can select which file(s) or folder(s) you want to move, or paste paths, separated with semicolons.
# Customize Shellfolders
# Customize windows indexing
# Monitoring folders
# Configure pagefiles

# Send to HDD
The program has a feature which adds a 'Store on HDD' option to the 'Send to' option of the windows explorer.
When you want to use that feature check 'Show Store on HDD in Send to menu of the explorer' in the Settings tab
# Further settings

## Language
The GUI exists (for 99%) in english and german to change that go to Settings > Language,
select the language you want and click "Safe and restart"
## Credentials & Authorizations
Such a program needs many authorizations, but due to the fact that it is open-source,
so you can read each line of code and look what I do with your authorizations.
First of all you can run the program as administrator to access most functionality.
If you dom't want to start it as admin every time yourself,
you can enable "Ask for administrator priviliges on startup" in the settings tab.

For some operations (which I am trying to solve differently) the program needs more rights due to windows weird User Account Control (UAC),
 which makes it required for the program to run some commands "as user" therefore it needs the name and password of an administrator account. These are NOT stored between sessions. These are securely stored as [SecureString and thereby encrypted](https://msdn.microsoft.com/en-us/library/system.security.securestring(v=vs.110).aspx#Anchor_5), so that even if my program would want to it couldn't get your password in plain text. To erase these from memory in a session go to Settings > Authorizations and click "Delete Credentials" thereby your encrypted password gets disposed.
