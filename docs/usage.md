# Moving files and folders
# Customize Shellfolders
# Customize windows indexing
# Monitoring folders
# Configure pagefiles
# Send to HDD
# Further settings
## Language
The GUI exists (for 99%) in english and german to change that go to Settings > Language, select the language you want and click "Safe and restart"
## Credentials & Authorizations
Such a programm needs many authorizations, but due to the fact that it is open-source, so you can read each line of code and look what I do with your authorizations. First of all you can run the program as administrator to access most functionality. If you dom't want to start it as admin every time yourself, you can enable "Ask for administrator priviliges on startup" in the settings tab.

For some operations (which I am trying to solve differently the program needs more rights due to windows weird user Account Control (UAC), which makes it required for the program to run some commands "as user" therefore it needs the name and password of an administrator account. These are NOT stored between sessions. These are securely stored as [SecureString and thereby encrypted](https://msdn.microsoft.com/en-us/library/system.security.securestring(v=vs.110).aspx#Anchor_5), so that even if my program would want to it couldn't get your password in plain text. To erase these from memory in a session go to Settings > Authorizations and click "Delete Credentials" thereby your encrypted password gets disposed.
