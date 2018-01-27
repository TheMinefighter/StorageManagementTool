using System;

namespace StorageManagementTool
{
    public static class UIStrings
    {
        public enum UILanguage
        {
            German,
            English
        }

        public static string Error
        {
            get
            {
                switch (Session.Singleton.CurrentLanguage)
                {
                    case UILanguage.German:
                        return"Fehler";
                    case UILanguage.English:
                        return "error";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static class WrapperStrings
        {
            public static string ErrorReadingRegistry
            {
                get
                {
                    switch (Session.Singleton.CurrentLanguage)
                    {
                        case UILanguage.German:
                            return "Bei dem lesen des Registry Wertes {0}  unter {1} ist ein Fehler aufgetreten.";
                        case UILanguage.English:
                            return "An error occurred while reading the registry value {0} located under the key {1}.";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public static string FileToRunNotAvailable
            {
                get
                {
                    switch (Session.Singleton.CurrentLanguage)
                    {
                        case UILanguage.German:
                            return
                                "Das Programm \"{0}\" Konnte nicht ausgeführt werden, da dieses nicht unter dem erwarteten Pfad verfügbar war. Möchten sie den richtigen Pfad des Programms auswählen?";
                        case UILanguage.English:
                            return
                                "The program \"{0}\" couldn´t be executed, due to the fact that it was not available under the expected path. Do you want to select the correct Path of the program";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public static string WrongEndingOfExecutable
            {
                get
                {
                    switch (Session.Singleton.CurrentLanguage)
                    {
                        case UILanguage.German:
                            return
                                "Es sollte das Program\"{0}\" ausgeführt werden, jedoch ist nicht bekannt das Dateien mit der Dateiendung {1} unter Windows ausführbar sind. Soll trotzdem versucht werden diese Datei auszuführen?";
                        case UILanguage.English:
                            return
                                "The program \"{0}\" is supposed to be be executed, but files with the ending {1} are probably not executeable using Windows. Do you want to try to execute that file anyway?";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public static string ExecuteableAdminError
            {
                get
                {
                    switch (Session.Singleton.CurrentLanguage)
                    {
                        case UILanguage.German:
                            return
                                "Fehler beim ausführen der Datei \" {0} \": der Nutzer hat dem Programm den Zugriff auf Administroteren Privilegien verwehrt, die für dessen Ausführung wahrscheinlich nötig gewesen wären. Soll das Programm den Vorgang abbrechen, den Vorgang wiederholen oder das Problem ignorieren und den Befehl ohne Adminstratorrechte ausführen";
                        case UILanguage.English:
                            return
                                "Error while executing \"{0}\": The user denied the program the access to administrator privileges.";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public static string RegistrySetSecurityException
            {
                get
                {
                    switch (Session.Singleton.CurrentLanguage)
                    {
                        case UILanguage.German:
                            return
                                "Fehler beim setzen des Registry Wertes \"{0}\", welcher unter \"{1}\" gespeichert ist, auf den wert\"{2}\" mit dem Typ \"{3}\"ist ein Fehler aufgetreten, da dieser Wert schreibgeschützt ist";
                        case UILanguage.English:
                            return
                                "Error while setting the registry value \"{0}\" located under \"{1}\", which has the registry value type \"{3}\", to \"{2}\", due to the fact that the value is readonly.";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public static string RegistrySetUnauthorizedAccess
            {
                get
                {
                    switch (Session.Singleton.CurrentLanguage)
                    {
                        case UILanguage.German:
                            return "Fehler beim setzen des Registry Wertes\"{0}\", welcher unter \"{1}\" gespeichert ist," +
                                   " auf den wert\"{2}\" mit dem Typ \"{3}\"ist ein Fehler aufgetreten, da dieses Programm" +
                                   " aktuell nicht auf Administratorenprivilegien zurückgreifen kann. Möchten sie die" +
                                   " Anwendung mit Administratoren-Privilegien neustarten?";
                        case UILanguage.English:
                            return "While reading the registry value {0] located under the key {1} occurred an error";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}